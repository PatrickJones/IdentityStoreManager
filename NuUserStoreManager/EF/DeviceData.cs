using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class DeviceData
    {
        public int DataSetId { get; set; }
        public string DataSet { get; set; }
        public DateTime LastUpdate { get; set; }

        public PatientDevices DataSetNavigation { get; set; }
    }
}
