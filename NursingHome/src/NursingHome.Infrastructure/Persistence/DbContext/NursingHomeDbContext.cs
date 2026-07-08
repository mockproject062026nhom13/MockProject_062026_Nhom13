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

    public virtual DbSet<address> addresses { get; private set; }

    public virtual DbSet<admission> admissions { get; private set; }

    public virtual DbSet<assessment> assessments { get; private set; }

    public virtual DbSet<assessment_detail> assessment_details { get; private set; }

    public virtual DbSet<assessment_metric> assessment_metrics { get; private set; }

    public virtual DbSet<audit_log> audit_logs { get; private set; }

    public virtual DbSet<bed> beds { get; private set; }

    public virtual DbSet<care_goal> care_goals { get; private set; }

    public virtual DbSet<care_intervention> care_interventions { get; private set; }

    public virtual DbSet<care_level> care_levels { get; private set; }

    public virtual DbSet<care_level_rate> care_level_rates { get; private set; }

    public virtual DbSet<care_plan> care_plans { get; private set; }

    public virtual DbSet<care_task> care_tasks { get; private set; }

    public virtual DbSet<chart_lock_event> chart_lock_events { get; private set; }

    public virtual DbSet<clinical_record> clinical_records { get; private set; }

    public virtual DbSet<contact> contacts { get; private set; }

    public virtual DbSet<facility> facilities { get; private set; }

    public virtual DbSet<incident> incidents { get; private set; }

    public virtual DbSet<incident_severity> incident_severities { get; private set; }

    public virtual DbSet<insurance_provider> insurance_providers { get; private set; }

    public virtual DbSet<invoice> invoices { get; private set; }

    public virtual DbSet<invoice_line_item> invoice_line_items { get; private set; }

    public virtual DbSet<medication_log> medication_logs { get; private set; }

    public virtual DbSet<medication_order> medication_orders { get; private set; }

    public virtual DbSet<medication_schedule> medication_schedules { get; private set; }

    public virtual DbSet<notification> notifications { get; private set; }

    public virtual DbSet<payment> payments { get; private set; }

    public virtual DbSet<permission> permissions { get; private set; }

    public virtual DbSet<phi_access_log> phi_access_logs { get; private set; }

    public virtual DbSet<pre_admission_screening> pre_admission_screenings { get; private set; }

    public virtual DbSet<resident> residents { get; private set; }

    public virtual DbSet<resident_care_level_history> resident_care_level_histories { get; private set; }

    public virtual DbSet<resident_contact> resident_contacts { get; private set; }

    public virtual DbSet<resident_insurance_policy> resident_insurance_policies { get; private set; }

    public virtual DbSet<resident_sensitive_info> resident_sensitive_infos { get; private set; }

    public virtual DbSet<role> roles { get; private set; }

    public virtual DbSet<room> rooms { get; private set; }

    public virtual DbSet<shift> shifts { get; private set; }

    public virtual DbSet<shift_assignment> shift_assignments { get; private set; }

    public virtual DbSet<sla_config> sla_configs { get; private set; }

    public virtual DbSet<staffing_config> staffing_configs { get; private set; }

    public virtual DbSet<user> users { get; private set; }

    public virtual DbSet<user_facility> user_facilities { get; private set; }

    public virtual DbSet<vital_sign> vital_signs { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<address>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__addresse__3213E83F857F95A6");

            entity.Property(e => e.address_type).HasDefaultValue("HOME");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.state).IsFixedLength();
            entity.Property(e => e.updated_at).HasDefaultValueSql("(sysdatetimeoffset())");
        });

        modelBuilder.Entity<admission>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__admissio__3213E83F5D437BDC");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.facility).WithMany(p => p.admissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__admission__facil__40058253");

            entity.HasOne(d => d.resident).WithMany(p => p.admissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__admission__resid__3F115E1A");
        });

        modelBuilder.Entity<assessment>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__assessme__3213E83F3C334068");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.assessed_byNavigation).WithMany(p => p.assessments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__assessmen__asses__51300E55");

            entity.HasOne(d => d.confirmed_care_level).WithMany(p => p.assessmentconfirmed_care_levels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__assessmen__confi__4F47C5E3");

            entity.HasOne(d => d.resident).WithMany(p => p.assessments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__assessmen__resid__503BEA1C");

            entity.HasOne(d => d.suggested_care_level).WithMany(p => p.assessmentsuggested_care_levels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__assessmen__sugge__4E53A1AA");
        });

        modelBuilder.Entity<assessment_detail>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__assessme__3213E83FDEBDC2C4");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.assessment).WithMany(p => p.assessment_details)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__assessmen__asses__58D1301D");

            entity.HasOne(d => d.metric).WithMany(p => p.assessment_details)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__assessmen__metri__59C55456");
        });

        modelBuilder.Entity<assessment_metric>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__assessme__3213E83FBE9C6892");
        });

        modelBuilder.Entity<audit_log>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__audit_lo__3213E83FBF78ED72");

            entity.Property(e => e.performed_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.performed_byNavigation).WithMany(p => p.audit_logs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__audit_log__perfo__54CB950F");
        });

        modelBuilder.Entity<bed>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__beds__3213E83F8AEB77CD");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.status).HasDefaultValue("AVAILABLE");

            entity.HasOne(d => d.room).WithMany(p => p.beds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__beds__room_id__02084FDA");
        });

        modelBuilder.Entity<care_goal>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__care_goa__3213E83FEF9618EF");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.care_plan).WithMany(p => p.care_goals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__care_goal__care___6DCC4D03");
        });

        modelBuilder.Entity<care_intervention>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__care_int__3213E83F20C6AA07");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.care_plan).WithMany(p => p.care_interventions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__care_inte__care___719CDDE7");
        });

        modelBuilder.Entity<care_level>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__care_lev__3213E83FE273D340");
        });

        modelBuilder.Entity<care_level_rate>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__care_lev__3213E83FFB5DC63E");

            entity.HasOne(d => d.care_level).WithMany(p => p.care_level_rates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__care_leve__care___0D7A0286");

            entity.HasOne(d => d.facility).WithMany(p => p.care_level_rates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__care_leve__facil__0E6E26BF");
        });

        modelBuilder.Entity<care_plan>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__care_pla__3213E83F2F900A02");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.updated_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.resident).WithMany(p => p.care_plans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__care_plan__resid__662B2B3B");
        });

        modelBuilder.Entity<care_task>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__care_tas__3213E83FAAFDF467");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.status).HasDefaultValue("PENDING");

            entity.HasOne(d => d.assigned_cna).WithMany(p => p.care_tasks).HasConstraintName("FK__care_task__assig__793DFFAF");

            entity.HasOne(d => d.care_intervention).WithMany(p => p.care_tasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__care_task__care___7849DB76");
        });

        modelBuilder.Entity<chart_lock_event>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__chart_lo__3213E83FAC8A5DFD");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.event_time).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.locked_by_system).HasDefaultValue(true);

            entity.HasOne(d => d.incident).WithMany(p => p.chart_lock_events)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__chart_loc__incid__4F12BBB9");

            entity.HasOne(d => d.unlocked_byNavigation).WithMany(p => p.chart_lock_events).HasConstraintName("FK__chart_loc__unloc__5006DFF2");
        });

        modelBuilder.Entity<clinical_record>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__clinical__3213E83F4CDB6C4D");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.updated_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.recorded_byNavigation).WithMany(p => p.clinical_records)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__clinical___recor__46B27FE2");

            entity.HasOne(d => d.resident).WithMany(p => p.clinical_records)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__clinical___resid__45BE5BA9");
        });

        modelBuilder.Entity<contact>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__contacts__3213E83F3726E320");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.updated_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.address).WithMany(p => p.contacts).HasConstraintName("FK__contacts__addres__282DF8C2");
        });

        modelBuilder.Entity<facility>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__faciliti__3213E83FD210712F");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.target_state).IsFixedLength();
            entity.Property(e => e.updated_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.address).WithMany(p => p.facilities).HasConstraintName("FK__facilitie__addre__6E01572D");
        });

        modelBuilder.Entity<incident>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__incident__3213E83FCAAEEAB3");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.reported_at).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.status).HasDefaultValue("OPEN");

            entity.HasOne(d => d.reported_byNavigation).WithMany(p => p.incidents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__incidents__repor__4959E263");

            entity.HasOne(d => d.resident).WithMany(p => p.incidents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__incidents__resid__477199F1");

            entity.HasOne(d => d.severity).WithMany(p => p.incidents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__incidents__sever__4865BE2A");
        });

        modelBuilder.Entity<incident_severity>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__incident__3213E83FA44FEA01");
        });

        modelBuilder.Entity<insurance_provider>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__insuranc__3213E83F0863B6AA");
        });

        modelBuilder.Entity<invoice>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__invoices__3213E83FB4F35BD7");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.status).HasDefaultValue("DRAFT");
            entity.Property(e => e.updated_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.resident).WithMany(p => p.invoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__invoices__reside__2704CA5F");
        });

        modelBuilder.Entity<invoice_line_item>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__invoice___3213E83F10C334BD");

            entity.HasOne(d => d.invoice).WithMany(p => p.invoice_line_items)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__invoice_l__invoi__32767D0B");
        });

        modelBuilder.Entity<medication_log>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__medicati__3213E83FE682555A");

            entity.Property(e => e.logged_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.administered_byNavigation).WithMany(p => p.medication_logadministered_byNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__medicatio__admin__0C50D423");

            entity.HasOne(d => d.order).WithMany(p => p.medication_logs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__medicatio__order__0B5CAFEA");

            entity.HasOne(d => d.witnessed_byNavigation).WithMany(p => p.medication_logwitnessed_byNavigations).HasConstraintName("FK__medicatio__witne__0D44F85C");
        });

        modelBuilder.Entity<medication_order>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__medicati__3213E83F8A2307F7");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.updated_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.prescribed_byNavigation).WithMany(p => p.medication_orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__medicatio__presc__7FEAFD3E");

            entity.HasOne(d => d.resident).WithMany(p => p.medication_orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__medicatio__resid__7EF6D905");
        });

        modelBuilder.Entity<medication_schedule>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__medicati__3213E83F004CD2E7");

            entity.Property(e => e.is_active).HasDefaultValue(true);

            entity.HasOne(d => d.order).WithMany(p => p.medication_schedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__medicatio__order__05A3D694");
        });

        modelBuilder.Entity<notification>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__notifica__3213E83F68EAB22F");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.user).WithMany(p => p.notifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__notificat__user___5F492382");
        });

        modelBuilder.Entity<payment>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__payments__3213E83FA5E58936");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.paid_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.invoice).WithMany(p => p.payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__payments__invoic__373B3228");

            entity.HasOne(d => d.received_byNavigation).WithMany(p => p.payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__payments__receiv__3A179ED3");
        });

        modelBuilder.Entity<permission>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__permissi__3213E83F11D0894E");

            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");
        });

        modelBuilder.Entity<phi_access_log>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__phi_acce__3213E83F528D315A");

            entity.Property(e => e.accessed_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.accessed_byNavigation).WithMany(p => p.phi_access_logs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__phi_acces__acces__589C25F3");
        });

        modelBuilder.Entity<pre_admission_screening>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__pre_admi__3213E83F8DC369B9");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.resident).WithMany(p => p.pre_admission_screenings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pre_admis__resid__395884C4");

            entity.HasOne(d => d.screened_byNavigation).WithMany(p => p.pre_admission_screenings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pre_admis__scree__3A4CA8FD");
        });

        modelBuilder.Entity<resident>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__resident__3213E83F2459FE28");

            entity.HasIndex(e => e.status, "idx_residents_status").HasFilter("([is_deleted]=(0))");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.status).HasDefaultValue("PENDING");
            entity.Property(e => e.updated_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.address).WithMany(p => p.residents).HasConstraintName("FK__residents__addre__160F4887");

            entity.HasOne(d => d.bed).WithMany(p => p.residents).HasConstraintName("FK__residents__bed_i__17036CC0");
        });

        modelBuilder.Entity<resident_care_level_history>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__resident__3213E83F8359E53D");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.care_level).WithMany(p => p.resident_care_level_histories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___care___245D67DE");

            entity.HasOne(d => d.resident).WithMany(p => p.resident_care_level_histories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___resid__236943A5");
        });

        modelBuilder.Entity<resident_contact>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__resident__3213E83F8C1A7303");

            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.contact).WithMany(p => p.resident_contacts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___conta__2FCF1A8A");

            entity.HasOne(d => d.resident).WithMany(p => p.resident_contacts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___resid__2EDAF651");
        });

        modelBuilder.Entity<resident_insurance_policy>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__resident__3213E83F92B49655");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.insurance_provider).WithMany(p => p.resident_insurance_policies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___insur__2057CCD0");

            entity.HasOne(d => d.resident).WithMany(p => p.resident_insurance_policies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___resid__1F63A897");
        });

        modelBuilder.Entity<resident_sensitive_info>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__resident__3213E83F973E87A8");

            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.updated_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.resident).WithOne(p => p.resident_sensitive_info)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resident___resid__1DB06A4F");
        });

        modelBuilder.Entity<role>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__roles__3213E83F2F881D78");

            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.updated_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasMany(d => d.permissions).WithMany(p => p.roles)
                .UsingEntity<Dictionary<string, object>>(
                    "role_permission",
                    r => r.HasOne<permission>().WithMany()
                        .HasForeignKey("permission_id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__role_perm__permi__5535A963"),
                    l => l.HasOne<role>().WithMany()
                        .HasForeignKey("role_id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__role_perm__role___5441852A"),
                    j =>
                    {
                        j.HasKey("role_id", "permission_id").HasName("PK__role_per__C85A54635FA26B98");
                        j.ToTable("role_permissions");
                    });
        });

        modelBuilder.Entity<room>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__rooms__3213E83FB9F53800");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.facility).WithMany(p => p.rooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__rooms__facility___7A672E12");
        });

        modelBuilder.Entity<shift>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__shifts__3213E83FD3721F5E");

            entity.HasOne(d => d.facility).WithMany(p => p.shifts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__shifts__facility__11158940");
        });

        modelBuilder.Entity<shift_assignment>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__shift_as__3213E83F9D731F36");

            entity.Property(e => e.status).HasDefaultValue("SCHEDULED");

            entity.HasOne(d => d.shift).WithMany(p => p.shift_assignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__shift_ass__shift__15DA3E5D");

            entity.HasOne(d => d.user).WithMany(p => p.shift_assignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__shift_ass__user___16CE6296");
        });

        modelBuilder.Entity<sla_config>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__sla_conf__3213E83F938BDC1B");

            entity.HasOne(d => d.severity).WithMany(p => p.sla_configs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__sla_confi__sever__40C49C62");
        });

        modelBuilder.Entity<staffing_config>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__staffing__3213E83F7B0264F3");

            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.facility).WithMany(p => p.staffing_configs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__staffing___facil__04E4BC85");
        });

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__users__3213E83F4890964F");

            entity.HasIndex(e => e.status, "idx_users_status").HasFilter("([is_deleted]=(0))");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.status).HasDefaultValue("ACTIVE");
            entity.Property(e => e.updated_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.role).WithMany(p => p.users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__role_id__5DCAEF64");
        });

        modelBuilder.Entity<user_facility>(entity =>
        {
            entity.HasKey(e => new { e.user_id, e.facility_id }).HasName("PK__user_fac__4290B9A5B2192637");

            entity.HasOne(d => d.facility).WithMany(p => p.user_facilities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_faci__facil__74AE54BC");

            entity.HasOne(d => d.user).WithMany(p => p.user_facilities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_faci__user___73BA3083");
        });

        modelBuilder.Entity<vital_sign>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__vital_si__3213E83F19229DD5");

            entity.Property(e => e.recorded_at).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.recorded_byNavigation).WithMany(p => p.vital_signs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__vital_sig__recor__5D95E53A");

            entity.HasOne(d => d.resident).WithMany(p => p.vital_signs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__vital_sig__resid__5CA1C101");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
