using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class Subscriptions
    {
        public int SubscriptionId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public int UserType { get; set; }
        public int SubscriptionType { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool? IsTrial { get; set; }
        public Guid InstitutionId { get; set; }

        public Institutions Institution { get; set; }
        public Patients User { get; set; }
        public Payments Payments { get; set; }
    }
}
