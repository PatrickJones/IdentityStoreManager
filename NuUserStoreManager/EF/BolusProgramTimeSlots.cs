using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class BolusProgramTimeSlots
    {
        public int BolusSlotId { get; set; }
        public double BolusValue { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan StopTime { get; set; }
        public int PumpProgramId { get; set; }
        public DateTime DateSet { get; set; }

        public PumpPrograms PumpProgram { get; set; }
    }
}
