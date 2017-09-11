using Bogus;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NuUserStoreManager;
using NuUserStoreManager.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuUserStoreManagerTest
{
    [TestClass]
    public abstract class BaseUnitTest
    {
        protected NuMedicsUserManager uManager;
        protected NuMedicsSignInManager sManager;
        protected NuMedicsGlobalContext testCtx;
        protected List<Users> FakeUsersCollection = new List<Users>();
        protected string DbName => "UserWithRelationships";

        internal readonly string em = "email@me.com";
        internal readonly string un = "MightyLion";
        internal readonly string no_un = "noUsername";
        internal readonly string tk = "my_token";
        internal readonly string pwd = "my_password";

        public BaseUnitTest()
        {
            // Set configuration to use in-memory database
            var builder = new DbContextOptionsBuilder<NuMedicsGlobalContext>().UseInMemoryDatabase(DbName);
            testCtx = new NuMedicsGlobalContext(builder.Options);

            InitContext();
        }

        protected void SetContextData()
        {
            // create fake data for clinician users
            var fClinicians = new Faker<Users>()
                .RuleFor(r => r.UserId, v => v.Random.Uuid())
                .RuleFor(r => r.UserType, 1)
                .RuleFor(r => r.CreationDate, v => v.Date.Past())
                .RuleForType<Clinicians>(typeof(Clinicians), r => new Clinicians
                {
                    Email = r.Person.Email,
                    Firstname = r.Person.FirstName,
                    Lastname = r.Person.LastName,
                    InstitutionId = r.Random.Uuid(),
                    LastUpdatedByUser = r.Random.Uuid(),
                    StateLicenseNumber = r.Random.Word()
                })
                .RuleForType<ICollection<UserAuthentications>>(typeof(ICollection<UserAuthentications>), v => new List<UserAuthentications> {
                    new UserAuthentications {
                        Username = v.Person.UserName,
                        Password = v.Internet.Password(),
                        PasswordFailureCount = v.Random.Int(0,5),
                        PasswordAnswerFailureCount = v.Random.Int(0,5),
                        LastActivityDate = v.Date.Past(),
                        LastLockOutDate = v.Date.Past(),
                        IsApproved = v.Random.Bool(),
                        IsLockedOut = v.Random.Bool(),
                        IsTempPassword = v.Random.Bool(),
                        IsloggedIn = v.Random.Bool(),
                        LastUpdatedByUser = v.Random.Uuid()
                    }
                });

            // create fake data for patient users
            var fPatients = new Faker<Users>()
                .RuleFor(r => r.UserId, v => v.Random.Uuid())
                .RuleFor(r => r.UserType, 2)
                .RuleFor(r => r.CreationDate, v => v.Date.Past())
                .RuleForType<Patients>(typeof(Patients), r => new Patients
                {
                    DateofBirth = r.Date.Past(),
                    Email = r.Person.Email,
                    Firstname = r.Person.FirstName,
                    Lastname = r.Person.LastName,
                    Gender = r.Random.Int(1, 2),
                    PlanId = 0,
                    InstitutionId = r.Random.Uuid(),
                    LastUpdatedByUser = r.Random.Uuid()
                })
                .RuleForType<ICollection<UserAuthentications>>(typeof(ICollection<UserAuthentications>), v => new List<UserAuthentications> {
                    new UserAuthentications {
                        Username = v.Person.UserName,
                        Password = v.Internet.Password(),
                        PasswordFailureCount = v.Random.Int(0,5),
                        PasswordAnswerFailureCount = v.Random.Int(0,5),
                        LastActivityDate = v.Date.Past(),
                        LastLockOutDate = v.Date.Past(),
                        IsApproved = v.Random.Bool(),
                        IsLockedOut = v.Random.Bool(),
                        IsTempPassword = v.Random.Bool(),
                        IsloggedIn = v.Random.Bool(),
                        LastUpdatedByUser = v.Random.Uuid()
                    }
                });

            var fcSet = fClinicians.Generate(3).ToList();
            var fpSet = fPatients.Generate(3).ToList();

            // Insert fake objects into member collection to compare against fake db context
            FakeUsersCollection.AddRange(fcSet);
            FakeUsersCollection.AddRange(fpSet);

            // Insert fake odjects into testing db context
            testCtx.Users.AddRange(fcSet);
            testCtx.Users.AddRange(fpSet);
            testCtx.SaveChanges();

        }

        private void InitContext()
        {
            // mock interfaces for UserManager
            Mock<IUserStore<NuApplicationUser>> mUserStore = new Mock<IUserStore<NuApplicationUser>>();
            Mock<IOptions<IdentityOptions>> mOptions = new Mock<IOptions<IdentityOptions>>();
            Mock<IPasswordHasher<NuApplicationUser>> mPasswordHasher = new Mock<IPasswordHasher<NuApplicationUser>>();
            Mock<IUserValidator<NuApplicationUser>> mUserValidator = new Mock<IUserValidator<NuApplicationUser>>();
            Mock<IPasswordValidator<NuApplicationUser>> mPasswordValidator = new Mock<IPasswordValidator<NuApplicationUser>>();
            Mock<ILookupNormalizer> mLookupNormalizer = new Mock<ILookupNormalizer>();
            Mock<IServiceProvider> mServices = new Mock<IServiceProvider>();
            Mock<ILogger<NuMedicsUserManager>> mLogger = new Mock<ILogger<NuMedicsUserManager>>();

            // mock interfaces for SignInManager
            Mock<IHttpContextAccessor> sHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUserClaimsPrincipalFactory<NuApplicationUser>> sUserClaimsPrincipleFactory = new Mock<IUserClaimsPrincipalFactory<NuApplicationUser>>();
            Mock<IOptions<IdentityOptions>> sOptions = new Mock<IOptions<IdentityOptions>>();
            Mock<ILogger<NuMedicsSignInManager>> sLogger = new Mock<ILogger<NuMedicsSignInManager>>();
            Mock<IAuthenticationSchemeProvider> sAuthenticationSchemeProvider = new Mock<IAuthenticationSchemeProvider>();


            this.uManager = new NuMedicsUserManager(mUserStore.Object,
                mOptions.Object, 
                mPasswordHasher.Object, 
                new List<IUserValidator<NuApplicationUser>> { mUserValidator.Object },
                new List<IPasswordValidator<NuApplicationUser>> { mPasswordValidator.Object },
                mLookupNormalizer.Object, 
                new IdentityErrorDescriber(), 
                mServices.Object, 
                mLogger.Object, 
                testCtx);

            this.sManager = new NuMedicsSignInManager(this.uManager,
                sHttpContextAccessor.Object,
                sUserClaimsPrincipleFactory.Object,
                sOptions.Object, sLogger.Object,
                sAuthenticationSchemeProvider.Object,
                testCtx);

            
            SetContextData();
        }

        //[TestMethod]
        public void VerifyCollection()
        {
            var fakeApp = FakeUsersCollection;
            var pat = FakeUsersCollection.First(f => f.UserType == 2);
            var cli = FakeUsersCollection.First(f => f.UserType == 1);


            Assert.AreEqual(fakeApp.Count, 6);
            Assert.AreEqual(pat.UserId, pat.Patients.UserId);
            Assert.AreEqual(cli.UserId, cli.Clinicians.UserId);
        }

        //[TestMethod]
        public void VerifyDbContext()
        {
            var pat = testCtx.Users
                .Include(a => a.UserAuthentications)
                .Include(p => p.Patients)
                .Include(c => c.Clinicians)
                .First(f => f.UserType == 2);

            var cli = testCtx.Users
                .Include(a => a.UserAuthentications)
                .Include(p => p.Patients)
                .Include(c => c.Clinicians)
                .First(f => f.UserType == 1);
            
            // verify entities were added to in-memory database
            Assert.AreEqual(testCtx.Users.Count(), 6);
            Assert.AreEqual(testCtx.UserAuthentications.Count(), 6);
            Assert.AreEqual(testCtx.Patients.Count(), 3);
            Assert.AreEqual(testCtx.Clinicians.Count(), 3);

            // verify patient relationships were created correctly
            Assert.IsNotNull(pat.Patients);
            Assert.IsNotNull(pat.UserAuthentications);
            Assert.IsNull(pat.Clinicians);
            Assert.AreEqual(pat.UserId, pat.Patients.UserId);
            Assert.AreEqual(pat.UserId, pat.UserAuthentications.SingleOrDefault().UserId);
            Assert.IsFalse(String.IsNullOrEmpty(pat.UserAuthentications.SingleOrDefault().Username));

            // verify clinician relationships were created correctly
            Assert.IsNotNull(cli.Clinicians);
            Assert.IsNotNull(cli.UserAuthentications);
            Assert.IsNull(cli.Patients);
            Assert.AreEqual(cli.UserId, cli.Clinicians.UserId);
            Assert.AreEqual(cli.UserId, cli.UserAuthentications.SingleOrDefault().UserId);
            Assert.IsFalse(String.IsNullOrEmpty(cli.UserAuthentications.SingleOrDefault().Username));
        }
    }
}
