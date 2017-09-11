using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class InsurancePlans
    {
        public InsurancePlans()
        {
            PatientsInsurancePlans = new HashSet<PatientsInsurancePlans>();
        }

        public int PlanId { get; set; }
        public string PlanType { get; set; }
        public string PlanIdentifier { get; set; }
        public string PolicyNumber { get; set; }
        public string GroupName { get; set; }
        public string GroupIdentifier { get; set; }
        public decimal CoPay { get; set; }
        public string Purchaser { get; set; }
        public bool IsActive { get; set; }
        public DateTime InActiveDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string PlanName { get; set; }
        public int CompanyId { get; set; }
        public Guid UserId { get; set; }
        public Guid LastUpdatedByUser { get; set; }

        public InsuranceProviders Company { get; set; }
        public ICollection<PatientsInsurancePlans> PatientsInsurancePlans { get; set; }
    }
}
