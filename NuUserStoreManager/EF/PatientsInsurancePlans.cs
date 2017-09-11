using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class PatientsInsurancePlans
    {
        public Guid UserId { get; set; }
        public int PlanId { get; set; }

        public InsurancePlans Plan { get; set; }
        public Patients User { get; set; }
    }
}
