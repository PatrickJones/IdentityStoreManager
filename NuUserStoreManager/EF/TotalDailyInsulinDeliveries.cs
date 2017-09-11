using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class TotalDailyInsulinDeliveries
    {
        public int DeliveryId { get; set; }
        public double TotalDelivered { get; set; }
        public double BasalDelivered { get; set; }
        public bool? Suspended { get; set; }
        public bool? TempActivated { get; set; }
        public bool? Valid { get; set; }
        public double BolusDelivered { get; set; }
        public Guid ReadingKeyId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }

        public ReadingHeaders ReadingKey { get; set; }
    }
}
