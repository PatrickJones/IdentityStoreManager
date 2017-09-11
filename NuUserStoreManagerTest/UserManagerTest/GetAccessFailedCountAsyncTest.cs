using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuUserStoreManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuUserStoreManagerTest.UserManagerTest
{
    [TestClass]
    public class GetAccessFailedCountAsyncTest : BaseUnitTest
    {
        [TestMethod]
        public void NuApplicationUser_Null()
        {
            var result = uManager.GetAccessFailedCountAsync(null).Result;

            Assert.AreEqual(result, 10);
        }

        [TestMethod]
        public void NuApplication_Username_Empty()
        {
            var result = uManager.GetAccessFailedCountAsync(new NuApplicationUser()).Result;

            Assert.AreEqual(result, 10);
        }

        [TestMethod]
        public void Get_Password_Fail_Count()
        {
            var user = testCtx.UserAuthentications.First(f => f.Username != un);
            var failCount = user.PasswordFailureCount;

            var result = uManager.GetAccessFailedCountAsync(new NuApplicationUser(user.Username)).Result;

            Assert.AreEqual(failCount, result);
        }

        [TestMethod]
        public void User_Not_Exist()
        {
            var result = uManager.GetAccessFailedCountAsync(new NuApplicationUser(un)).Result;

            Assert.AreEqual(result, 10);
        }

        [TestMethod]
        public void Username_Dupliucated()
        {
            var userFirst = testCtx.UserAuthentications.First();
            var userLast = testCtx.UserAuthentications.Last();

            userFirst.Username = un;
            userLast.Username = un;
            testCtx.SaveChanges();

            var result = uManager.GetAccessFailedCountAsync(new NuApplicationUser(un)).Result;

            Assert.AreEqual(result, 10);
        }

        [TestMethod]
        public void Exception_Thrown()
        {
            var user = testCtx.UserAuthentications.First();
            uManager.ctx = null;

            var result = uManager.GetAccessFailedCountAsync(new NuApplicationUser(user.Username)).Result;

            Assert.AreEqual(result, 10);
        }
    }
}
