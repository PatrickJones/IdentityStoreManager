using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class Clinicians
    {
        public Guid UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string StateLicenseNumber { get; set; }
        public Guid InstitutionId { get; set; }
        public Guid LastUpdatedByUser { get; set; }
        public string Email { get; set; }

        public Institutions Institution { get; set; }
        public Users User { get; set; }
    }
}
