using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NuUserStoreManager.EF
{
    public partial class NuMedicsGlobalContext : DbContext
    {
        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<AppLoginHistories> AppLoginHistories { get; set; }
        public virtual DbSet<AppSettings> AppSettings { get; set; }
        public virtual DbSet<AppUserSettings> AppUserSettings { get; set; }
        public virtual DbSet<BasalDeliveries> BasalDeliveries { get; set; }
        public virtual DbSet<BasalDeliveryData> BasalDeliveryData { get; set; }
        public virtual DbSet<BasalProgramTimeSlots> BasalProgramTimeSlots { get; set; }
        public virtual DbSet<Bgtargets> Bgtargets { get; set; }
        public virtual DbSet<BloodGlucoseReadings> BloodGlucoseReadings { get; set; }
        public virtual DbSet<BolusCarbs> BolusCarbs { get; set; }
        public virtual DbSet<BolusDeliveries> BolusDeliveries { get; set; }
        public virtual DbSet<BolusDeliveryData> BolusDeliveryData { get; set; }
        public virtual DbSet<BolusProgramTimeSlots> BolusProgramTimeSlots { get; set; }
        public virtual DbSet<CareSettings> CareSettings { get; set; }
        public virtual DbSet<Cgmreminders> Cgmreminders { get; set; }
        public virtual DbSet<Cgmsessions> Cgmsessions { get; set; }
        public virtual DbSet<Checks> Checks { get; set; }
        public virtual DbSet<CheckStatus> CheckStatus { get; set; }
        public virtual DbSet<Clinicians> Clinicians { get; set; }
        public virtual DbSet<CorrectionFactors> CorrectionFactors { get; set; }
        public virtual DbSet<DailyTimeSlots> DailyTimeSlots { get; set; }
        public virtual DbSet<DatabaseInfo> DatabaseInfo { get; set; }
        public virtual DbSet<DataShareCategories> DataShareCategories { get; set; }
        public virtual DbSet<DataShareRequestLog> DataShareRequestLog { get; set; }
        public virtual DbSet<DeviceData> DeviceData { get; set; }
        public virtual DbSet<DeviceSettings> DeviceSettings { get; set; }
        public virtual DbSet<DiabetesControlTypes> DiabetesControlTypes { get; set; }
        public virtual DbSet<DiabetesManagementData> DiabetesManagementData { get; set; }
        public virtual DbSet<EndUserLicenseAgreements> EndUserLicenseAgreements { get; set; }
        public virtual DbSet<Institutions> Institutions { get; set; }
        public virtual DbSet<InsulinBrands> InsulinBrands { get; set; }
        public virtual DbSet<InsulinCarbRatio> InsulinCarbRatio { get; set; }
        public virtual DbSet<InsulinCorrections> InsulinCorrections { get; set; }
        public virtual DbSet<InsulinMethods> InsulinMethods { get; set; }
        public virtual DbSet<InsulinTypes> InsulinTypes { get; set; }
        public virtual DbSet<InsuranceAddresses> InsuranceAddresses { get; set; }
        public virtual DbSet<InsuranceContacts> InsuranceContacts { get; set; }
        public virtual DbSet<InsurancePlans> InsurancePlans { get; set; }
        public virtual DbSet<InsuranceProviders> InsuranceProviders { get; set; }
        public virtual DbSet<LinkTypes> LinkTypes { get; set; }
        public virtual DbSet<NutritionReadings> NutritionReadings { get; set; }
        public virtual DbSet<PasswordHistories> PasswordHistories { get; set; }
        public virtual DbSet<PatientAddresses> PatientAddresses { get; set; }
        public virtual DbSet<PatientDevices> PatientDevices { get; set; }
        public virtual DbSet<PatientInstitutionLinkHistory> PatientInstitutionLinkHistory { get; set; }
        public virtual DbSet<PatientMedications> PatientMedications { get; set; }
        public virtual DbSet<PatientPhoneNumbers> PatientPhoneNumbers { get; set; }
        public virtual DbSet<PatientPhotos> PatientPhotos { get; set; }
        public virtual DbSet<Patients> Patients { get; set; }
        public virtual DbSet<PatientsInstitutions> PatientsInstitutions { get; set; }
        public virtual DbSet<PatientsInsurancePlans> PatientsInsurancePlans { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<PayPal> PayPal { get; set; }
        public virtual DbSet<PhysiologicalReadings> PhysiologicalReadings { get; set; }
        public virtual DbSet<PumpPrograms> PumpPrograms { get; set; }
        public virtual DbSet<Pumps> Pumps { get; set; }
        public virtual DbSet<PumpSettings> PumpSettings { get; set; }
        public virtual DbSet<ReadingErrors> ReadingErrors { get; set; }
        public virtual DbSet<ReadingEvents> ReadingEvents { get; set; }
        public virtual DbSet<ReadingEventTypes> ReadingEventTypes { get; set; }
        public virtual DbSet<ReadingHeaders> ReadingHeaders { get; set; }
        public virtual DbSet<SharedAreas> SharedAreas { get; set; }
        public virtual DbSet<Subscriptions> Subscriptions { get; set; }
        public virtual DbSet<SubscriptionType> SubscriptionType { get; set; }
        public virtual DbSet<TensReadings> TensReadings { get; set; }
        public virtual DbSet<TherapyTypes> TherapyTypes { get; set; }
        public virtual DbSet<TotalDailyInsulinDeliveries> TotalDailyInsulinDeliveries { get; set; }
        public virtual DbSet<UserAuthentications> UserAuthentications { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UserTypes> UserTypes { get; set; }

        // can only have a single option per instance.
        private bool UseDefaultBuilder { get; set; } = true;

        public NuMedicsGlobalContext(DbContextOptions<NuMedicsGlobalContext> options) : base(options) { UseDefaultBuilder = false; }
        public NuMedicsGlobalContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (UseDefaultBuilder)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=PRIMARYPC\LIONDEV2016;Database=NuMedicsGlobal;User=sa;Password=J1master&2pupilJ;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Applications>(entity =>
            {
                entity.HasKey(e => e.ApplicationId)
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.ApplicationName)
                    .HasName("UQ__Applicat__3091033102669C91")
                    .IsUnique();

                entity.Property(e => e.ApplicationId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ApplicationName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.BannerEnable).HasDefaultValueSql("((0))");

                entity.Property(e => e.BannerMessage).HasDefaultValueSql("('<div></div>')");

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.LastUpdatedbyUser).HasDefaultValueSql("(newid())");

                entity.Property(e => e.LicneseAgreement).HasColumnType("text");

                entity.Property(e => e.SupportAvailability).HasMaxLength(250);

                entity.Property(e => e.SupportEmail).HasMaxLength(150);

                entity.Property(e => e.SupportNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<AppLoginHistories>(entity =>
            {
                entity.HasKey(e => e.HistoryId);

                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AppLoginHistories)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppLoginHistories_Applications");
            });

            modelBuilder.Entity<AppSettings>(entity =>
            {
                entity.HasKey(e => e.AppSettingId);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AppSettings)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppSettings_Applications");
            });

            modelBuilder.Entity<AppUserSettings>(entity =>
            {
                entity.HasKey(e => e.AppUserSettingId);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Appication)
                    .WithMany(p => p.AppUserSettings)
                    .HasForeignKey(d => d.AppicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppUserSettings_Applications");
            });

            modelBuilder.Entity<BasalDeliveries>(entity =>
            {
                entity.HasKey(e => e.BasalDeliveryId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Duration)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.ReadingKey)
                    .WithMany(p => p.BasalDeliveries)
                    .HasForeignKey(d => d.ReadingKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BasalDeliveries_ReadingHeaders1");
            });

            modelBuilder.Entity<BasalDeliveryData>(entity =>
            {
                entity.HasKey(e => e.DataId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.BasalDelivery)
                    .WithMany(p => p.BasalDeliveryData)
                    .HasForeignKey(d => d.BasalDeliveryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BasalDeliveryData_BasalDeliveries");
            });

            modelBuilder.Entity<BasalProgramTimeSlots>(entity =>
            {
                entity.HasKey(e => e.BasalSlotId);

                entity.Property(e => e.DateSet).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("time(2)");

                entity.Property(e => e.StopTime).HasColumnType("time(2)");

                entity.HasOne(d => d.PumpProgram)
                    .WithMany(p => p.BasalProgramTimeSlots)
                    .HasForeignKey(d => d.PumpProgramId)
                    .HasConstraintName("FK_PumpProgramTimeSlots_PumpPrograms");
            });

            modelBuilder.Entity<Bgtargets>(entity =>
            {
                entity.HasKey(e => e.TargetId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("BGTargets");

                entity.Property(e => e.TargetId).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.TargetBg).HasColumnName("TargetBG");

                entity.Property(e => e.TargetBgcorrect).HasColumnName("TargetBGCorrect");

                entity.HasOne(d => d.Target)
                    .WithOne(p => p.Bgtargets)
                    .HasForeignKey<Bgtargets>(d => d.TargetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BGTargets_BolusDelivery");
            });

            modelBuilder.Entity<BloodGlucoseReadings>(entity =>
            {
                entity.HasKey(e => e.ReadingId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Active).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsCgmdata)
                    .HasColumnName("IsCGMData")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ReadingDateTime).HasColumnType("datetime");

                entity.Property(e => e.Units).HasMaxLength(50);

                entity.Property(e => e.Value).HasMaxLength(50);

                entity.HasOne(d => d.ReadingKey)
                    .WithMany(p => p.BloodGlucoseReadings)
                    .HasForeignKey(d => d.ReadingKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BloodGlucoseReadings_ReadingHeaders");
            });

            modelBuilder.Entity<BolusCarbs>(entity =>
            {
                entity.HasKey(e => e.CarbId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.CarbId).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Carb)
                    .WithOne(p => p.BolusCarbs)
                    .HasForeignKey<BolusCarbs>(d => d.CarbId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BolusCarbs_BolusDelivery");
            });

            modelBuilder.Entity<BolusDeliveries>(entity =>
            {
                entity.HasKey(e => e.BolusDeliveryId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.AmountDelivered).HasDefaultValueSql("((0))");

                entity.Property(e => e.AmountSuggested).HasDefaultValueSql("((0))");

                entity.Property(e => e.BolusTrigger).HasMaxLength(50);

                entity.Property(e => e.Duration).HasDefaultValueSql("((0))");

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.ReadingKey)
                    .WithMany(p => p.BolusDeliveries)
                    .HasForeignKey(d => d.ReadingKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BolusDelivery_ReadingHeaders1");
            });

            modelBuilder.Entity<BolusDeliveryData>(entity =>
            {
                entity.HasKey(e => e.DataId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.BolusDelivery)
                    .WithMany(p => p.BolusDeliveryData)
                    .HasForeignKey(d => d.BolusDeliveryId)
                    .HasConstraintName("FK_BolusDeliveryData_BolusDelivery");
            });

            modelBuilder.Entity<BolusProgramTimeSlots>(entity =>
            {
                entity.HasKey(e => e.BolusSlotId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.DateSet).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("time(2)");

                entity.Property(e => e.StopTime).HasColumnType("time(2)");

                entity.HasOne(d => d.PumpProgram)
                    .WithMany(p => p.BolusProgramTimeSlots)
                    .HasForeignKey(d => d.PumpProgramId)
                    .HasConstraintName("FK_BolusProgramTimeSlots_PumpPrograms");
            });

            modelBuilder.Entity<CareSettings>(entity =>
            {
                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.DiabetesManagementType).HasMaxLength(150);

                entity.Property(e => e.HyperglycemicLevel).HasDefaultValueSql("((0))");

                entity.Property(e => e.HypoglycemicLevel).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CareSettings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CareSettings_Patients1");
            });

            modelBuilder.Entity<Cgmreminders>(entity =>
            {
                entity.HasKey(e => e.ReminderId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("CGMReminders");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Enabled).HasDefaultValueSql("((0))");

                entity.Property(e => e.Time)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ReadingKey)
                    .WithMany(p => p.Cgmreminders)
                    .HasForeignKey(d => d.ReadingKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CGMReminders_ReadingHeaders");
            });

            modelBuilder.Entity<Cgmsessions>(entity =>
            {
                entity.HasKey(e => e.CgmsessionId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("CGMSessions");

                entity.Property(e => e.CgmsessionId).HasColumnName("CGMSessionId");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.SessionDateTime).HasColumnType("datetime");

                entity.Property(e => e.TimeInSeconds).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ReadingKey)
                    .WithMany(p => p.Cgmsessions)
                    .HasForeignKey(d => d.ReadingKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CGMSessions_ReadingHeaders");
            });

            modelBuilder.Entity<Checks>(entity =>
            {
                entity.HasKey(e => e.CheckId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.CheckId).ValueGeneratedNever();

                entity.Property(e => e.CheckAmount).HasColumnType("money");

                entity.Property(e => e.CheckDateRecieved).HasColumnType("datetime");

                entity.Property(e => e.CheckNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Check)
                    .WithOne(p => p.Checks)
                    .HasForeignKey<Checks>(d => d.CheckId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Checks_Payments");
            });

            modelBuilder.Entity<CheckStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Clinicians>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.StateLicenseNumber).HasMaxLength(150);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.Clinicians)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Clinicians_Institution");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Clinicians)
                    .HasForeignKey<Clinicians>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Clinicians_Users");
            });

            modelBuilder.Entity<CorrectionFactors>(entity =>
            {
                entity.HasKey(e => e.FactorId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.FactorId).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Factor)
                    .WithOne(p => p.CorrectionFactors)
                    .HasForeignKey<CorrectionFactors>(d => d.FactorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CorrectionFactors_BolusDelivery");
            });

            modelBuilder.Entity<DailyTimeSlots>(entity =>
            {
                entity.HasKey(e => e.TimeSlotId);

                entity.Property(e => e.TimeSlotBoundary)
                    .HasColumnName("TImeSlotBoundary")
                    .HasColumnType("time(2)");

                entity.Property(e => e.TimeSlotDescription).HasMaxLength(50);

                entity.HasOne(d => d.CareSettings)
                    .WithMany(p => p.DailyTimeSlots)
                    .HasForeignKey(d => d.CareSettingsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DailyTimeSlots_CareSettings");
            });

            modelBuilder.Entity<DataShareCategories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<DataShareRequestLog>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.RequestDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DeviceData>(entity =>
            {
                entity.HasKey(e => e.DataSetId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.DataSetId).ValueGeneratedOnAdd();

                entity.Property(e => e.DataSet)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.HasOne(d => d.DataSetNavigation)
                    .WithOne(p => p.DeviceData)
                    .HasForeignKey<DeviceData>(d => d.DataSetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceData_PatientDevices");
            });

            modelBuilder.Entity<DeviceSettings>(entity =>
            {
                entity.HasKey(e => e.SettingId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ReadingDate).HasColumnType("datetime");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.ReadingKey)
                    .WithMany(p => p.DeviceSettings)
                    .HasForeignKey(d => d.ReadingKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceSettings_ReadingHeaders");
            });

            modelBuilder.Entity<DiabetesControlTypes>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.ControlName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.DmdataId).HasColumnName("DMDataId");

                entity.Property(e => e.IsEnabled).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.CareSettings)
                    .WithMany(p => p.DiabetesControlTypes)
                    .HasForeignKey(d => d.CareSettingsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiabetesControlTypes_CareSettings");
            });

            modelBuilder.Entity<DiabetesManagementData>(entity =>
            {
                entity.HasKey(e => e.DmdataId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.DmdataId)
                    .HasColumnName("DMDataId")
                    .ValueGeneratedNever();

                entity.Property(e => e.HighBglevel)
                    .HasColumnName("HighBGLevel")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LowBglevel)
                    .HasColumnName("LowBGLevel")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PostmealTarget).HasDefaultValueSql("((0))");

                entity.Property(e => e.PremealTarget).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Dmdata)
                    .WithOne(p => p.DiabetesManagementData)
                    .HasForeignKey<DiabetesManagementData>(d => d.DmdataId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiabetesManagementData_PatientDevices");
            });

            modelBuilder.Entity<EndUserLicenseAgreements>(entity =>
            {
                entity.HasKey(e => e.AgreementId);

                entity.Property(e => e.AgreementDate).HasColumnType("datetime");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.EndUserLicenseAgreements)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EndUserLicenseAgreements_Applications");
            });

            modelBuilder.Entity<Institutions>(entity =>
            {
                entity.HasKey(e => e.InstitutionId);

                entity.Property(e => e.InstitutionId).ValueGeneratedNever();

                entity.Property(e => e.City).HasMaxLength(250);

                entity.Property(e => e.ContactEmail).HasMaxLength(150);

                entity.Property(e => e.ContactFirstname).HasMaxLength(150);

                entity.Property(e => e.ContactLastname).HasMaxLength(150);

                entity.Property(e => e.Country).HasMaxLength(250);

                entity.Property(e => e.LegacySiteId).HasDefaultValueSql("((0))");

                entity.Property(e => e.Licenses).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.State).HasMaxLength(250);

                entity.Property(e => e.Street).HasMaxLength(250);

                entity.Property(e => e.Zip).HasMaxLength(50);
            });

            modelBuilder.Entity<InsulinBrands>(entity =>
            {
                entity.HasKey(e => e.InsulinBrandId);

                entity.Property(e => e.BrandName)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.Manufacturer).HasMaxLength(80);
            });

            modelBuilder.Entity<InsulinCarbRatio>(entity =>
            {
                entity.HasKey(e => e.RatioId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.RatioId).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Icratio).HasColumnName("ICRatio");

                entity.HasOne(d => d.Ratio)
                    .WithOne(p => p.InsulinCarbRatio)
                    .HasForeignKey<InsulinCarbRatio>(d => d.RatioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InsulinCarbRatio_BolusDelivery");
            });

            modelBuilder.Entity<InsulinCorrections>(entity =>
            {
                entity.HasKey(e => e.CorrectionId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.CorrectionId).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Correction)
                    .WithOne(p => p.InsulinCorrections)
                    .HasForeignKey<InsulinCorrections>(d => d.CorrectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InsulinCorrections_BolusDelivery");
            });

            modelBuilder.Entity<InsulinMethods>(entity =>
            {
                entity.HasKey(e => e.InsulinMethodId);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Method)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<InsulinTypes>(entity =>
            {
                entity.HasKey(e => e.InsulinTypeId);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<InsuranceAddresses>(entity =>
            {
                entity.HasKey(e => e.AddressId);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.County).HasMaxLength(250);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Street1)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Street2).HasMaxLength(250);

                entity.Property(e => e.Street3).HasMaxLength(250);

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.InsuranceAddresses)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_InsuranceAddresses_InsuranceProviders");
            });

            modelBuilder.Entity<InsuranceContacts>(entity =>
            {
                entity.HasKey(e => e.ContactId);

                entity.Property(e => e.Email).HasMaxLength(80);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.InsuranceContacts)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_InsuranceContacts_InsuranceProviders");
            });

            modelBuilder.Entity<InsurancePlans>(entity =>
            {
                entity.HasKey(e => e.PlanId);

                entity.Property(e => e.CoPay).HasColumnType("money");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.GroupIdentifier).HasMaxLength(150);

                entity.Property(e => e.GroupName).HasMaxLength(150);

                entity.Property(e => e.InActiveDate).HasColumnType("datetime");

                entity.Property(e => e.PlanIdentifier).HasMaxLength(150);

                entity.Property(e => e.PlanName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.PlanType).HasMaxLength(150);

                entity.Property(e => e.PolicyNumber).HasMaxLength(150);

                entity.Property(e => e.Purchaser).HasMaxLength(150);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.InsurancePlans)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_InsurancePlans_InsuranceProviders");
            });

            modelBuilder.Entity<InsuranceProviders>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.Property(e => e.InActiveDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<LinkTypes>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<NutritionReadings>(entity =>
            {
                entity.HasKey(e => e.ReadingId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Calories).HasDefaultValueSql("((0))");

                entity.Property(e => e.Carbohydrates).HasDefaultValueSql("((0))");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Fat).HasDefaultValueSql("((0))");

                entity.Property(e => e.Protien).HasDefaultValueSql("((0))");

                entity.Property(e => e.ReadingDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.ReadingKey)
                    .WithMany(p => p.NutritionReadings)
                    .HasForeignKey(d => d.ReadingKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NutritionReadings_ReadingHeaders1");
            });

            modelBuilder.Entity<PasswordHistories>(entity =>
            {
                entity.HasKey(e => e.HistoryId);

                entity.Property(e => e.LastDateUsed).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Authentication)
                    .WithMany(p => p.PasswordHistories)
                    .HasForeignKey(d => d.AuthenticationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PasswordHistories_UserAuthentications");
            });

            modelBuilder.Entity<PatientAddresses>(entity =>
            {
                entity.HasKey(e => e.AddressId);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.County).HasMaxLength(250);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Street1)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Street2).HasMaxLength(250);

                entity.Property(e => e.Street3).HasMaxLength(250);

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PatientAddresses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PatientAddresses_Patients");
            });

            modelBuilder.Entity<PatientDevices>(entity =>
            {
                entity.HasKey(e => e.DeviceId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.DeviceModel).HasMaxLength(80);

                entity.Property(e => e.DeviceName).HasMaxLength(80);

                entity.Property(e => e.HardwareVersion).HasMaxLength(50);

                entity.Property(e => e.IsCgmdata)
                    .HasColumnName("IsCGMData")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Manufacturer).HasMaxLength(150);

                entity.Property(e => e.MeterIndex).HasDefaultValueSql("((0))");

                entity.Property(e => e.SerialNumber).HasMaxLength(50);

                entity.Property(e => e.SoftwareVersion).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PatientDevices)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PatientDevices_Patients");
            });

            modelBuilder.Entity<PatientInstitutionLinkHistory>(entity =>
            {
                entity.HasKey(e => e.LinkId);

                entity.Property(e => e.Date).HasColumnType("date");
            });

            modelBuilder.Entity<PatientMedications>(entity =>
            {
                entity.HasKey(e => e.PatientMedicationId);

                entity.Property(e => e.LastUpdated).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PatientMedications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientMedications_Patients");
            });

            modelBuilder.Entity<PatientPhoneNumbers>(entity =>
            {
                entity.HasKey(e => e.PhoneId);

                entity.Property(e => e.Extension).HasMaxLength(50);

                entity.Property(e => e.IsPrimary).HasDefaultValueSql("((0))");

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RecieveText).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PatientPhoneNumbers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PatientPhoneNumbers_Patients");
            });

            modelBuilder.Entity<PatientPhotos>(entity =>
            {
                entity.HasKey(e => e.PhotoId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.Photo)
                    .IsRequired()
                    .HasColumnType("image");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PatientPhotos)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PatientPhotos_Patients");
            });

            modelBuilder.Entity<Patients>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.DateofBirth).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.Middlename).HasMaxLength(80);

                entity.Property(e => e.Mrid)
                    .HasColumnName("MRID")
                    .HasMaxLength(150);

                entity.Property(e => e.Race).HasMaxLength(20);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Patients)
                    .HasForeignKey<Patients>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patients_Users");
            });

            modelBuilder.Entity<PatientsInstitutions>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.InstitutionId });

                entity.ToTable("Patients_Institutions");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.PatientsInstitutions)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patients_Institutions_Institutions");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PatientsInstitutions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patients_Institutions_Patients");
            });

            modelBuilder.Entity<PatientsInsurancePlans>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PlanId });

                entity.ToTable("Patients_InsurancePlans");

                entity.HasOne(d => d.Plan)
                    .WithMany(p => p.PatientsInsurancePlans)
                    .HasForeignKey(d => d.PlanId)
                    .HasConstraintName("FK_Patients_InsurancePlans_InsurancePlans");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PatientsInsurancePlans)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Patients_InsurancePlans_Patients");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.HasKey(e => e.MethodId);

                entity.Property(e => e.MethodName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Payments>(entity =>
            {
                entity.HasKey(e => e.PaymentId);

                entity.Property(e => e.PaymentId).ValueGeneratedNever();

                entity.Property(e => e.ApprovalDate).HasColumnType("datetime");

                entity.HasOne(d => d.Payment)
                    .WithOne(p => p.Payments)
                    .HasForeignKey<Payments>(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payments_Subscriptions");
            });

            modelBuilder.Entity<PayPal>(entity =>
            {
                entity.Property(e => e.PayPalId).ValueGeneratedNever();

                entity.Property(e => e.McFee)
                    .HasColumnName("mc_fee")
                    .HasColumnType("money");

                entity.Property(e => e.McGross)
                    .HasColumnName("mc_gross")
                    .HasColumnType("money");

                entity.Property(e => e.ParentTxnId)
                    .HasColumnName("parent_txn_id")
                    .HasMaxLength(50);

                entity.Property(e => e.PayPalPostVars)
                    .IsRequired()
                    .HasColumnName("PayPal_post_vars");

                entity.Property(e => e.PaymentDate)
                    .HasColumnName("payment_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.PaymentStatus)
                    .HasColumnName("payment_status")
                    .HasMaxLength(50);

                entity.Property(e => e.PendingReason)
                    .HasColumnName("pending_reason")
                    .HasMaxLength(50);

                entity.Property(e => e.ReasonCode)
                    .HasColumnName("reason_code")
                    .HasMaxLength(50);

                entity.Property(e => e.SourceIp)
                    .IsRequired()
                    .HasColumnName("SourceIP")
                    .HasMaxLength(50);

                entity.Property(e => e.TxnId)
                    .IsRequired()
                    .HasColumnName("txn_id")
                    .HasMaxLength(50);

                entity.HasOne(d => d.PayPalNavigation)
                    .WithOne(p => p.PayPal)
                    .HasForeignKey<PayPal>(d => d.PayPalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PayPal_Payments");
            });

            modelBuilder.Entity<PhysiologicalReadings>(entity =>
            {
                entity.HasKey(e => e.ReadingId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.ReadingKey)
                    .WithMany(p => p.PhysiologicalReadings)
                    .HasForeignKey(d => d.ReadingKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PhysiologicalReadings_ReadingHeaders1");
            });

            modelBuilder.Entity<PumpPrograms>(entity =>
            {
                entity.HasKey(e => e.PumpProgramId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.NumOfSegments).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProgramName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Source).HasMaxLength(50);

                entity.Property(e => e.Valid).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.PumpKey)
                    .WithMany(p => p.PumpPrograms)
                    .HasForeignKey(d => d.PumpKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PumpPrograms_Pumps1");
            });

            modelBuilder.Entity<Pumps>(entity =>
            {
                entity.HasKey(e => e.PumpKeyId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.PumpKeyId).ValueGeneratedNever();

                entity.Property(e => e.Notes).HasMaxLength(200);

                entity.Property(e => e.PumpInfusionSet).HasMaxLength(50);

                entity.Property(e => e.PumpName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PumpStartDate).HasColumnType("datetime");

                entity.Property(e => e.PumpType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ReplacementDate).HasColumnType("datetime");

                entity.HasOne(d => d.PumpKey)
                    .WithOne(p => p.Pumps)
                    .HasForeignKey<Pumps>(d => d.PumpKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pumps_ReadingHeaders");
            });

            modelBuilder.Entity<PumpSettings>(entity =>
            {
                entity.HasKey(e => e.SettingId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(150);

                entity.Property(e => e.SettingName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.SettingValue)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.PumpKey)
                    .WithMany(p => p.PumpSettings)
                    .HasForeignKey(d => d.PumpKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PumpSettings_Pumps1");
            });

            modelBuilder.Entity<ReadingErrors>(entity =>
            {
                entity.HasKey(e => e.ErrorId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.ErrorName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ErrorText).HasMaxLength(250);

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.ReadingKey)
                    .WithMany(p => p.ReadingErrors)
                    .HasForeignKey(d => d.ReadingKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReadingErrors_ReadingHeaders1");
            });

            modelBuilder.Entity<ReadingEvents>(entity =>
            {
                entity.HasKey(e => e.Eventid)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.EventTime).HasColumnType("datetime");

                entity.Property(e => e.EventValue)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ResumeTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.StopTime).HasColumnType("datetime");

                entity.HasOne(d => d.ReadingKey)
                    .WithMany(p => p.ReadingEvents)
                    .HasForeignKey(d => d.ReadingKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReadingEvents_ReadingHeaders1");
            });

            modelBuilder.Entity<ReadingEventTypes>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ReadingHeaders>(entity =>
            {
                entity.HasKey(e => e.ReadingKeyId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.ReadingKeyId).ValueGeneratedNever();

                entity.Property(e => e.IsCgmdata)
                    .HasColumnName("IsCGMData")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LegacyDownloadKeyId).HasMaxLength(80);

                entity.Property(e => e.MeterDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1)-(1))-(1900))");

                entity.Property(e => e.Readings).HasDefaultValueSql("((0))");

                entity.Property(e => e.ReviewedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1)-(1))-(1900))");

                entity.Property(e => e.ServerDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1)-(1))-(1900))");

                entity.Property(e => e.SiteSource).HasMaxLength(50);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.ReadingHeaders)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_MeterReadingHeader_PatientDevices");
            });

            modelBuilder.Entity<SharedAreas>(entity =>
            {
                entity.HasKey(e => e.ShareId);

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.SharedAreas)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SharedAreas_DataShareRequestLog");
            });

            modelBuilder.Entity<Subscriptions>(entity =>
            {
                entity.HasKey(e => e.SubscriptionId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.IsTrial).HasDefaultValueSql("((0))");

                entity.Property(e => e.SubscriptionDate).HasColumnType("datetime");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subscriptions_Institutions");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subscriptions_Patients");
            });

            modelBuilder.Entity<SubscriptionType>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(150);

                entity.Property(e => e.Label).HasMaxLength(150);

                entity.Property(e => e.Price).HasColumnType("money");
            });

            modelBuilder.Entity<TensReadings>(entity =>
            {
                entity.HasKey(e => e.ReadingId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.ReadingDate).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasMaxLength(50);

                entity.HasOne(d => d.ReadingKey)
                    .WithMany(p => p.TensReadings)
                    .HasForeignKey(d => d.ReadingKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TensReadings_ReadingHeaders1");
            });

            modelBuilder.Entity<TherapyTypes>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<TotalDailyInsulinDeliveries>(entity =>
            {
                entity.HasKey(e => e.DeliveryId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.BasalDelivered).HasDefaultValueSql("((0))");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Suspended).HasDefaultValueSql("((0))");

                entity.Property(e => e.TempActivated).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalDelivered).HasDefaultValueSql("((0))");

                entity.Property(e => e.Valid).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ReadingKey)
                    .WithMany(p => p.TotalDailyInsulinDeliveries)
                    .HasForeignKey(d => d.ReadingKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TotalDailyInsulinDeliveries_ReadingHeaders1");
            });

            modelBuilder.Entity<UserAuthentications>(entity =>
            {
                entity.HasKey(e => e.AuthenticationId);

                entity.Property(e => e.LastActivityDate).HasColumnType("datetime");

                entity.Property(e => e.LastLockOutDate).HasColumnType("datetime");

                entity.Property(e => e.NotApprovedReason).HasMaxLength(1000);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.PasswordAnswer).HasMaxLength(250);

                entity.Property(e => e.PasswordQuestion).HasMaxLength(250);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.UserAuthentications)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAuthentications_Applications");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAuthentications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAuthentications_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<UserTypes>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
