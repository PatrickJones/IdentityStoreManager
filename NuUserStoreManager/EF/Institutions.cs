using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class Institutions
    {
        public Institutions()
        {
            Clinicians = new HashSet<Clinicians>();
            PatientsInstitutions = new HashSet<PatientsInstitutions>();
            Subscriptions = new HashSet<Subscriptions>();
        }

        public Guid InstitutionId { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string ContactFirstname { get; set; }
        public string ContactLastname { get; set; }
        public string ContactEmail { get; set; }
        public Guid UserId { get; set; }
        public int LegacySiteId { get; set; }
        public int Licenses { get; set; }
        public Guid LastUpdatedByUser { get; set; }

        public ICollection<Clinicians> Clinicians { get; set; }
        public ICollection<PatientsInstitutions> PatientsInstitutions { get; set; }
        public ICollection<Subscriptions> Subscriptions { get; set; }
    }
}
