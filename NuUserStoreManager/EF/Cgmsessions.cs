using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class Cgmsessions
    {
        public DateTime SessionDateTime { get; set; }
        public int TimeInSeconds { get; set; }
        public bool? IsActive { get; set; }
        public Guid ReadingKeyId { get; set; }
        public DateTime Date { get; set; }
        public long CgmsessionId { get; set; }

        public ReadingHeaders ReadingKey { get; set; }
    }
}
