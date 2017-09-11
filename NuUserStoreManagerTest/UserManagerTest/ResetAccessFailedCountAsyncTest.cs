using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuUserStoreManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuUserStoreManagerTest.UserManagerTest
{
    [TestClass]
    public class ResetAccessFailedCountAsyncTest : BaseUnitTest
    {
        [TestMethod]
        public void NuApplicationUser_Null()
        {
            var result = uManager.ResetAccessFailedCountAsync(null).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.NuApplicationNull, result.Errors.First().Description);
        }

        [TestMethod]
        public void NuApplication_Username_Empty()
        {
            var result = uManager.ResetAccessFailedCountAsync(new NuApplicationUser()).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.NuApplicationNull, result.Errors.First().Description);
        }

        [TestMethod]
        public void Reset_Password_Fail_Count()
        {
            var user = testCtx.UserAuthentications.First(f => f.Username != un);
            var uName = user.Username;
            user.PasswordFailureCount = 1; // reset to at least 1 for this test
            testCtx.SaveChanges();

            var result = uManager.ResetAccessFailedCountAsync(new NuApplicationUser(uName)).Result;

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(testCtx.UserAuthentications.First(f => f.Username == uName).PasswordFailureCount, 0);
        }

        [TestMethod]
        public void User_Not_Exist()
        {
            var result = uManager.ResetAccessFailedCountAsync(new NuApplicationUser(no_un)).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.UserNotFound(no_un), result.Errors.First().Description);
        }

        [TestMethod]
        public void Username_Duplicated()
        {
            var userFirst = testCtx.UserAuthentications.First();
            var userLast = testCtx.UserAuthentications.Last();

            userFirst.Username = un;
            userLast.Username = un;
            testCtx.SaveChanges();

            var result = uManager.ResetAccessFailedCountAsync(new NuApplicationUser(un)).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.DuplicateUsername(un), result.Errors.First().Description);
        }

        [TestMethod]
        public void Exception_Thrown()
        {
            var user = testCtx.UserAuthentications.First();
            uManager.ctx = null;

            var result = uManager.ResetAccessFailedCountAsync(new NuApplicationUser(user.Username)).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.Error("ResetAccessFailedCountAsync", un), result.Errors.First().Description);
        }

    }
}
