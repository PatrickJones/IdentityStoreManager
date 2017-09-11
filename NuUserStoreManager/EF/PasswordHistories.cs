using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class PasswordHistories
    {
        public string Password { get; set; }
        public Guid UserId { get; set; }
        public DateTime LastDateUsed { get; set; }
        public int AuthenticationId { get; set; }
        public int HistoryId { get; set; }

        public UserAuthentications Authentication { get; set; }
    }
}
