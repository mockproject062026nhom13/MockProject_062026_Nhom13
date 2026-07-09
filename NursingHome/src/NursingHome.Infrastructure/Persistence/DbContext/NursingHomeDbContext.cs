using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Persistence.DbContexts;

public partial class NursingHomeDbContext
    : Microsoft.EntityFrameworkCore.DbContext
{
    public NursingHomeDbContext(DbContextOptions<NursingHomeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; private set; }

    public virtual DbSet<Admission> Admissions { get; private set; }

    public virtual DbSet<Assessment> Assessments { get; private set; }

    public virtual DbSet<AssessmentDetail> AssessmentDetails { get; private set; }

    public virtual DbSet<AssessmentMetric> AssessmentMetrics { get; private set; }

    public virtual DbSet<AuditLog> AuditLogs { get; private set; }

    public virtual DbSet<Bed> Beds { get; private set; }

    public virtual DbSet<CareGoal> CareGoals { get; private set; }

    public virtual DbSet<CareIntervention> CareInterventions { get; private set; }

    public virtual DbSet<CareLevel> CareLevels { get; private set; }

    public virtual DbSet<CareLevelRate> CareLevelRates { get; private set; }

    public virtual DbSet<CarePlan> CarePlans { get; private set; }

    public virtual DbSet<CareTask> CareTasks { get; private set; }

    public virtual DbSet<ClinicalRecord> ClinicalRecords { get; private set; }

    public virtual DbSet<ConsumableSupply> ConsumableSupplies { get; private set; }

    public virtual DbSet<Contact> Contacts { get; private set; }

    public virtual DbSet<DurableMedicalEquipment> DurableMedicalEquipments { get; private set; }

    public virtual DbSet<Facility> Facilities { get; private set; }

    public virtual DbSet<Incident> Incidents { get; private set; }

    public virtual DbSet<IncidentSeverity> IncidentSeverities { get; private set; }

    public virtual DbSet<IncidentTimeline> IncidentTimelines { get; private set; }

    public virtual DbSet<InsuranceProvider> InsuranceProviders { get; private set; }

    public virtual DbSet<InventoryCategory> InventoryCategories { get; private set; }

    public virtual DbSet<Invoice> Invoices { get; private set; }

    public virtual DbSet<InvoiceLineItem> InvoiceLineItems { get; private set; }

    public virtual DbSet<MedicationLog> MedicationLogs { get; private set; }

    public virtual DbSet<MedicationOrder> MedicationOrders { get; private set; }

    public virtual DbSet<MedicationSchedule> MedicationSchedules { get; private set; }

    public virtual DbSet<Notification> Notifications { get; private set; }

    public virtual DbSet<Payment> Payments { get; private set; }

    public virtual DbSet<Permission> Permissions { get; private set; }

    public virtual DbSet<PhiAccessLog> PhiAccessLogs { get; private set; }

    public virtual DbSet<PreAdmissionScreening> PreAdmissionScreenings { get; private set; }

    public virtual DbSet<Resident> Residents { get; private set; }

    public virtual DbSet<ResidentCareLevelHistory> ResidentCareLevelHistories { get; private set; }

    public virtual DbSet<ResidentContact> ResidentContacts { get; private set; }

    public virtual DbSet<ResidentInsurancePolicy> ResidentInsurancePolicies { get; private set; }

    public virtual DbSet<ResidentSensitiveInfo> ResidentSensitiveInfos { get; private set; }

    public virtual DbSet<Role> Roles { get; private set; }

    public virtual DbSet<Room> Rooms { get; private set; }

    public virtual DbSet<Shift> Shifts { get; private set; }

    public virtual DbSet<ShiftAssignment> ShiftAssignments { get; private set; }

    public virtual DbSet<SlaConfig> SlaConfigs { get; private set; }

    public virtual DbSet<StaffingConfig> StaffingConfigs { get; private set; }

    public virtual DbSet<User> Users { get; private set; }

    public virtual DbSet<UserFacility> UserFacilities { get; private set; }

    public virtual DbSet<VitalSign> VitalSigns { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__addresse__3213E83F30DDA70A");

            entity.Property(e => e.AddressType).HasDefaultValue("HOME");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.State).IsFixedLength();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
        });

        modelBuilder.Entity<Admission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__admissio__3213E83F34FCEE09");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Facility).WithMany(p => p.Admissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__admission__facil__40C49C62");

            entity.HasOne(d => d.Resident).WithMany(p => p.Admissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__admission__resid__3FD07829");
        });

        modelBuilder.Entity<Assessment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__assessme__3213E83F35D50A27");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.AssessedByNavigation).WithMany(p => p.Assessments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__assessmen__asses__467D75B8");

            entity.HasOne(d => d.ConfirmedCareLevel).WithMany(p => p.AssessmentConfirmedCareLevels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__assessmen__confi__44952D46");

            entity.HasOne(d => d.Resident).WithMany(p => p.Assessments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__assessmen__resid__4589517F");

            entity.HasOne(d => d.SuggestedCareLevel).WithMany(p => p.AssessmentSuggestedCareLevels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__assessmen__sugge__43A1090D");
        });

        modelBuilder.Entity<AssessmentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__assessme__3213E83FFE3B347A");

            entity.HasOne(d => d.Assessment).WithMany(p => p.AssessmentDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__assessmen__asses__477199F1");

            entity.HasOne(d => d.Metric).WithMany(p => p.AssessmentDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__assessmen__metri__4865BE2A");
        });

        modelBuilder.Entity<AssessmentMetric>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__assessme__3213E83FDEA4E7C6");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__audit_lo__3213E83F4A19EE94");

            entity.Property(e => e.PerformedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.PerformedByNavigation).WithMany(p => p.AuditLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__audit_log__perfo__6225902D");
        });

        modelBuilder.Entity<Bed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__beds__3213E83F80233057");

            entity.Property(e => e.Status).HasDefaultValue("AVAILABLE");

            entity.HasOne(d => d.Room).WithMany(p => p.Beds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__beds__room_id__32767D0B");
        });

        modelBuilder.Entity<CareGoal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__care_goa__3213E83F3806C759");

            entity.HasOne(d => d.CarePlan).WithMany(p => p.CareGoals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__care_goal__care___4C364F0E");
        });

        modelBuilder.Entity<CareIntervention>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__care_int__3213E83FCB13A9CF");

            entity.HasOne(d => d.CarePlan).WithMany(p => p.CareInterventions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__care_inte__care___4D2A7347");
        });

        modelBuilder.Entity<CareLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__care_lev__3213E83F42A8B634");
        });

        modelBuilder.Entity<CareLevelRate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__care_lev__3213E83F3FB8F6E4");

            entity.HasOne(d => d.CareLevel).WithMany(p => p.CareLevelRates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__care_leve__care___345EC57D");

            entity.HasOne(d => d.Facility).WithMany(p => p.CareLevelRates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__care_leve__facil__3552E9B6");
        });

        modelBuilder.Entity<CarePlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__care_pla__3213E83F90EB131D");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Resident).WithMany(p => p.CarePlans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__care_plan__resid__4B422AD5");
        });

        modelBuilder.Entity<CareTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__care_tas__3213E83F83005667");

            entity.Property(e => e.Status).HasDefaultValue("PENDING");

            entity.HasOne(d => d.AssignedCna).WithMany(p => p.CareTasks).HasConstraintName("FK__care_task__assig__4F12BBB9");

            entity.HasOne(d => d.CareIntervention).WithMany(p => p.CareTasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__care_task__care___4E1E9780");
        });

        modelBuilder.Entity<ClinicalRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__clinical__3213E83F019C1F63");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.RecordedByNavigation).WithMany(p => p.ClinicalRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__clinical___recor__42ACE4D4");

            entity.HasOne(d => d.Resident).WithMany(p => p.ClinicalRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__clinical___resid__41B8C09B");
        });

        modelBuilder.Entity<ConsumableSupply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__consumab__3213E83FCED600E5");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Status).HasDefaultValue("OK");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Category).WithMany(p => p.ConsumableSupplies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_supplies_category");

            entity.HasOne(d => d.Facility).WithMany(p => p.ConsumableSupplies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_supplies_facility");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__contacts__3213E83F5F5F9D99");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Address).WithMany(p => p.Contacts).HasConstraintName("FK__contacts__addres__3B0BC30C");
        });

        modelBuilder.Entity<DurableMedicalEquipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__durable___3213E83FAFF24DC3");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Status).HasDefaultValue("AVAILABLE");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.AssignedToResidentNavigation).WithMany(p => p.DurableMedicalEquipments).HasConstraintName("FK_dme_resident");

            entity.HasOne(d => d.AssignedToUserNavigation).WithMany(p => p.DurableMedicalEquipments).HasConstraintName("FK_dme_user");

            entity.HasOne(d => d.Category).WithMany(p => p.DurableMedicalEquipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dme_category");

            entity.HasOne(d => d.Facility).WithMany(p => p.DurableMedicalEquipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dme_facility");
        });

        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__faciliti__3213E83FA738D2D7");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.TargetState).IsFixedLength();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Address).WithMany(p => p.Facilities).HasConstraintName("FK__facilitie__addre__2EA5EC27");
        });

        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__incident__3213E83F901EAF32");

            entity.Property(e => e.ReportedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Status).HasDefaultValue("OPEN");

            entity.HasOne(d => d.ReportedByNavigation).WithMany(p => p.Incidents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__incidents__repor__61316BF4");

            entity.HasOne(d => d.Resident).WithMany(p => p.Incidents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__incidents__resid__5F492382");

            entity.HasOne(d => d.Severity).WithMany(p => p.Incidents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__incidents__sever__603D47BB");
        });

        modelBuilder.Entity<IncidentSeverity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__incident__3213E83F8486301F");
        });

        modelBuilder.Entity<IncidentTimeline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__incident__3213E83F1F7315E7");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.ActorNavigation).WithMany(p => p.IncidentTimelines).HasConstraintName("FK__incident___actor__65F62111");

            entity.HasOne(d => d.Incident).WithMany(p => p.IncidentTimelines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__incident___incid__6501FCD8");
        });

        modelBuilder.Entity<InsuranceProvider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__insuranc__3213E83F3EB3F7A5");
        });

        modelBuilder.Entity<InventoryCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__inventor__3213E83FD17499F3");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__invoices__3213E83F3B84AB8E");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Status).HasDefaultValue("DRAFT");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Resident).WithMany(p => p.Invoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__invoices__reside__5A846E65");
        });

        modelBuilder.Entity<InvoiceLineItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__invoice___3213E83FC0C0E3EE");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceLineItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__invoice_l__invoi__5B78929E");
        });

        modelBuilder.Entity<MedicationLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__medicati__3213E83F4E841EF3");

            entity.Property(e => e.LoggedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.AdministeredByNavigation).WithMany(p => p.MedicationLogAdministeredByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__medicatio__admin__53D770D6");

            entity.HasOne(d => d.Order).WithMany(p => p.MedicationLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__medicatio__order__52E34C9D");

            entity.HasOne(d => d.WitnessedByNavigation).WithMany(p => p.MedicationLogWitnessedByNavigations).HasConstraintName("FK__medicatio__witne__54CB950F");
        });

        modelBuilder.Entity<MedicationOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__medicati__3213E83F7CC3C528");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.PrescribedByNavigation).WithMany(p => p.MedicationOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__medicatio__presc__50FB042B");

            entity.HasOne(d => d.Resident).WithMany(p => p.MedicationOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__medicatio__resid__5006DFF2");
        });

        modelBuilder.Entity<MedicationSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__medicati__3213E83FF13A2E0B");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Order).WithMany(p => p.MedicationSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__medicatio__order__51EF2864");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__notifica__3213E83FA7C7ED9A");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__notificat__user___640DD89F");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__payments__3213E83F85B8D789");

            entity.Property(e => e.PaidAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__payments__invoic__5C6CB6D7");

            entity.HasOne(d => d.ReceivedByNavigation).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__payments__receiv__5D60DB10");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__permissi__3213E83FEA452E24");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
        });

        modelBuilder.Entity<PhiAccessLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__phi_acce__3213E83FE2E8477F");

            entity.Property(e => e.AccessedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.AccessedByNavigation).WithMany(p => p.PhiAccessLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__phi_acces__acces__6319B466");
        });

        modelBuilder.Entity<PreAdmissionScreening>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pre_admi__3213E83F53874E27");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Resident).WithMany(p => p.PreAdmissionScreenings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pre_admis__resid__3DE82FB7");

            entity.HasOne(d => d.ScreenedByNavigation).WithMany(p => p.PreAdmissionScreenings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pre_admis__scree__3EDC53F0");
        });

        modelBuilder.Entity<Resident>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__resident__3213E83F4337B1E4");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Status).HasDefaultValue("PENDING");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Address).WithMany(p => p.Residents).HasConstraintName("FK__residents__addre__36470DEF");

            entity.HasOne(d => d.Bed).WithMany(p => p.Residents).HasConstraintName("FK__residents__bed_i__373B3228");
        });

        modelBuilder.Entity<ResidentCareLevelHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__resident__3213E83FB7F49FBF");

            entity.HasOne(d => d.CareLevel).WithMany(p => p.ResidentCareLevelHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___care___3A179ED3");

            entity.HasOne(d => d.Resident).WithMany(p => p.ResidentCareLevelHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___resid__39237A9A");
        });

        modelBuilder.Entity<ResidentContact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__resident__3213E83F94B8EBDF");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Contact).WithMany(p => p.ResidentContacts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___conta__3CF40B7E");

            entity.HasOne(d => d.Resident).WithMany(p => p.ResidentContacts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___resid__3BFFE745");
        });

        modelBuilder.Entity<ResidentInsurancePolicy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__resident__3213E83F79F0F44B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.InsuranceProvider).WithMany(p => p.ResidentInsurancePolicies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___insur__59904A2C");

            entity.HasOne(d => d.Resident).WithMany(p => p.ResidentInsurancePolicies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___resid__589C25F3");
        });

        modelBuilder.Entity<ResidentSensitiveInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__resident__3213E83F7C755BE3");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Resident).WithOne(p => p.ResidentSensitiveInfo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___resid__382F5661");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83FBB0F59C3");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__role_perm__permi__2CBDA3B5"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__role_perm__role___2BC97F7C"),
                    j =>
                    {
                        j.HasKey("RoleId", "PermissionId").HasName("PK__role_per__C85A54638AB1C73B");
                        j.ToTable("role_permissions");
                        j.IndexerProperty<long>("RoleId").HasColumnName("role_id");
                        j.IndexerProperty<long>("PermissionId").HasColumnName("permission_id");
                    });
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rooms__3213E83F83575558");

            entity.HasOne(d => d.Facility).WithMany(p => p.Rooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__rooms__facility___318258D2");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__shifts__3213E83FCB7DE149");

            entity.HasOne(d => d.Facility).WithMany(p => p.Shifts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__shifts__facility__55BFB948");
        });

        modelBuilder.Entity<ShiftAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__shift_as__3213E83F7E83D4A9");

            entity.Property(e => e.Status).HasDefaultValue("SCHEDULED");

            entity.HasOne(d => d.Shift).WithMany(p => p.ShiftAssignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__shift_ass__shift__56B3DD81");

            entity.HasOne(d => d.User).WithMany(p => p.ShiftAssignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__shift_ass__user___57A801BA");
        });

        modelBuilder.Entity<SlaConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sla_conf__3213E83FB5B33DBC");

            entity.HasOne(d => d.Severity).WithMany(p => p.SlaConfigs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__sla_confi__sever__5E54FF49");
        });

        modelBuilder.Entity<StaffingConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__staffing__3213E83FDF7A8181");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Facility).WithMany(p => p.StaffingConfigs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__staffing___facil__336AA144");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83FC317222B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Status).HasDefaultValue("ACTIVE");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__role_id__2DB1C7EE");
        });

        modelBuilder.Entity<UserFacility>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.FacilityId }).HasName("PK__user_fac__4290B9A5EDA96261");

            entity.HasOne(d => d.Facility).WithMany(p => p.UserFacilities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_faci__facil__308E3499");

            entity.HasOne(d => d.User).WithMany(p => p.UserFacilities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_faci__user___2F9A1060");
        });

        modelBuilder.Entity<VitalSign>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vital_si__3213E83F44CF4A66");

            entity.Property(e => e.RecordedAt).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.RecordedByNavigation).WithMany(p => p.VitalSigns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__vital_sig__recor__4A4E069C");

            entity.HasOne(d => d.Resident).WithMany(p => p.VitalSigns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__vital_sig__resid__4959E263");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
