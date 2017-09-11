using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NuUserStoreManagerTest.UserManagerTest
{
    [TestClass]
    public class DefaultPropertiesTest : BaseUnitTest
    {
        [TestMethod]
        public void Verify_NuMedicsUserManager_Properties()
        {
            Assert.IsFalse(uManager.SupportsUserAuthenticationTokens);
            Assert.IsFalse(uManager.SupportsUserAuthenticatorKey);
            Assert.IsFalse(uManager.SupportsUserTwoFactor);
            Assert.IsFalse(uManager.SupportsUserTwoFactorRecoveryCodes);
            Assert.IsTrue(uManager.SupportsUserPassword);
            Assert.IsFalse(uManager.SupportsUserSecurityStamp);
            Assert.IsFalse(uManager.SupportsUserRole);
            Assert.IsFalse(uManager.SupportsUserLogin);
            Assert.IsTrue(uManager.SupportsUserEmail);
            Assert.IsFalse(uManager.SupportsUserPhoneNumber);
            Assert.IsTrue(uManager.SupportsUserClaim);
            Assert.IsFalse(uManager.SupportsQueryableUsers);
            Assert.IsTrue(uManager.SupportsUserLockout);
        }
    }
}
 