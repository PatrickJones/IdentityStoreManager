using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class PumpSettings
    {
        public int SettingId { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public string Description { get; set; }
        public Guid PumpKeyId { get; set; }
        public DateTime Date { get; set; }

        public Pumps PumpKey { get; set; }
    }
}
