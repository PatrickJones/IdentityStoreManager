using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class PatientAddresses
    {
        public int AddressId { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Street3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public Guid UserId { get; set; }
        public Guid LastUpdatedByUser { get; set; }

        public Patients User { get; set; }
    }
}
