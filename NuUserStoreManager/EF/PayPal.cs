using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class PayPal
    {
        public int PayPalId { get; set; }
        public string TxnId { get; set; }
        public string ParentTxnId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string PendingReason { get; set; }
        public string ReasonCode { get; set; }
        public decimal McGross { get; set; }
        public decimal McFee { get; set; }
        public string PayPalPostVars { get; set; }
        public string SourceIp { get; set; }
        public int PaymentId { get; set; }

        public Payments PayPalNavigation { get; set; }
    }
}
