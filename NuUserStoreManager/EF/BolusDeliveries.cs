using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class BolusDeliveries
    {
        public BolusDeliveries()
        {
            BolusDeliveryData = new HashSet<BolusDeliveryData>();
        }

        public int BolusDeliveryId { get; set; }
        public DateTime StartDateTime { get; set; }
        public double AmountDelivered { get; set; }
        public double AmountSuggested { get; set; }
        public int Duration { get; set; }
        public string BolusTrigger { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public Guid ReadingKeyId { get; set; }
        public Guid UserId { get; set; }

        public ReadingHeaders ReadingKey { get; set; }
        public Bgtargets Bgtargets { get; set; }
        public BolusCarbs BolusCarbs { get; set; }
        public CorrectionFactors CorrectionFactors { get; set; }
        public InsulinCarbRatio InsulinCarbRatio { get; set; }
        public InsulinCorrections InsulinCorrections { get; set; }
        public ICollection<BolusDeliveryData> BolusDeliveryData { get; set; }
    }
}
