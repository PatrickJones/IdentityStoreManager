using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class Pumps
    {
        public Pumps()
        {
            PumpPrograms = new HashSet<PumpPrograms>();
            PumpSettings = new HashSet<PumpSettings>();
        }

        public string PumpType { get; set; }
        public string PumpName { get; set; }
        public DateTime PumpStartDate { get; set; }
        public string PumpInfusionSet { get; set; }
        public int ActiveProgramId { get; set; }
        public double Cannula { get; set; }
        public DateTime ReplacementDate { get; set; }
        public string Notes { get; set; }
        public Guid UserId { get; set; }
        public Guid PumpKeyId { get; set; }

        public ReadingHeaders PumpKey { get; set; }
        public ICollection<PumpPrograms> PumpPrograms { get; set; }
        public ICollection<PumpSettings> PumpSettings { get; set; }
    }
}
