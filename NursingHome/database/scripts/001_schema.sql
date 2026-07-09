/* =====================================================================================
   NURSING HOME / ASSISTED LIVING MANAGEMENT SYSTEM — SCHEMA V4
   Dialect: Microsoft SQL Server (T-SQL).
   Ghi chú kỹ thuật quan trọng (đọc trước khi dùng):
   - Yêu cầu "BIGINT AUTO_INCREMENT" (cú pháp MySQL) được dịch sang "BIGINT IDENTITY(1,1)".
   - Yêu cầu "TIMESTAMP WITH TIME ZONE" (cú pháp Postgres) được dịch sang "DATETIMEOFFSET(0)",
     kiểu tương đương của SQL Server, lưu offset múi giờ theo từng bản ghi.
   - Các cột "*_encrypted" dùng VARCHAR(512) để chứa dữ liệu ĐÃ MÃ HÓA (ciphertext dạng Base64
     của AES-256, hoặc dùng Always Encrypted / column-level encryption ở tầng ứng dụng / DB).
     Bản thân DDL không thực hiện mã hóa — đó là trách nhiệm của tầng service.

   Thay đổi so với V3:
   - Khóa chính: THỐNG NHẤT dùng BIGINT IDENTITY(1,1) cho TẤT CẢ các bảng (bỏ UNIQUEIDENTIFIER
     ở các thực thể "mặt tiền" như users/residents/facilities/invoices...) — theo đúng yêu cầu
     gốc "toàn bộ BIGINT AUTO_INCREMENT". Lưu ý đánh đổi: ID tuần tự có thể lộ số thứ tự bản ghi
     (PHI/ID enumeration) — nếu bận tâm, hãy che ID ở tầng API (dùng slug/opaque id trên route).
   - Khóa ngoại (FOREIGN KEY) được tách thành các câu ALTER TABLE ở CUỐI FILE thay vì khai báo
     inline trong CREATE TABLE — nhờ vậy thứ tự tạo bảng không còn phụ thuộc lẫn nhau, và toàn
     bộ index cũng gom về một khối riêng cho dễ quản lý.
   - Bổ sung Module Kho & Tài sản (inventory_categories, durable_medical_equipment,
     consumable_supplies) và bảng incident_timelines (nhật ký dòng thời gian sự cố — thay cho
     chart_lock_events ở V3).
===================================================================================== */

-- =====================================================================================
-- DATABASE INITIALIZATION
--
-- This project is designed to run with Docker Compose.
-- The database is created automatically by docker/db/init.sh before this script executes.
--
-- Docker:
--      CREATE DATABASE [NursingHome];
--
-- Therefore, this script is responsible only for creating database objects
-- (tables, constraints, indexes, views, procedures, seed data...).
--
-- If you execute this script manually using SSMS/Azure Data Studio without Docker,
-- uncomment the CREATE DATABASE / USE block below and comment out the Docker USE.
-- =====================================================================================

/*
CREATE DATABASE NursingHome;
GO

USE NursingHome;
GO
*/

-- Docker mode

USE NursingHome;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =====================================================================================
-- TABLES
-- Toàn bộ CREATE TABLE nằm ở khối này. Khóa ngoại & index được khai báo ở các khối riêng
-- phía dưới (xem PART "INDEXES" và "FOREIGN KEY CONSTRAINTS").
-- =====================================================================================

-- =====================================================================================
-- PART 0: GLOBAL LOOKUP / RBAC (Role-Based Access Control)
-- =====================================================================================

-- EN: System roles (Admin, DON - Director of Nursing, RN, LPN, CNA, Billing, Front Desk...)
-- VI: Vai trò hệ thống, dùng để phân quyền truy cập PHI theo nguyên tắc "minimum necessary".
CREATE TABLE [roles] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [role_name] NVARCHAR(100) UNIQUE NOT NULL,
  [description] NVARCHAR(500),
  [is_deleted] BIT NOT NULL DEFAULT (0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [updated_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- EN: Fine-grained permission codes (e.g. RESIDENT_PHI_VIEW, MAR_ADMINISTER, BILLING_EDIT).
-- VI: Danh sách quyền hạn chi tiết, đặc biệt tách riêng quyền xem PHI/PII để audit chặt.
CREATE TABLE [permissions] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [action_code] NVARCHAR(100) UNIQUE NOT NULL,
  [is_phi_sensitive] BIT NOT NULL DEFAULT (0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- EN: N:N junction mapping roles to their granted permissions.
-- VI: Bảng trung gian N-N gán quyền cho từng vai trò.
CREATE TABLE [role_permissions] (
  [role_id] BIGINT NOT NULL,
  [permission_id] BIGINT NOT NULL,
  PRIMARY KEY ([role_id], [permission_id])
)
GO

-- =====================================================================================
-- PART 1: SECURITY & USER MANAGEMENT
-- =====================================================================================

-- EN: Staff/system users (nurses, CNAs, admins, billing). Password never stored in plain text.
-- VI: Tài khoản nhân viên hệ thống. Không lưu mật khẩu dạng plain text, chỉ lưu hash.
CREATE TABLE [users] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [employee_code] NVARCHAR(50) UNIQUE NOT NULL,
  [email] NVARCHAR(255) UNIQUE NOT NULL,
  [password_hash] NVARCHAR(300) NOT NULL,
  [first_name] NVARCHAR(100) NOT NULL,
  [middle_name] NVARCHAR(100),
  [last_name] NVARCHAR(100) NOT NULL,
  [license_number] NVARCHAR(100),                         -- RN/LPN professional license #
  [phone_number] NVARCHAR(20),
  [status] VARCHAR(20) NOT NULL CHECK (status IN ('ACTIVE','INACTIVE','LOCKED')) DEFAULT 'ACTIVE',
  [mfa_enabled] BIT NOT NULL DEFAULT (0),                 -- HIPAA: khuyến nghị bắt buộc MFA cho tài khoản có quyền PHI
  [last_login_at] DATETIMEOFFSET(0),
  [role_id] BIGINT NOT NULL,
  [is_deleted] BIT NOT NULL DEFAULT (0),
  [deleted_at] DATETIMEOFFSET(0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [updated_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- =====================================================================================
-- PART 2: ADDRESS (dùng chung cho facility, resident, contact — chuẩn hóa theo Mỹ)
-- =====================================================================================

-- EN: Reusable US-format address. state is the 2-letter USPS code; zip_code supports ZIP+4.
-- VI: Bảng địa chỉ dùng chung, chuẩn hóa theo định dạng Mỹ (state 2 ký tự, zip 5 hoặc 9 số).
CREATE TABLE [addresses] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [street_line1] NVARCHAR(200) NOT NULL,
  [street_line2] NVARCHAR(200),
  [city] NVARCHAR(100) NOT NULL,
  [state] CHAR(2) NOT NULL CHECK (state = UPPER(state)),  -- vd: CA, NY, TX
  [zip_code] VARCHAR(10) NOT NULL CHECK (zip_code LIKE '[0-9][0-9][0-9][0-9][0-9]'
                                              OR zip_code LIKE '[0-9][0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]'),
  [address_type] VARCHAR(20) NOT NULL CHECK (address_type IN ('HOME','MAILING','FACILITY','BILLING')) DEFAULT 'HOME',
  [is_deleted] BIT NOT NULL DEFAULT (0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [updated_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- =====================================================================================
-- PART 3: FACILITY & PHYSICAL STRUCTURE
-- =====================================================================================

-- EN: A licensed facility/location (each facility is licensed per-state in the US).
-- VI: Cơ sở/chi nhánh viện dưỡng lão. Mỗi bang tại Mỹ có quy định cấp phép riêng.
CREATE TABLE [facilities] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [facility_code] NVARCHAR(50) UNIQUE NOT NULL,
  [name] NVARCHAR(200) NOT NULL,
  [license_number] NVARCHAR(100) NOT NULL,                -- state facility license #
  [target_state] CHAR(2) NOT NULL,
  [address_id] BIGINT,
  [phone_number] NVARCHAR(20),
  [is_deleted] BIT NOT NULL DEFAULT (0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [updated_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- EN: N:N junction — which staff user works at which facility (is_primary = home facility).
-- VI: Bảng trung gian N-N: nhân viên làm việc tại cơ sở nào (is_primary = cơ sở chính).
CREATE TABLE [user_facilities] (
  [user_id] BIGINT NOT NULL,
  [facility_id] BIGINT NOT NULL,
  [is_primary] BIT NOT NULL DEFAULT (0),
  PRIMARY KEY ([user_id], [facility_id])
)
GO

CREATE TABLE [rooms] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [room_number] NVARCHAR(20) NOT NULL,
  [room_type] NVARCHAR(50) NOT NULL,
  [facility_id] BIGINT NOT NULL,
  [is_deleted] BIT NOT NULL DEFAULT (0)
)
GO

CREATE TABLE [beds] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [bed_number] NVARCHAR(20) NOT NULL,
  [status] VARCHAR(20) NOT NULL CHECK (status IN ('AVAILABLE','OCCUPIED','MAINTENANCE')) DEFAULT 'AVAILABLE',
  [room_id] BIGINT NOT NULL
)
GO

-- EN: Minimum-staffing config per facility (federal/state ratio compliance, e.g. CMS rule).
-- VI: Cấu hình định mức nhân sự tối thiểu mỗi cơ sở, phục vụ tuân thủ quy định của CMS/bang.
CREATE TABLE [staffing_configs] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [min_hrs_per_resident_day] DECIMAL(5,2) NOT NULL,
  [warn_below_percentage] INT NOT NULL,
  [facility_id] BIGINT NOT NULL,
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- =====================================================================================
-- PART 4: CARE LEVEL (chuẩn hóa thuật ngữ "mức độ chăm sóc" theo ngành Mỹ)
-- =====================================================================================

-- EN: Level of Care catalogue used across the US industry: Independent Living, Assisted
--     Living, Memory Care, Skilled Nursing, Hospice.
-- VI: Danh mục "Mức độ chăm sóc" theo chuẩn ngành viện dưỡng lão Mỹ.
CREATE TABLE [care_levels] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [level_code] VARCHAR(30) UNIQUE NOT NULL CHECK (level_code IN
                        ('INDEPENDENT_LIVING','ASSISTED_LIVING','MEMORY_CARE','SKILLED_NURSING','HOSPICE')),
  [level_name] NVARCHAR(100) NOT NULL,
  [is_deleted] BIT NOT NULL DEFAULT (0)
)
GO

-- EN: Historical daily rate per care level (rates change over time / by facility).
-- VI: Lịch sử đơn giá theo ngày của từng mức độ chăm sóc (giá thay đổi theo thời gian/cơ sở).
CREATE TABLE [care_level_rates] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [care_level_id] BIGINT NOT NULL,
  [facility_id] BIGINT NOT NULL,
  [daily_rate] DECIMAL(18,2) NOT NULL,
  [effective_from] DATE NOT NULL,
  [effective_to] DATE
)
GO

-- =====================================================================================
-- PART 5: RESIDENTS & PII/PHI
-- =====================================================================================

-- EN: Core resident demographic record. Only "minimum necessary" identifying data lives
--     here; the highest-risk identifiers (SSN, MRN, insurance ID) are isolated in
--     resident_sensitive_info for stricter access control and encryption.
-- VI: Hồ sơ nhân khẩu học cơ bản của Cư dân (KHÔNG gọi patients/customers theo đúng chuẩn
--     ngành). Các định danh rủi ro cao (SSN, MRN, số bảo hiểm) tách riêng sang bảng
--     resident_sensitive_info để kiểm soát truy cập và mã hóa chặt hơn.
CREATE TABLE [residents] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [first_name] NVARCHAR(100) NOT NULL,
  [middle_name] NVARCHAR(100),
  [last_name] NVARCHAR(100) NOT NULL,
  [date_of_birth] DATE NOT NULL,
  [gender] VARCHAR(20) CHECK (gender IN ('MALE','FEMALE','OTHER','UNDISCLOSED')),
  [marital_status] VARCHAR(20),
  [religion_preference] NVARCHAR(100),
  [status] VARCHAR(20) NOT NULL CHECK (status IN ('PENDING','ACTIVE','DISCHARGED','DECEASED')) DEFAULT 'PENDING',
  [is_chart_locked] BIT NOT NULL DEFAULT (0),
  [address_id] BIGINT,                                    -- địa chỉ trước khi nhập viện
  [bed_id] BIGINT,
  [is_deleted] BIT NOT NULL DEFAULT (0),
  [deleted_at] DATETIMEOFFSET(0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [updated_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- EN: Highest-sensitivity identifiers, 1:1 with resident. All *_encrypted columns store
--     ciphertext (application/column-level encryption) — never plain values.
-- VI: Thông tin nhạy cảm nhất của Cư dân (SSN, MRN, số bảo hiểm...), quan hệ 1-1 với residents.
--     Toàn bộ cột *_encrypted lưu dữ liệu ĐÃ MÃ HÓA, tuyệt đối không lưu thô.
CREATE TABLE [resident_sensitive_info] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [resident_id] BIGINT UNIQUE NOT NULL,
  [ssn_encrypted] VARCHAR(512),                           -- Social Security Number
  [medical_record_number_encrypted] VARCHAR(512),         -- MRN
  [primary_insurance_id_encrypted] VARCHAR(512),
  [bank_account_encrypted] VARCHAR(512),                  -- dùng cho auto-pay, nếu có
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [updated_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

CREATE TABLE [resident_care_level_history] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [start_date] DATE NOT NULL,
  [end_date] DATE,
  [resident_id] BIGINT NOT NULL,
  [care_level_id] BIGINT NOT NULL
)
GO

-- EN: A "person" record reusable as guarantor and/or emergency contact — the same person
--     (e.g. one adult child) is often both, so a single contact entity avoids duplication.
-- VI: Thực thể "người liên hệ" dùng chung cho cả Người bảo hộ tài chính lẫn Người liên hệ
--     khẩn cấp — vì trong thực tế thường là cùng một người (con trai/con gái).
CREATE TABLE [contacts] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [first_name] NVARCHAR(100) NOT NULL,
  [middle_name] NVARCHAR(100),
  [last_name] NVARCHAR(100) NOT NULL,
  [phone_primary] NVARCHAR(20) NOT NULL,
  [phone_secondary] NVARCHAR(20),
  [email] NVARCHAR(255),
  [address_id] BIGINT,
  [is_deleted] BIT NOT NULL DEFAULT (0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [updated_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- EN: N:N junction — one resident can have many contacts (children), and one contact can
--     be guarantor/emergency contact for many residents (e.g. both parents).
-- VI: Bảng trung gian N-N: một Cư dân có nhiều người liên hệ, một người liên hệ có thể
--     gửi/bảo hộ nhiều Cư dân (ví dụ gửi cả cha lẫn mẹ vào viện).
CREATE TABLE [resident_contacts] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [resident_id] BIGINT NOT NULL,
  [contact_id] BIGINT NOT NULL,
  [relationship_type] VARCHAR(50) NOT NULL,               -- SON, DAUGHTER, SPOUSE, LEGAL_GUARDIAN, FRIEND...
  [is_guarantor] BIT NOT NULL DEFAULT (0),
  [is_emergency_contact] BIT NOT NULL DEFAULT (0),
  [is_primary] BIT NOT NULL DEFAULT (0),
  [financial_responsibility_pct] DECIMAL(5,2) CHECK (financial_responsibility_pct BETWEEN 0 AND 100),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- =====================================================================================
-- PART 6: MODULE M1 — INTAKE & EHR
-- =====================================================================================

CREATE TABLE [pre_admission_screenings] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [status] VARCHAR(20) NOT NULL CHECK (status IN ('DRAFT','COMPLETED','REJECTED')),
  [resident_id] BIGINT NOT NULL,
  [screened_by] BIGINT NOT NULL,
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

CREATE TABLE [admissions] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [admission_date] DATE NOT NULL,
  [discharge_date] DATE,
  [discharge_reason] NVARCHAR(255),
  [resident_id] BIGINT NOT NULL,
  [facility_id] BIGINT NOT NULL,
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- EN: Progress notes, diagnoses, lab results — high-value PHI, every read must be audited
--     (see phi_access_logs).
-- VI: Ghi chú lâm sàng (tiến triển, chẩn đoán, kết quả xét nghiệm) — PHI giá trị cao,
--     mọi lượt xem phải được ghi log tại bảng phi_access_logs.
CREATE TABLE [clinical_records] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [record_type] VARCHAR(50) NOT NULL CHECK (record_type IN ('PROGRESS_NOTE','DIAGNOSIS','LAB_RESULT')),
  [description] NVARCHAR(MAX) NOT NULL,
  [resident_id] BIGINT NOT NULL,
  [recorded_by] BIGINT NOT NULL,
  [is_deleted] BIT NOT NULL DEFAULT (0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [updated_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

CREATE TABLE [assessments] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [adl_total_score] INT NOT NULL,
  [is_overridden] BIT NOT NULL DEFAULT (0),
  [suggested_care_level_id] BIGINT NOT NULL,
  [confirmed_care_level_id] BIGINT NOT NULL,
  [resident_id] BIGINT NOT NULL,
  [assessed_by] BIGINT NOT NULL,
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

CREATE TABLE [assessment_metrics] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [category] VARCHAR(50) NOT NULL CHECK (category IN ('ADL','IADL','BRADEN','MORSE')),
  [metric_name] NVARCHAR(100) NOT NULL
)
GO

CREATE TABLE [assessment_details] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [score] INT NOT NULL,
  [notes] NVARCHAR(500),
  [assessment_id] BIGINT NOT NULL,
  [metric_id] BIGINT NOT NULL
)
GO

-- EN: Vital signs log (BP, HR, temperature, SpO2, pain scale). High-frequency append-only
--     table -> BIGINT identity for insert performance.
-- VI: Nhật ký chỉ số sinh tồn — bảng ghi liên tục tần suất cao, dùng BIGINT identity để tối
--     ưu tốc độ insert/index.
CREATE TABLE [vital_signs] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [resident_id] BIGINT NOT NULL,
  [recorded_by] BIGINT NOT NULL,
  [blood_pressure_systolic] SMALLINT,
  [blood_pressure_diastolic] SMALLINT,
  [heart_rate_bpm] SMALLINT,
  [respiratory_rate] SMALLINT,
  [temperature_fahrenheit] DECIMAL(4,1),
  [spo2_percentage] TINYINT CHECK (spo2_percentage BETWEEN 0 AND 100),
  [pain_scale] TINYINT CHECK (pain_scale BETWEEN 0 AND 10),
  [notes] NVARCHAR(500),
  [recorded_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- =====================================================================================
-- PART 7: MODULE M2 — CARE PLANNING
-- =====================================================================================

CREATE TABLE [care_plans] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [status] VARCHAR(20) NOT NULL CHECK (status IN ('DRAFT','ACTIVE','RESOLVED','DISCONTINUED')),
  [significant_change_flag] BIT NOT NULL DEFAULT (0),
  [resident_id] BIGINT NOT NULL,
  [is_deleted] BIT NOT NULL DEFAULT (0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [updated_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

CREATE TABLE [care_goals] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [status] VARCHAR(20) NOT NULL CHECK (status IN ('IN_PROGRESS','ACHIEVED','NOT_MET')),
  [care_plan_id] BIGINT NOT NULL
)
GO

CREATE TABLE [care_interventions] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [assigned_role] VARCHAR(50) NOT NULL,                   -- NURSE, CNA, THERAPIST...
  [care_plan_id] BIGINT NOT NULL
)
GO

CREATE TABLE [care_tasks] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [task_type] VARCHAR(50) NOT NULL,
  [status] VARCHAR(20) NOT NULL CHECK (status IN ('PENDING','COMPLETED','MISSED')) DEFAULT 'PENDING',
  [is_abnormal_flagged] BIT NOT NULL DEFAULT (0),
  [care_intervention_id] BIGINT NOT NULL,
  [assigned_cna_id] BIGINT,
  [scheduled_time] DATETIMEOFFSET(0) NOT NULL,
  [completed_at] DATETIMEOFFSET(0)
)
GO

-- =====================================================================================
-- PART 8: MODULE M3 — eMAR (Electronic Medication Administration Record)
-- =====================================================================================

CREATE TABLE [medication_orders] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [drug_name] NVARCHAR(200) NOT NULL,
  [dosage] NVARCHAR(100) NOT NULL,
  [route] VARCHAR(30) NOT NULL,                           -- ORAL, IV, TOPICAL, INJECTION...
  [frequency] NVARCHAR(100) NOT NULL,                     -- vd: "Every 8 hours"
  [is_controlled_substance] BIT NOT NULL DEFAULT (0),     -- DEA Schedule II-V, cần đối chiếu kép
  [status] VARCHAR(20) NOT NULL CHECK (status IN ('ACTIVE','DISCONTINUED','ON_HOLD')),
  [resident_id] BIGINT NOT NULL,
  [prescribed_by] BIGINT NOT NULL,
  [is_deleted] BIT NOT NULL DEFAULT (0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [updated_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- EN: Scheduled dose times generated from an order's frequency (feeds the eMAR pass list).
-- VI: Các mốc giờ phát thuốc được sinh ra từ tần suất của y lệnh (phục vụ danh sách eMAR).
CREATE TABLE [medication_schedules] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [order_id] BIGINT NOT NULL,
  [scheduled_time] TIME NOT NULL,
  [is_active] BIT NOT NULL DEFAULT (1)
)
GO

-- EN: Actual administration event (eMAR). Controlled-substance doses should require a
--     witness (witnessed_by) per most state regulations.
-- VI: Sự kiện phát thuốc thực tế (eMAR). Thuốc thuộc danh mục kiểm soát nên có người chứng
--     kiến (witnessed_by) theo quy định của bang.
CREATE TABLE [medication_logs] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [status] VARCHAR(20) NOT NULL CHECK (status IN ('ADMINISTERED','REFUSED','HELD','NOT_AVAILABLE')),
  [is_clinically_justified] BIT NOT NULL DEFAULT (0),
  [override_reason] NVARCHAR(500),
  [order_id] BIGINT NOT NULL,
  [administered_by] BIGINT NOT NULL,
  [witnessed_by] BIGINT,
  [logged_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- =====================================================================================
-- PART 9: STAFF SCHEDULING (Phân ca trực)
-- =====================================================================================

CREATE TABLE [shifts] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [facility_id] BIGINT NOT NULL,
  [shift_name] VARCHAR(20) NOT NULL CHECK (shift_name IN ('DAY','EVENING','NIGHT','WEEKEND')),
  [start_time] TIME NOT NULL,
  [end_time] TIME NOT NULL,
  [description] NVARCHAR(255)
)
GO

CREATE TABLE [shift_assignments] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [shift_id] BIGINT NOT NULL,
  [user_id] BIGINT NOT NULL,
  [work_date] DATE NOT NULL,
  [status] VARCHAR(20) NOT NULL CHECK (status IN ('SCHEDULED','CONFIRMED','CALLED_OUT','COMPLETED')) DEFAULT 'SCHEDULED',
  [clock_in_at] DATETIMEOFFSET(0),
  [clock_out_at] DATETIMEOFFSET(0)
)
GO

-- =====================================================================================
-- PART 10: BILLING (Medicare / Medicaid / Private Insurance / Co-pay)
-- =====================================================================================

CREATE TABLE [insurance_providers] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [provider_name] NVARCHAR(200) NOT NULL,
  [provider_type] VARCHAR(20) NOT NULL CHECK (provider_type IN ('MEDICARE','MEDICAID','PRIVATE','OTHER'))
)
GO

-- EN: policy_number stored encrypted — it is a PHI-adjacent financial identifier.
-- VI: policy_number lưu ở dạng mã hóa vì gắn với danh tính bảo hiểm cá nhân (PHI-adjacent).
CREATE TABLE [resident_insurance_policies] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [resident_id] BIGINT NOT NULL,
  [insurance_provider_id] BIGINT NOT NULL,
  [policy_number_encrypted] VARCHAR(512) NOT NULL,
  [group_number] NVARCHAR(100),
  [effective_from] DATE NOT NULL,
  [effective_to] DATE,
  [is_primary] BIT NOT NULL DEFAULT (0),
  [is_deleted] BIT NOT NULL DEFAULT (0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- EN: Invoice explicitly splits payer responsibility across Medicare, Medicaid, private
--     insurance, and the family's own out-of-pocket / co-pay amount.
-- VI: Hóa đơn tách rõ phần Medicare, Medicaid, bảo hiểm tư nhân chi trả, và phần gia đình tự
--     trả (co-pay/out-of-pocket).
CREATE TABLE [invoices] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [resident_id] BIGINT NOT NULL,
  [billing_period_start] DATE NOT NULL,
  [billing_period_end] DATE NOT NULL,
  [total_amount] DECIMAL(18,2) NOT NULL,
  [medicare_covered_amount] DECIMAL(18,2) NOT NULL DEFAULT (0),
  [medicaid_covered_amount] DECIMAL(18,2) NOT NULL DEFAULT (0),
  [private_insurance_covered_amount] DECIMAL(18,2) NOT NULL DEFAULT (0),
  [patient_responsibility_amount] DECIMAL(18,2) NOT NULL DEFAULT (0),   -- co-pay / out-of-pocket
  [status] VARCHAR(20) NOT NULL CHECK (status IN ('DRAFT','SENT','PARTIALLY_PAID','PAID','OVERDUE','VOID')) DEFAULT 'DRAFT',
  [due_date] DATE NOT NULL,
  [is_deleted] BIT NOT NULL DEFAULT (0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [updated_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

CREATE TABLE [invoice_line_items] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [invoice_id] BIGINT NOT NULL,
  [description] NVARCHAR(255) NOT NULL,
  [item_type] VARCHAR(30) NOT NULL CHECK (item_type IN ('ROOM_BOARD','CARE_LEVEL','MEDICATION','THERAPY','OTHER')),
  [amount] DECIMAL(18,2) NOT NULL
)
GO

-- EN: payment_token_encrypted stores a tokenized payment reference only (per PCI-DSS, raw
--     card/bank numbers must never be persisted in application databases).
-- VI: payment_token_encrypted chỉ lưu token thanh toán đã mã hóa (theo PCI-DSS, tuyệt đối
--     không lưu số thẻ/số tài khoản gốc trong database ứng dụng).
CREATE TABLE [payments] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [invoice_id] BIGINT NOT NULL,
  [payer_type] VARCHAR(20) NOT NULL CHECK (payer_type IN ('MEDICARE','MEDICAID','PRIVATE_INSURANCE','FAMILY')),
  [payment_method] VARCHAR(20) NOT NULL CHECK (payment_method IN ('CREDIT_CARD','ACH','CHECK','CASH','INSURANCE_DIRECT')),
  [amount] DECIMAL(18,2) NOT NULL,
  [payment_token_encrypted] VARCHAR(512),
  [received_by] BIGINT NOT NULL,
  [paid_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- =====================================================================================
-- PART 11: MODULE M7 — INCIDENT & RISK
-- =====================================================================================

CREATE TABLE [incident_severities] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [level_name] NVARCHAR(50) NOT NULL,
  [chart_lock_trigger] BIT NOT NULL DEFAULT (0)
)
GO

-- EN: SLA window (hours) per severity — drives the incident's sla_deadline.
-- VI: Cửa sổ SLA (giờ) theo từng mức độ nghiêm trọng — dùng để tính hạn xử lý (sla_deadline).
CREATE TABLE [sla_configs] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [sla_window_hrs] INT NOT NULL,
  [severity_id] BIGINT NOT NULL
)
GO

CREATE TABLE [incidents] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [incident_type] VARCHAR(50) NOT NULL CHECK (incident_type IN ('FALL','MEDICATION_ERROR','ALTERCATION','SKIN_TEAR')),
  [status] VARCHAR(20) NOT NULL CHECK (status IN ('OPEN','UNDER_INVESTIGATION','CLOSED')) DEFAULT 'OPEN',
  [description] NVARCHAR(MAX),
  [sla_deadline] DATETIMEOFFSET(0) NOT NULL,
  [resident_id] BIGINT NOT NULL,
  [severity_id] BIGINT NOT NULL,
  [reported_by] BIGINT NOT NULL,
  [reported_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- EN: Append-only timeline of actions taken on an incident (state changes, chart lock/unlock,
--     escalations...). Replaces the V3 chart_lock_events table with a more general audit trail.
-- VI: Nhật ký dòng thời gian (append-only) các hành động trên một sự cố (đổi trạng thái,
--     khóa/mở khóa hồ sơ, leo thang...). Thay cho bảng chart_lock_events ở V3, tổng quát hơn.
CREATE TABLE [incident_timelines] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [incident_id] BIGINT NOT NULL,
  [action] varchar(max),
  [reason] varchar(max),
  [actor] bigint,
  [createdAt] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- =====================================================================================
-- PART 12: AUDIT, PHI ACCESS LOG & NOTIFICATIONS (HIPAA compliance core)
-- =====================================================================================

-- EN: Write-audit trail (INSERT/UPDATE/DELETE) with before/after JSON snapshots.
-- VI: Nhật ký ghi vết thay đổi (thêm/sửa/xóa), lưu cả giá trị cũ và mới dạng JSON.
CREATE TABLE [audit_logs] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [table_name] NVARCHAR(100) NOT NULL,
  [record_id] NVARCHAR(100) NOT NULL,                     -- lưu dạng string để tương thích cả GUID lẫn BIGINT
  [action] VARCHAR(20) NOT NULL CHECK (action IN ('INSERT','UPDATE','DELETE')),
  [old_data] NVARCHAR(MAX),                               -- JSON snapshot trước khi thay đổi
  [new_data] NVARCHAR(MAX),                               -- JSON snapshot sau khi thay đổi
  [performed_by] BIGINT NOT NULL,
  [performed_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [ip_address] VARCHAR(45)
)
GO

-- EN: READ-audit trail — HIPAA requires logging who *viewed* PHI, not just who changed it.
-- VI: Nhật ký ghi vết TRUY CẬP/XEM PHI — HIPAA yêu cầu log cả hành vi xem, không chỉ sửa/xóa.
CREATE TABLE [phi_access_logs] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [table_name] NVARCHAR(100) NOT NULL,
  [record_id] NVARCHAR(100) NOT NULL,
  [accessed_by] BIGINT NOT NULL,
  [access_type] VARCHAR(20) NOT NULL CHECK (access_type IN ('VIEW','PRINT','EXPORT','DOWNLOAD')),
  [access_reason] NVARCHAR(255),
  [ip_address] VARCHAR(45),
  [accessed_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

CREATE TABLE [notifications] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [title] NVARCHAR(255) NOT NULL,
  [type] VARCHAR(50) NOT NULL,
  [is_read] BIT NOT NULL DEFAULT (0),
  [user_id] BIGINT NOT NULL,
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- =====================================================================================
-- PART 13: MODULE — INVENTORY & ASSETS (Kho vật tư & thiết bị y tế)
-- =====================================================================================

-- EN: Shared category catalogue for both durable equipment and consumable supplies.
-- VI: Danh mục phân loại dùng chung cho cả thiết bị bền (DME) lẫn vật tư tiêu hao.
CREATE TABLE [inventory_categories] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [category_name] NVARCHAR(100) UNIQUE NOT NULL,
  [description] NVARCHAR(500),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- EN: Durable Medical Equipment — trackable assets (wheelchairs, beds, oxygen concentrators)
--     identified by a unique asset_tag and assignable to a user or a resident.
-- VI: Thiết bị y tế bền (xe lăn, giường, máy tạo oxy...) — tài sản theo dõi được bằng
--     asset_tag duy nhất, có thể gán cho nhân viên hoặc cư dân.
CREATE TABLE [durable_medical_equipment] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [item_name] NVARCHAR(200) NOT NULL,
  [category_id] BIGINT NOT NULL,
  [asset_tag] VARCHAR(50) UNIQUE NOT NULL,
  [status] VARCHAR(30) NOT NULL CHECK ([status] IN ('AVAILABLE', 'IN_SERVICE', 'UNDER_MAINTENANCE', 'RETIRED')) DEFAULT 'AVAILABLE',
  [facility_id] BIGINT NOT NULL,
  [assigned_to_user] BIGINT,
  [assigned_to_resident] BIGINT,
  [unit_value] DECIMAL(18,2) NOT NULL DEFAULT (0),
  [is_deleted] BIT NOT NULL DEFAULT (0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [updated_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- EN: Consumable supplies with stock levels — status auto-reflects reorder_threshold logic
--     (OK / LOW_STOCK / OUT_OF_STOCK), maintained by the application layer.
-- VI: Vật tư tiêu hao có theo dõi tồn kho — status phản ánh ngưỡng đặt lại hàng
--     (OK / LOW_STOCK / OUT_OF_STOCK), do tầng ứng dụng cập nhật.
CREATE TABLE [consumable_supplies] (
  [id] BIGINT IDENTITY(1,1) PRIMARY KEY,
  [item_name] NVARCHAR(200) NOT NULL,
  [category_id] BIGINT NOT NULL,
  [facility_id] BIGINT NOT NULL,
  [stock_on_hand] INT NOT NULL CHECK ([stock_on_hand] >= 0) DEFAULT (0),
  [total] INT NOT NULL CHECK ([total] >= 0) DEFAULT (0),
  [reorder_threshold] INT NOT NULL CHECK ([reorder_threshold] >= 0) DEFAULT (0),
  [unit_cost] DECIMAL(18,2) NOT NULL DEFAULT (0),
  [private_pay_rate] DECIMAL(18,2) NOT NULL DEFAULT (0),
  [status] VARCHAR(20) NOT NULL CHECK ([status] IN ('OK', 'LOW_STOCK', 'OUT_OF_STOCK')) DEFAULT 'OK',
  [is_deleted] BIT NOT NULL DEFAULT (0),
  [created_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
  [updated_at] DATETIMEOFFSET(0) NOT NULL DEFAULT (SYSDATETIMEOFFSET())
)
GO

-- =====================================================================================
-- INDEXES
-- Gom toàn bộ index vào một khối cho dễ rà soát hiệu năng (khác V3 vốn khai báo inline).
-- =====================================================================================

CREATE INDEX [idx_users_role_id] ON [users] ([role_id])
GO

CREATE INDEX [idx_users_status] ON [users] ([status])
GO

CREATE UNIQUE INDEX [uq_room_per_facility] ON [rooms] ([facility_id], [room_number])
GO

CREATE INDEX [idx_rooms_facility_id] ON [rooms] ([facility_id])
GO

CREATE UNIQUE INDEX [uq_bed_per_room] ON [beds] ([room_id], [bed_number])
GO

CREATE INDEX [idx_beds_room_id] ON [beds] ([room_id])
GO

CREATE INDEX [idx_beds_status] ON [beds] ([status])
GO

CREATE INDEX [idx_staffing_configs_facility_id] ON [staffing_configs] ([facility_id])
GO

CREATE INDEX [idx_care_level_rates_lookup] ON [care_level_rates] ([care_level_id], [facility_id], [effective_from])
GO

CREATE INDEX [idx_residents_status] ON [residents] ([status])
GO

CREATE INDEX [idx_residents_bed_id] ON [residents] ([bed_id])
GO

CREATE INDEX [idx_residents_dob] ON [residents] ([date_of_birth])
GO

CREATE INDEX [idx_residents_name] ON [residents] ([last_name], [first_name])
GO

CREATE INDEX [idx_resident_care_level_history_resident_id] ON [resident_care_level_history] ([resident_id])
GO

CREATE UNIQUE INDEX [uq_resident_contact] ON [resident_contacts] ([resident_id], [contact_id], [relationship_type])
GO

CREATE INDEX [idx_resident_contacts_resident_id] ON [resident_contacts] ([resident_id])
GO

CREATE INDEX [idx_resident_contacts_contact_id] ON [resident_contacts] ([contact_id])
GO

CREATE INDEX [idx_pre_admission_screenings_resident_id] ON [pre_admission_screenings] ([resident_id])
GO

CREATE INDEX [idx_admissions_resident_id] ON [admissions] ([resident_id])
GO

CREATE INDEX [idx_clinical_records_resident_id] ON [clinical_records] ([resident_id])
GO

CREATE INDEX [idx_assessments_resident_id] ON [assessments] ([resident_id])
GO

CREATE INDEX [idx_assessment_details_assessment_id] ON [assessment_details] ([assessment_id])
GO

CREATE INDEX [idx_vital_signs_resident_id_recorded_at] ON [vital_signs] ([resident_id], [recorded_at])
GO

CREATE INDEX [idx_care_plans_resident_id] ON [care_plans] ([resident_id])
GO

CREATE INDEX [idx_care_goals_care_plan_id] ON [care_goals] ([care_plan_id])
GO

CREATE INDEX [idx_care_interventions_care_plan_id] ON [care_interventions] ([care_plan_id])
GO

CREATE INDEX [idx_care_tasks_intervention_id] ON [care_tasks] ([care_intervention_id])
GO

CREATE INDEX [idx_care_tasks_assigned_cna_id] ON [care_tasks] ([assigned_cna_id])
GO

CREATE INDEX [idx_care_tasks_scheduled_time] ON [care_tasks] ([scheduled_time])
GO

CREATE INDEX [idx_medication_orders_resident_id] ON [medication_orders] ([resident_id])
GO

CREATE INDEX [idx_medication_orders_status] ON [medication_orders] ([status])
GO

CREATE INDEX [idx_medication_schedules_order_id] ON [medication_schedules] ([order_id])
GO

CREATE INDEX [idx_medication_logs_order_id_logged_at] ON [medication_logs] ([order_id], [logged_at])
GO

CREATE INDEX [idx_medication_logs_administered_by] ON [medication_logs] ([administered_by])
GO

CREATE INDEX [idx_shifts_facility_id] ON [shifts] ([facility_id])
GO

CREATE UNIQUE INDEX [uq_shift_assignment] ON [shift_assignments] ([shift_id], [user_id], [work_date])
GO

CREATE INDEX [idx_shift_assignments_user_id_work_date] ON [shift_assignments] ([user_id], [work_date])
GO

CREATE INDEX [idx_resident_insurance_policies_resident_id] ON [resident_insurance_policies] ([resident_id])
GO

CREATE INDEX [idx_invoices_resident_id] ON [invoices] ([resident_id])
GO

CREATE INDEX [idx_invoices_status] ON [invoices] ([status])
GO

CREATE INDEX [idx_invoice_line_items_invoice_id] ON [invoice_line_items] ([invoice_id])
GO

CREATE INDEX [idx_payments_invoice_id] ON [payments] ([invoice_id])
GO

CREATE INDEX [idx_incidents_resident_id] ON [incidents] ([resident_id])
GO

CREATE INDEX [idx_incidents_status] ON [incidents] ([status])
GO

CREATE INDEX [idx_incident_timelines_incident_id] ON [incident_timelines] ([incident_id])
GO

CREATE INDEX [idx_audit_logs_table_record] ON [audit_logs] ([table_name], [record_id])
GO

CREATE INDEX [idx_audit_logs_performed_by] ON [audit_logs] ([performed_by])
GO

CREATE INDEX [idx_audit_logs_performed_at] ON [audit_logs] ([performed_at])
GO

CREATE INDEX [idx_phi_access_logs_table_record] ON [phi_access_logs] ([table_name], [record_id])
GO

CREATE INDEX [idx_phi_access_logs_accessed_by] ON [phi_access_logs] ([accessed_by])
GO

CREATE INDEX [idx_notifications_user_id_is_read] ON [notifications] ([user_id], [is_read])
GO

CREATE INDEX [idx_dme_facility_lookup] ON [durable_medical_equipment] ([facility_id], [is_deleted])
GO

CREATE INDEX [idx_dme_status_filter] ON [durable_medical_equipment] ([status])
GO

CREATE INDEX [idx_dme_item_name] ON [durable_medical_equipment] ([item_name])
GO

CREATE INDEX [idx_supplies_facility_lookup] ON [consumable_supplies] ([facility_id], [is_deleted])
GO

CREATE INDEX [idx_supplies_status_filter] ON [consumable_supplies] ([status])
GO

-- =====================================================================================
-- FOREIGN KEY CONSTRAINTS
-- Tách riêng ở cuối file để thứ tự CREATE TABLE không phụ thuộc lẫn nhau.
-- =====================================================================================

ALTER TABLE [durable_medical_equipment] ADD CONSTRAINT [FK_dme_category] FOREIGN KEY ([category_id]) REFERENCES [inventory_categories] ([id])
GO

ALTER TABLE [durable_medical_equipment] ADD CONSTRAINT [FK_dme_facility] FOREIGN KEY ([facility_id]) REFERENCES [facilities] ([id])
GO

ALTER TABLE [durable_medical_equipment] ADD CONSTRAINT [FK_dme_user] FOREIGN KEY ([assigned_to_user]) REFERENCES [users] ([id])
GO

ALTER TABLE [durable_medical_equipment] ADD CONSTRAINT [FK_dme_resident] FOREIGN KEY ([assigned_to_resident]) REFERENCES [residents] ([id])
GO

ALTER TABLE [consumable_supplies] ADD CONSTRAINT [FK_supplies_category] FOREIGN KEY ([category_id]) REFERENCES [inventory_categories] ([id])
GO

ALTER TABLE [consumable_supplies] ADD CONSTRAINT [FK_supplies_facility] FOREIGN KEY ([facility_id]) REFERENCES [facilities] ([id])
GO

ALTER TABLE [role_permissions] ADD FOREIGN KEY ([role_id]) REFERENCES [roles] ([id])
GO

ALTER TABLE [role_permissions] ADD FOREIGN KEY ([permission_id]) REFERENCES [permissions] ([id])
GO

ALTER TABLE [users] ADD FOREIGN KEY ([role_id]) REFERENCES [roles] ([id])
GO

ALTER TABLE [facilities] ADD FOREIGN KEY ([address_id]) REFERENCES [addresses] ([id])
GO

ALTER TABLE [user_facilities] ADD FOREIGN KEY ([user_id]) REFERENCES [users] ([id])
GO

ALTER TABLE [user_facilities] ADD FOREIGN KEY ([facility_id]) REFERENCES [facilities] ([id])
GO

ALTER TABLE [rooms] ADD FOREIGN KEY ([facility_id]) REFERENCES [facilities] ([id])
GO

ALTER TABLE [beds] ADD FOREIGN KEY ([room_id]) REFERENCES [rooms] ([id])
GO

ALTER TABLE [staffing_configs] ADD FOREIGN KEY ([facility_id]) REFERENCES [facilities] ([id])
GO

ALTER TABLE [care_level_rates] ADD FOREIGN KEY ([care_level_id]) REFERENCES [care_levels] ([id])
GO

ALTER TABLE [care_level_rates] ADD FOREIGN KEY ([facility_id]) REFERENCES [facilities] ([id])
GO

ALTER TABLE [residents] ADD FOREIGN KEY ([address_id]) REFERENCES [addresses] ([id])
GO

ALTER TABLE [residents] ADD FOREIGN KEY ([bed_id]) REFERENCES [beds] ([id])
GO

ALTER TABLE [resident_sensitive_info] ADD FOREIGN KEY ([resident_id]) REFERENCES [residents] ([id])
GO

ALTER TABLE [resident_care_level_history] ADD FOREIGN KEY ([resident_id]) REFERENCES [residents] ([id])
GO

ALTER TABLE [resident_care_level_history] ADD FOREIGN KEY ([care_level_id]) REFERENCES [care_levels] ([id])
GO

ALTER TABLE [contacts] ADD FOREIGN KEY ([address_id]) REFERENCES [addresses] ([id])
GO

ALTER TABLE [resident_contacts] ADD FOREIGN KEY ([resident_id]) REFERENCES [residents] ([id])
GO

ALTER TABLE [resident_contacts] ADD FOREIGN KEY ([contact_id]) REFERENCES [contacts] ([id])
GO

ALTER TABLE [pre_admission_screenings] ADD FOREIGN KEY ([resident_id]) REFERENCES [residents] ([id])
GO

ALTER TABLE [pre_admission_screenings] ADD FOREIGN KEY ([screened_by]) REFERENCES [users] ([id])
GO

ALTER TABLE [admissions] ADD FOREIGN KEY ([resident_id]) REFERENCES [residents] ([id])
GO

ALTER TABLE [admissions] ADD FOREIGN KEY ([facility_id]) REFERENCES [facilities] ([id])
GO

ALTER TABLE [clinical_records] ADD FOREIGN KEY ([resident_id]) REFERENCES [residents] ([id])
GO

ALTER TABLE [clinical_records] ADD FOREIGN KEY ([recorded_by]) REFERENCES [users] ([id])
GO

ALTER TABLE [assessments] ADD FOREIGN KEY ([suggested_care_level_id]) REFERENCES [care_levels] ([id])
GO

ALTER TABLE [assessments] ADD FOREIGN KEY ([confirmed_care_level_id]) REFERENCES [care_levels] ([id])
GO

ALTER TABLE [assessments] ADD FOREIGN KEY ([resident_id]) REFERENCES [residents] ([id])
GO

ALTER TABLE [assessments] ADD FOREIGN KEY ([assessed_by]) REFERENCES [users] ([id])
GO

ALTER TABLE [assessment_details] ADD FOREIGN KEY ([assessment_id]) REFERENCES [assessments] ([id])
GO

ALTER TABLE [assessment_details] ADD FOREIGN KEY ([metric_id]) REFERENCES [assessment_metrics] ([id])
GO

ALTER TABLE [vital_signs] ADD FOREIGN KEY ([resident_id]) REFERENCES [residents] ([id])
GO

ALTER TABLE [vital_signs] ADD FOREIGN KEY ([recorded_by]) REFERENCES [users] ([id])
GO

ALTER TABLE [care_plans] ADD FOREIGN KEY ([resident_id]) REFERENCES [residents] ([id])
GO

ALTER TABLE [care_goals] ADD FOREIGN KEY ([care_plan_id]) REFERENCES [care_plans] ([id])
GO

ALTER TABLE [care_interventions] ADD FOREIGN KEY ([care_plan_id]) REFERENCES [care_plans] ([id])
GO

ALTER TABLE [care_tasks] ADD FOREIGN KEY ([care_intervention_id]) REFERENCES [care_interventions] ([id])
GO

ALTER TABLE [care_tasks] ADD FOREIGN KEY ([assigned_cna_id]) REFERENCES [users] ([id])
GO

ALTER TABLE [medication_orders] ADD FOREIGN KEY ([resident_id]) REFERENCES [residents] ([id])
GO

ALTER TABLE [medication_orders] ADD FOREIGN KEY ([prescribed_by]) REFERENCES [users] ([id])
GO

ALTER TABLE [medication_schedules] ADD FOREIGN KEY ([order_id]) REFERENCES [medication_orders] ([id])
GO

ALTER TABLE [medication_logs] ADD FOREIGN KEY ([order_id]) REFERENCES [medication_orders] ([id])
GO

ALTER TABLE [medication_logs] ADD FOREIGN KEY ([administered_by]) REFERENCES [users] ([id])
GO

ALTER TABLE [medication_logs] ADD FOREIGN KEY ([witnessed_by]) REFERENCES [users] ([id])
GO

ALTER TABLE [shifts] ADD FOREIGN KEY ([facility_id]) REFERENCES [facilities] ([id])
GO

ALTER TABLE [shift_assignments] ADD FOREIGN KEY ([shift_id]) REFERENCES [shifts] ([id])
GO

ALTER TABLE [shift_assignments] ADD FOREIGN KEY ([user_id]) REFERENCES [users] ([id])
GO

ALTER TABLE [resident_insurance_policies] ADD FOREIGN KEY ([resident_id]) REFERENCES [residents] ([id])
GO

ALTER TABLE [resident_insurance_policies] ADD FOREIGN KEY ([insurance_provider_id]) REFERENCES [insurance_providers] ([id])
GO

ALTER TABLE [invoices] ADD FOREIGN KEY ([resident_id]) REFERENCES [residents] ([id])
GO

ALTER TABLE [invoice_line_items] ADD FOREIGN KEY ([invoice_id]) REFERENCES [invoices] ([id])
GO

ALTER TABLE [payments] ADD FOREIGN KEY ([invoice_id]) REFERENCES [invoices] ([id])
GO

ALTER TABLE [payments] ADD FOREIGN KEY ([received_by]) REFERENCES [users] ([id])
GO

ALTER TABLE [sla_configs] ADD FOREIGN KEY ([severity_id]) REFERENCES [incident_severities] ([id])
GO

ALTER TABLE [incidents] ADD FOREIGN KEY ([resident_id]) REFERENCES [residents] ([id])
GO

ALTER TABLE [incidents] ADD FOREIGN KEY ([severity_id]) REFERENCES [incident_severities] ([id])
GO

ALTER TABLE [incidents] ADD FOREIGN KEY ([reported_by]) REFERENCES [users] ([id])
GO

ALTER TABLE [audit_logs] ADD FOREIGN KEY ([performed_by]) REFERENCES [users] ([id])
GO

ALTER TABLE [phi_access_logs] ADD FOREIGN KEY ([accessed_by]) REFERENCES [users] ([id])
GO

ALTER TABLE [notifications] ADD FOREIGN KEY ([user_id]) REFERENCES [users] ([id])
GO

ALTER TABLE [incident_timelines] ADD FOREIGN KEY ([incident_id]) REFERENCES [incidents] ([id])
GO

ALTER TABLE [incident_timelines] ADD FOREIGN KEY ([actor]) REFERENCES [users] ([id])
GO
