using Microsoft.AspNetCore.Identity;
using NuUserStoreManager.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace NuUserStoreManager
{
    public class NuApplicationUser : IdentityUser
    {
        public Guid UserId { get; set; }

        public NuApplicationUser(string userName) : base(userName)
        {}

        public NuApplicationUser() : base()
        {}
    }
}
