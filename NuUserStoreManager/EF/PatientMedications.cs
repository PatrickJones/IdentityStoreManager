using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class PatientMedications
    {
        public string Name { get; set; }
        public long MedicationId { get; set; }
        public DateTime LastUpdated { get; set; }
        public int HourlyDosageInterval { get; set; }
        public int DosageTakenDaily { get; set; }
        public Guid UserId { get; set; }
        public int PatientMedicationId { get; set; }

        public Patients User { get; set; }
    }
}
