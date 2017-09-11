using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class ReadingHeaders
    {
        public ReadingHeaders()
        {
            BasalDeliveries = new HashSet<BasalDeliveries>();
            BloodGlucoseReadings = new HashSet<BloodGlucoseReadings>();
            BolusDeliveries = new HashSet<BolusDeliveries>();
            Cgmreminders = new HashSet<Cgmreminders>();
            Cgmsessions = new HashSet<Cgmsessions>();
            DeviceSettings = new HashSet<DeviceSettings>();
            NutritionReadings = new HashSet<NutritionReadings>();
            PhysiologicalReadings = new HashSet<PhysiologicalReadings>();
            ReadingErrors = new HashSet<ReadingErrors>();
            ReadingEvents = new HashSet<ReadingEvents>();
            TensReadings = new HashSet<TensReadings>();
            TotalDailyInsulinDeliveries = new HashSet<TotalDailyInsulinDeliveries>();
        }

        public int DeviceId { get; set; }
        public DateTime ServerDateTime { get; set; }
        public DateTime MeterDateTime { get; set; }
        public int Readings { get; set; }
        public string SiteSource { get; set; }
        public DateTime ReviewedOn { get; set; }
        public bool? IsCgmdata { get; set; }
        public Guid UserId { get; set; }
        public string LegacyDownloadKeyId { get; set; }
        public Guid ReadingKeyId { get; set; }

        public PatientDevices Device { get; set; }
        public Pumps Pumps { get; set; }
        public ICollection<BasalDeliveries> BasalDeliveries { get; set; }
        public ICollection<BloodGlucoseReadings> BloodGlucoseReadings { get; set; }
        public ICollection<BolusDeliveries> BolusDeliveries { get; set; }
        public ICollection<Cgmreminders> Cgmreminders { get; set; }
        public ICollection<Cgmsessions> Cgmsessions { get; set; }
        public ICollection<DeviceSettings> DeviceSettings { get; set; }
        public ICollection<NutritionReadings> NutritionReadings { get; set; }
        public ICollection<PhysiologicalReadings> PhysiologicalReadings { get; set; }
        public ICollection<ReadingErrors> ReadingErrors { get; set; }
        public ICollection<ReadingEvents> ReadingEvents { get; set; }
        public ICollection<TensReadings> TensReadings { get; set; }
        public ICollection<TotalDailyInsulinDeliveries> TotalDailyInsulinDeliveries { get; set; }
    }
}
