using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class InsuranceProviders
    {
        public InsuranceProviders()
        {
            InsuranceAddresses = new HashSet<InsuranceAddresses>();
            InsuranceContacts = new HashSet<InsuranceContacts>();
            InsurancePlans = new HashSet<InsurancePlans>();
        }

        public int CompanyId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public DateTime InActiveDate { get; set; }
        public Guid LastUpdatedByUser { get; set; }

        public ICollection<InsuranceAddresses> InsuranceAddresses { get; set; }
        public ICollection<InsuranceContacts> InsuranceContacts { get; set; }
        public ICollection<InsurancePlans> InsurancePlans { get; set; }
    }
}
