using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class Cgmreminders
    {
        public int ReminderId { get; set; }
        public string Type { get; set; }
        public bool? Enabled { get; set; }
        public string Time { get; set; }
        public Guid ReadingKeyId { get; set; }
        public DateTime Date { get; set; }

        public ReadingHeaders ReadingKey { get; set; }
    }
}
