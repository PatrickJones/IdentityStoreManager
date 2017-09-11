using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class DataShareRequestLog
    {
        public DataShareRequestLog()
        {
            SharedAreas = new HashSet<SharedAreas>();
        }

        public int RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid PatientId { get; set; }

        public ICollection<SharedAreas> SharedAreas { get; set; }
    }
}
