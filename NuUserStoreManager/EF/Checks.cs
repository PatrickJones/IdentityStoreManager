using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class Checks
    {
        public int CheckId { get; set; }
        public int CheckStatus { get; set; }
        public string CheckNumber { get; set; }
        public long CheckCode { get; set; }
        public DateTime CheckDateRecieved { get; set; }
        public decimal CheckAmount { get; set; }

        public Payments Check { get; set; }
    }
}
