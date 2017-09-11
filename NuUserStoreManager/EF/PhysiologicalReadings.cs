using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class PhysiologicalReadings
    {
        public int ReadingId { get; set; }
        public DateTime Time { get; set; }
        public int Weight { get; set; }
        public int Hunger { get; set; }
        public int Nausea { get; set; }
        public int CaloriesBurned { get; set; }
        public int CaloriesConsumed { get; set; }
        public Guid ReadingKeyId { get; set; }
        public Guid UserId { get; set; }

        public ReadingHeaders ReadingKey { get; set; }
    }
}
