using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuUserStoreManager;
using System.Security.Claims;
using System.Linq;

namespace NuUserStoreManagerTest.UserManagerTest
{
    [TestClass]
    public class AddClaimsAsyncTest : BaseUnitTest
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var result = uManager.AddClaimAsync(new NuApplicationUser(), new Claim("", "")).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.NotSupported("AddClaimsAsync"), result.Errors.First().Description);
        }
    }
}
