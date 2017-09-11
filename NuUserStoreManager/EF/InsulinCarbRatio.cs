using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class InsulinCarbRatio
    {
        public int RatioId { get; set; }
        public int Icratio { get; set; }
        public DateTime Date { get; set; }

        public BolusDeliveries Ratio { get; set; }
    }
}
