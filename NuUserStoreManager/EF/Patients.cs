using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class Patients
    {
        public Patients()
        {
            CareSettings = new HashSet<CareSettings>();
            PatientAddresses = new HashSet<PatientAddresses>();
            PatientDevices = new HashSet<PatientDevices>();
            PatientMedications = new HashSet<PatientMedications>();
            PatientPhoneNumbers = new HashSet<PatientPhoneNumbers>();
            PatientPhotos = new HashSet<PatientPhotos>();
            PatientsInstitutions = new HashSet<PatientsInstitutions>();
            PatientsInsurancePlans = new HashSet<PatientsInsurancePlans>();
            Subscriptions = new HashSet<Subscriptions>();
        }

        public Guid UserId { get; set; }
        public string Mrid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public int Gender { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Race { get; set; }
        public int PlanId { get; set; }
        public string Email { get; set; }
        public Guid InstitutionId { get; set; }
        public Guid LastUpdatedByUser { get; set; }

        public Users User { get; set; }
        public ICollection<CareSettings> CareSettings { get; set; }
        public ICollection<PatientAddresses> PatientAddresses { get; set; }
        public ICollection<PatientDevices> PatientDevices { get; set; }
        public ICollection<PatientMedications> PatientMedications { get; set; }
        public ICollection<PatientPhoneNumbers> PatientPhoneNumbers { get; set; }
        public ICollection<PatientPhotos> PatientPhotos { get; set; }
        public ICollection<PatientsInstitutions> PatientsInstitutions { get; set; }
        public ICollection<PatientsInsurancePlans> PatientsInsurancePlans { get; set; }
        public ICollection<Subscriptions> Subscriptions { get; set; }
    }
}
