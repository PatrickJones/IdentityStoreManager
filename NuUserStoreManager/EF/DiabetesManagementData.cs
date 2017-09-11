using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class DiabetesManagementData
    {
        public int DmdataId { get; set; }
        public int LowBglevel { get; set; }
        public int HighBglevel { get; set; }
        public int PremealTarget { get; set; }
        public int PostmealTarget { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid ModifiedUserId { get; set; }
        public Guid UserId { get; set; }

        public PatientDevices Dmdata { get; set; }
    }
}
