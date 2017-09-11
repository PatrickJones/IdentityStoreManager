using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class Payments
    {
        public int PaymentId { get; set; }
        public int PaymentMethod { get; set; }
        public bool PaymentApproved { get; set; }
        public DateTime ApprovalDate { get; set; }

        public Subscriptions Payment { get; set; }
        public Checks Checks { get; set; }
        public PayPal PayPal { get; set; }
    }
}
