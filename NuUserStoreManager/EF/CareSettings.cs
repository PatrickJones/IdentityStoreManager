using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class CareSettings
    {
        public CareSettings()
        {
            DailyTimeSlots = new HashSet<DailyTimeSlots>();
            DiabetesControlTypes = new HashSet<DiabetesControlTypes>();
        }

        public int CareSettingsId { get; set; }
        public Guid UserId { get; set; }
        public int HyperglycemicLevel { get; set; }
        public int HypoglycemicLevel { get; set; }
        public int InsulinMethod { get; set; }
        public string DiabetesManagementType { get; set; }
        public int InsulinBrand { get; set; }
        public DateTime DateModified { get; set; }
        public Guid LastUpdatedByUser { get; set; }

        public Patients User { get; set; }
        public ICollection<DailyTimeSlots> DailyTimeSlots { get; set; }
        public ICollection<DiabetesControlTypes> DiabetesControlTypes { get; set; }
    }
}
