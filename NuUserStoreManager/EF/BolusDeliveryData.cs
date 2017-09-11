using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class BolusDeliveryData
    {
        public int DataId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int BolusDeliveryId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public BolusDeliveries BolusDelivery { get; set; }
    }
}
