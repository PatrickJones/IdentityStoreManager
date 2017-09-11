using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuUserStoreManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuUserStoreManagerTest.UserManagerTest
{
    [TestClass]
    public class AddPasswordAsyncTest :  BaseUnitTest
    {
        [TestMethod]
        public void NuApplicationUser_Null()
        {
            var result = uManager.AddPasswordAsync(null, pwd).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.NuApplicationNull, result.Errors.First().Description);
        }

        [TestMethod]
        public void NuApplication_Username_Empty()
        {
            var result = uManager.AddPasswordAsync(new NuApplicationUser(), pwd).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.NuApplicationNull, result.Errors.First().Description);
        }

        [TestMethod]
        public void NuApplication_Password_Empty()
        {
            var result = uManager.AddPasswordAsync(new NuApplicationUser(un), String.Empty).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.EmptyString("Password"), result.Errors.First().Description);
        }

        [TestMethod]
        public void Insert_Password()
        {
            var user = testCtx.UserAuthentications.First(f => f.Username != un);

            var result = uManager.AddPasswordAsync(new NuApplicationUser(user.Username), pwd).Result;

            Assert.IsTrue(result.Succeeded);
        }

        [TestMethod]
        public void User_Not_Exist()
        {
            var result = uManager.AddPasswordAsync(new NuApplicationUser(no_un), pwd).Result;

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

            var result = uManager.AddPasswordAsync(new NuApplicationUser(un), pwd).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.DuplicateUsername(un), result.Errors.First().Description);
        }

        [TestMethod]
        public void Exception_Thrown()
        {
            var user = testCtx.UserAuthentications.First();
            uManager.ctx = null;

            var result = uManager.AddPasswordAsync(new NuApplicationUser(user.Username), pwd).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.Error("AddPasswordAsync", un), result.Errors.First().Description);
        }
    }
}
