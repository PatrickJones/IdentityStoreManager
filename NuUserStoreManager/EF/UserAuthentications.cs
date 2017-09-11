using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class UserAuthentications
    {
        public UserAuthentications()
        {
            PasswordHistories = new HashSet<PasswordHistories>();
        }

        public int AuthenticationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public int PasswordFailureCount { get; set; }
        public int PasswordAnswerFailureCount { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime LastLockOutDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsTempPassword { get; set; }
        public bool IsloggedIn { get; set; }
        public string NotApprovedReason { get; set; }
        public Guid LastUpdatedByUser { get; set; }

        public Applications Application { get; set; }
        public Users User { get; set; }
        public ICollection<PasswordHistories> PasswordHistories { get; set; }
    }
}
