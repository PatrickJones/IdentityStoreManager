﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuUserStoreManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuUserStoreManagerTest.UserManagerTest
{
    [TestClass]
    public class ChangeEmailAsyncTest : BaseUnitTest
    {
        [TestMethod]
        public void NuApplicationUser_Null()
        {
            var result = uManager.ChangeEmailAsync(null, em, tk).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.NuApplicationNull, result.Errors.First().Description);
        }

        [TestMethod]
        public void NuApplication_Username_Empty()
        {
            var result = uManager.ChangeEmailAsync(new NuApplicationUser(), em, tk).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.NuApplicationNull, result.Errors.First().Description);
        }

        [TestMethod]
        public void NuApplication_Email_Empty()
        {
            var result = uManager.ChangeEmailAsync(new NuApplicationUser(un), String.Empty, tk).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.EmptyString("Email"), result.Errors.First().Description);
        }

        [TestMethod]
        public void NuApplication_Email_Token_Empty()
        {
            var result = uManager.ChangeEmailAsync(new NuApplicationUser(un), em, String.Empty).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.EmptyString("Email token"), result.Errors.First().Description);
        }

        [TestMethod]
        public void Change_Email()
        {
            var user = testCtx.UserAuthentications.First(f => f.Username != un);

            var result = uManager.ChangeEmailAsync(new NuApplicationUser(user.Username), em, tk).Result;

            Assert.IsTrue(result.Succeeded);
        }

        [TestMethod]
        public void User_Not_Exist()
        {
            var result = uManager.ChangeEmailAsync(new NuApplicationUser(no_un), em, tk).Result;

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

            var result = uManager.ChangeEmailAsync(new NuApplicationUser(un), em, tk).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.DuplicateUsername(un), result.Errors.First().Description);
        }

        [TestMethod]
        public void Exception_Thrown()
        {
            var user = testCtx.UserAuthentications.First();
            uManager.ctx = null;

            var result = uManager.ChangeEmailAsync(new NuApplicationUser(user.Username), em, tk).Result;

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(result.Errors.Count(), 1);
            Assert.AreEqual(AppMessages.Error("ChangeEmailAsync", un), result.Errors.First().Description);
        }
    }
}
