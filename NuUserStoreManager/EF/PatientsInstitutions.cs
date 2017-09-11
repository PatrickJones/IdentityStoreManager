using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class PatientsInstitutions
    {
        public Guid UserId { get; set; }
        public Guid InstitutionId { get; set; }

        public Institutions Institution { get; set; }
        public Patients User { get; set; }
    }
}
