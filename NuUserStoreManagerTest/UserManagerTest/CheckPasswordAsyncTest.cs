using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuUserStoreManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuUserStoreManagerTest.UserManagerTest
{
    [TestClass]
    public class CheckPasswordAsyncTest : BaseUnitTest
    {
        [TestMethod]
        public void NuApplicationUser_Null()
        {
            var result = uManager.CheckPasswordAsync(null, pwd).Result;

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NuApplication_Username_Empty()
        {
            var result = uManager.CheckPasswordAsync(new NuApplicationUser(), pwd).Result;

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NuApplication_Password_Empty()
        {
            var result = uManager.CheckPasswordAsync(new NuApplicationUser(un), String.Empty).Result;

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Password_Match()
        {
            var user = testCtx.UserAuthentications.First(f => f.Username != un);
            user.Password = DataEncryption.Encrypt(pwd);
            testCtx.SaveChanges();

            var result = uManager.CheckPasswordAsync(new NuApplicationUser(user.Username), pwd).Result;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Password_Not_Match()
        {
            var bogusPassword = "bogus_123";

            var user = testCtx.UserAuthentications.First(f => f.Username != un);
            user.Password = DataEncryption.Encrypt(pwd);
            testCtx.SaveChanges();

            var result = uManager.CheckPasswordAsync(new NuApplicationUser(user.Username), bogusPassword).Result;

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void User_Not_Exist()
        {
            var result = uManager.CheckPasswordAsync(new NuApplicationUser(no_un), pwd).Result;

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Username_Duplicated()
        {
            var userFirst = testCtx.UserAuthentications.First();
            var userLast = testCtx.UserAuthentications.Last();

            userFirst.Username = un;
            userLast.Username = un;
            testCtx.SaveChanges();

            var result = uManager.CheckPasswordAsync(new NuApplicationUser(un), pwd).Result;

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Exception_Thrown()
        {
            var user = testCtx.UserAuthentications.First();
            uManager.ctx = null;

            var result = uManager.CheckPasswordAsync(new NuApplicationUser(user.Username), pwd).Result;

            Assert.IsFalse(result);
        }
    }
}
