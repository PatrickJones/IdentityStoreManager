using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class PumpPrograms
    {
        public PumpPrograms()
        {
            BasalProgramTimeSlots = new HashSet<BasalProgramTimeSlots>();
            BolusProgramTimeSlots = new HashSet<BolusProgramTimeSlots>();
        }

        public int PumpProgramId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Source { get; set; }
        public string ProgramName { get; set; }
        public int ProgramKey { get; set; }
        public bool? Valid { get; set; }
        public int NumOfSegments { get; set; }
        public Guid PumpKeyId { get; set; }

        public Pumps PumpKey { get; set; }
        public ICollection<BasalProgramTimeSlots> BasalProgramTimeSlots { get; set; }
        public ICollection<BolusProgramTimeSlots> BolusProgramTimeSlots { get; set; }
    }
}
