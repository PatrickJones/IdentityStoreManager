using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class InsuranceContacts
    {
        public int ContactId { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int CompanyId { get; set; }
        public Guid LastUpdatedByUser { get; set; }

        public InsuranceProviders Company { get; set; }
    }
}
