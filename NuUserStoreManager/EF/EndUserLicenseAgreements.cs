using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class EndUserLicenseAgreements
    {
        public Guid UserId { get; set; }
        public DateTime AgreementDate { get; set; }
        public Guid ApplicationId { get; set; }
        public int AgreementId { get; set; }

        public Applications Application { get; set; }
    }
}
