using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuUserStoreManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace NuUserStoreManagerTest.UserManagerTest
{
    [TestClass]
    public class AddClaimAsyncTest : BaseUnitTest
    {
        [TestMethod]
        public void Add_Claim_Not_Supported()
        {
            var result = uManager.AddClaimAsync(new NuApplicationUser(), new Claim("","")).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.NotSupported("AddClaimAsync"), result.Errors.First().Description);
        }
    }
}
