using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class PatientPhoneNumbers
    {
        public int PhoneId { get; set; }
        public string Number { get; set; }
        public string Extension { get; set; }
        public int Type { get; set; }
        public bool? IsPrimary { get; set; }
        public bool? RecieveText { get; set; }
        public Guid UserId { get; set; }
        public Guid LastUpdatedByUser { get; set; }

        public Patients User { get; set; }
    }
}
