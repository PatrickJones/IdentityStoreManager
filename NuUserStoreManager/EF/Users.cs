using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class Users
    {
        public Users()
        {
            UserAuthentications = new HashSet<UserAuthentications>();
        }

        public Guid UserId { get; set; }
        public int UserType { get; set; }
        public DateTime CreationDate { get; set; }

        public Clinicians Clinicians { get; set; }
        public Patients Patients { get; set; }
        public ICollection<UserAuthentications> UserAuthentications { get; set; }
    }
}
