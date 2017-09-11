using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class AppLoginHistories
    {
        public Guid UserId { get; set; }
        public DateTime LoginDate { get; set; }
        public Guid ApplicationId { get; set; }
        public int HistoryId { get; set; }

        public Applications Application { get; set; }
    }
}
