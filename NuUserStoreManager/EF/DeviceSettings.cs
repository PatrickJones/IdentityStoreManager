using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class DeviceSettings
    {
        public int SettingId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public Guid ReadingKeyId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ReadingDate { get; set; }

        public ReadingHeaders ReadingKey { get; set; }
    }
}
