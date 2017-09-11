using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class PatientDevices
    {
        public PatientDevices()
        {
            ReadingHeaders = new HashSet<ReadingHeaders>();
        }

        public int DeviceId { get; set; }
        public Guid UserId { get; set; }
        public int MeterIndex { get; set; }
        public string Manufacturer { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceName { get; set; }
        public string SerialNumber { get; set; }
        public string SoftwareVersion { get; set; }
        public string HardwareVersion { get; set; }
        public bool? IsCgmdata { get; set; }

        public Patients User { get; set; }
        public DeviceData DeviceData { get; set; }
        public DiabetesManagementData DiabetesManagementData { get; set; }
        public ICollection<ReadingHeaders> ReadingHeaders { get; set; }
    }
}
