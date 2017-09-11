using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class BasalProgramTimeSlots
    {
        public int BasalSlotId { get; set; }
        public double BasalValue { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan StopTime { get; set; }
        public int PumpProgramId { get; set; }
        public DateTime DateSet { get; set; }

        public PumpPrograms PumpProgram { get; set; }
    }
}
