using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class BloodGlucoseReadings
    {
        public int ReadingId { get; set; }
        public DateTime ReadingDateTime { get; set; }
        public string Units { get; set; }
        public string Value { get; set; }
        public Guid UserId { get; set; }
        public bool? Active { get; set; }
        public Guid ReadingKeyId { get; set; }
        public bool? IsCgmdata { get; set; }

        public ReadingHeaders ReadingKey { get; set; }
    }
}
