using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class DiabetesControlTypes
    {
        public int TypeId { get; set; }
        public string ControlName { get; set; }
        public bool? IsEnabled { get; set; }
        public int DmdataId { get; set; }
        public int CareSettingsId { get; set; }
        public Guid LastUpdatedByUser { get; set; }

        public CareSettings CareSettings { get; set; }
    }
}
