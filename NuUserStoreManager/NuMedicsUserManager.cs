using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NuUserStoreManager.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace NuUserStoreManager
{
    public class NuMedicsUserManager : UserManager<NuApplicationUser>
    {
        public NuMedicsUserManager(IUserStore<NuApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<NuApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<NuApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<NuApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<NuApplicationUser>> logger,
            NuMedicsGlobalContext dbContext) : base(store,optionsAccessor,passwordHasher,userValidators,passwordValidators,keyNormalizer,errors,services,logger)
        {
            ctx = dbContext;
        }

        public NuMedicsGlobalContext ctx;

        public override bool SupportsUserAuthenticationTokens => false;
        public override bool SupportsUserAuthenticatorKey => false;
        public override bool SupportsUserTwoFactor => false;
        public override bool SupportsUserTwoFactorRecoveryCodes => false;
        public override bool SupportsUserPassword => true;
        public override bool SupportsUserSecurityStamp => false;
        public override bool SupportsUserRole => false;
        public override bool SupportsUserLogin => false;
        public override bool SupportsUserEmail => true;
        public override bool SupportsUserPhoneNumber => false;
        public override bool SupportsUserClaim => true;
        public override bool SupportsQueryableUsers => false; 
        public override bool SupportsUserLockout => true;
        public override IQueryable<NuApplicationUser> Users => base.Users;

        public override Task<IdentityResult> AccessFailedAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew<IdentityResult>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return IdentityResult.Failed(new IdentityError { Description = AppMessages.NuApplicationNull });
                    }

                    Logger.LogInformation($"Attempting to set access fail count for user {user.UserName}");

                    int success = 0;

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    if (u != default(UserAuthentications))
                    {
                        u.PasswordFailureCount++;
                        ctx.SaveChanges();
                        success = 1;
                    }
                    else
                    {
                        success = -1;
                    }

                    switch (success)
                    {
                        case -1:
                            Logger.LogInformation($"User does not does not exist in database. Username: {user.UserName}");
                            return IdentityResult.Failed(new IdentityError { Description = AppMessages.UserNotFound(user.UserName) });
                        case 1:
                        default:
                            Logger.LogInformation($"Incrementing failed access count for Username: {user.UserName}");
                            return IdentityResult.Success;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Username is duplicated in database for user {user.UserName}");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.DuplicateUsername(user.UserName) });
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error incrementing access fail count for user {user.UserName}");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.Error("AccessFailedAsync", user.UserName) });
                }
            });
        }

        public override Task<int> GetAccessFailedCountAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew<int>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return 10; // user does not exist in database, so return high value to lock user out.
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    if (u != null)
                    {
                        Logger.LogInformation($"Returning failed access count: {u.PasswordFailureCount}");
                        return u.PasswordFailureCount;
                    }
                    else
                    {
                        Logger.LogInformation($"User does not does not exist in database. Username: {user.UserName}");
                        return 10; // user does not exist in database, so return high value to lock user out.
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return 10; // user does not exist in database, so return high value to lock user out.
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "Error reseting access failed count.");
                    return 10; // user does not exist in database, so return high value to lock user out.
                }
            });
        }

        public override Task<IdentityResult> ResetAccessFailedCountAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew<IdentityResult>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return IdentityResult.Failed(new IdentityError { Description = AppMessages.NuApplicationNull });
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    int success = 0;

                    if (u != null)
                    {
                        u.PasswordFailureCount = 0;
                        ctx.SaveChanges();
                        success = 1;
                    }
                    else
                    {
                        success = -1;
                    }

                    switch (success)
                    {
                        case -1:
                            Logger.LogInformation($"Access reset failed for Username: {user.UserName}. Username not found");
                            return IdentityResult.Failed(new IdentityError { Description = AppMessages.UserNotFound(user.UserName) });
                        case 1:
                        default:
                            Logger.LogInformation($"Reseting access failed for Username: {user.UserName}");
                            return IdentityResult.Success;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.DuplicateUsername(user.UserName) });
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error incrementing access fail count for user {user.UserName}");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.Error("ResetAccessFailedCountAsync", user.UserName) });
                }
            });
        }

        public override Task<IdentityResult> AddClaimAsync(NuApplicationUser user, Claim claim)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("AddClaimAsync invoked. Adding Claim to database is not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("AddClaimAsync") });
            });
        }

        public override Task<IdentityResult> AddClaimsAsync(NuApplicationUser user, IEnumerable<Claim> claims)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("AddClaimAsync invoked. Adding Claims to database is not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("AddClaimAsync") });
            });
        }

        public override Task<IdentityResult> AddPasswordAsync(NuApplicationUser user, string password)
        {
            return Task.Factory.StartNew<IdentityResult>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return IdentityResult.Failed(new IdentityError { Description = AppMessages.NuApplicationNull });
                    }

                    if (String.IsNullOrEmpty(password))
                    {
                        Logger.LogError($"Password is empty string for Username:  {user.UserName}.");
                        return IdentityResult.Failed(new IdentityError { Description = AppMessages.EmptyString("Password") });
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    int success = 0;

                    if (u != null)
                    {
                        u.Password = password;
                        ctx.SaveChanges();
                        success = 1;
                    }
                    else
                    {
                        success = -1;
                    }

                    switch (success) 
                    {
                        case -1:
                            Logger.LogInformation($"Password insert failed for Username: {user.UserName}. Username not found");
                            return IdentityResult.Failed(new IdentityError { Description = AppMessages.UserNotFound(user.UserName) });
                        case 1:
                        default:
                            Logger.LogInformation($"Inserted new  password for Username: {user.UserName}");
                            return IdentityResult.Success;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.DuplicateUsername(user.UserName) });
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error inserting password for Usename: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.Error("AddPasswordAsync", user.UserName) });
                }
            });
        }

        public override Task<IdentityResult> ChangeEmailAsync(NuApplicationUser user, string newEmail, string token)
        {
            return Task.Factory.StartNew<IdentityResult>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return IdentityResult.Failed(new IdentityError { Description = AppMessages.NuApplicationNull });
                    }

                    if (String.IsNullOrEmpty(newEmail))
                    {
                        Logger.LogError($"Email is empty string for Username:  {user.UserName}.");
                        return IdentityResult.Failed(new IdentityError { Description = AppMessages.EmptyString("Email") });
                    }

                    if (String.IsNullOrEmpty(token))
                    {
                        Logger.LogError($"Email tooken is empty string for Username:  {user.UserName}.");
                        return IdentityResult.Failed(new IdentityError { Description = AppMessages.EmptyString("Email token") });
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    int success = 0;

                    if (u != null)
                    {
                        var usr = ctx.Users.Include(i => i.Patients).Include(c => c.Clinicians).SingleOrDefault(w => w.UserId == u.UserId);
                        if (usr != null)
                        {
                            switch (usr.UserType)
                            {
                                case 1:
                                    usr.Clinicians.Email = newEmail;
                                    break;
                                case 2:
                                    usr.Patients.Email = newEmail;
                                    break;
                                case 3: // user is admin - do nothing
                                default:
                                    break;
                            }

                            ctx.SaveChanges();
                            success = 1;
                        }
                    }
                    else
                    {
                        success = -1;
                    }

                    switch (success)
                    {
                        case -1:
                            Logger.LogInformation($"Email insert failed for Username: {user.UserName}. Username not found");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.UserNotFound(user.UserName) });
                        case 1:
                        default:
                            Logger.LogInformation($"Inserted new email for Username: {user.UserName}");
                            return IdentityResult.Success;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.DuplicateUsername(user.UserName) });
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error changing email for Usename: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.Error("ChangeEmailAsync", user.UserName) });
                }
            });
        }

        public override Task<bool> CheckPasswordAsync(NuApplicationUser user, string password)
        {
            return Task.Factory.StartNew<bool>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return false;
                    }

                    if (String.IsNullOrEmpty(password))
                    {
                        Logger.LogError($"Password is empty string for Username:  {user.UserName}.");
                        return false;
                    }

                    if (!String.IsNullOrEmpty(user.UserName))
                    {
                        var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                        int success = 0;

                        if (u != null)
                        {
                            //TODO: a decrypt logic
                            success = (u.Password == password) ? 1 : 0;
                        }
                        else
                        {
                            Logger.LogInformation($"Password check failed for Username: {user.UserName}. Username not found");
                            return false;
                        }

                        switch (success)
                        {
                            case 0:
                                Logger.LogWarning($"Password check failed for Username: {user.UserName}.");
                                return false;
                            case 1:
                            default:
                                Logger.LogInformation($"Password check valid for Username: {user.UserName}");
                                return true;
                        }
                    }

                    Logger.LogWarning($"Password check failed for Username: {user.UserName}. Username not set on NuApplicationUser.");
                    return false;
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return false;
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error validating password for Usename: {user.UserName}.");
                    return false;
                }
            });
        }

        public override Task<NuApplicationUser> FindByEmailAsync(string email)
        {
            return Task.Factory.StartNew<NuApplicationUser>(() => {
                try
                {
                    if (String.IsNullOrEmpty(email))
                    {
                        Logger.LogError($"Cannot find user. Email string is empty.");
                        return new NuApplicationUser();
                    }
                    else
                    {
                        var tempUser = new List<NuApplicationUser>();

                        var pq = from p in ctx.Patients
                                join u in ctx.UserAuthentications on p.UserId equals u.UserId
                                where p.Email == email
                                select new NuApplicationUser(u.Username);
                        tempUser.AddRange(pq);

                        var cq = from c in ctx.Clinicians
                                join u in ctx.UserAuthentications on c.UserId equals u.UserId
                                where c.Email == email
                                select new NuApplicationUser(u.Username);
                        tempUser.AddRange(cq);

                        switch (tempUser.Count)
                        {
                            case 0:
                                Logger.LogWarning($"Cannot find user based on email: {email}.");
                                return new NuApplicationUser();
                            case 1:
                            default:
                                var user = tempUser.SingleOrDefault(f => f.Email == email);
                                Logger.LogInformation($"Found user based email: {email} - Username: {user.UserName}");
                                return user;
                        }
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate emails in database for email: {email}.");
                    return new NuApplicationUser();
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error finding user for email: {email}.");
                    return new NuApplicationUser();
                }
            });
        }

        public override Task<NuApplicationUser> FindByIdAsync(string userId)
        {
            return Task.Factory.StartNew<NuApplicationUser>(() => {
                try
                {
                    bool parsed = Guid.TryParse(userId, out Guid uGuid);

                    if (parsed)
                    {
                        var usr = ctx.UserAuthentications.SingleOrDefault(w => w.UserId == uGuid);
                        if (usr != null)
                        {
                            return new NuApplicationUser(usr.Username);
                        }
                        else
                        {
                            Logger.LogError($"Cannot find user. UserId does not exist.");
                            return new NuApplicationUser();
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"Uable to parse userid: {userId}");
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate user id's in database for user id: {userId}.");
                    return new NuApplicationUser();
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error finding user for user id: {userId}.");
                    return new NuApplicationUser();
                }
            });
        }

        public override Task<NuApplicationUser> FindByNameAsync(string userName)
        {
            return Task.Factory.StartNew<NuApplicationUser>(() => {
                try
                {
                    if (String.IsNullOrEmpty(userName))
                    {
                        Logger.LogError($"Cannot find user. Username does not exist.");
                        return new NuApplicationUser();
                    }

                    var usr = ctx.UserAuthentications.SingleOrDefault(w => w.Username == userName);
                    if (usr != null)
                    {
                        return new NuApplicationUser(usr.Username);
                    }
                    else
                    {
                        Logger.LogError($"Cannot find user. Username does not exist: {userName}.");
                        return new NuApplicationUser();
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database for username: {userName}.");
                    return new NuApplicationUser();
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error finding user for username: {userName}.");
                    return new NuApplicationUser();
                }
            });
        }

        public override Task<DateTimeOffset?> GetLockoutEndDateAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew<DateTimeOffset?>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    if (u != null)
                    {
                        return u.LastLockOutDate;
                    }
                    else
                    {
                        throw new ArgumentNullException("User not found in database.");
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error adding password for Usename: {user.UserName}.");
                    return DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
                }
            });
        }

        public override Task<bool> GetLockoutEnabledAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew<bool>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return false;
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    if (u != null)
                    {
                        return u.IsLockedOut;
                    }
                    else
                    {
                        throw new ArgumentNullException("User not found in database.");
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return false;
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error adding password for Usename: {user.UserName}.");
                    return false;
                }
            });
        }

        public override Task<string> GetEmailAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew<string>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return String.Empty;
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    if (u != null)
                    {
                        var usr = ctx.Users.Include(i => i.Patients).Include(c => c.Clinicians).SingleOrDefault(w => w.UserId == u.UserId);
                        if (usr != null)
                        {
                            switch (usr.UserType)
                            {
                                case 1:
                                    return usr.Clinicians.Email;
                                case 2:
                                    return usr.Patients.Email;
                                case 3: // user is admin - do nothing
                                default:
                                    return String.Empty;
                            }
                        }
                    }

                    Logger.LogWarning($"Get email failed for Username: {user.UserName}. Username not set on NuApplicationUser.");
                    return String.Empty;
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return String.Empty;
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error adding password for Usename: {user.UserName}.");
                    return String.Empty;
                }
            });
        }

        public override Task<IList<Claim>> GetClaimsAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetClaimsAsync invoked. Claim stored in database is not supported.");
                return new List<Claim>() as IList<Claim>;
            });
        }

        public override Task<NuApplicationUser> GetUserAsync(ClaimsPrincipal principal)
        {
            return Task.Factory.StartNew<NuApplicationUser>(() => {

                var claimName = String.Empty;

                try
                {
                    if (principal == null)
                    {
                        Logger.LogError("ClaimsPrinciple is null.");
                        return new NuApplicationUser();
                    }

                    claimName = principal.FindFirstValue(ClaimTypes.Name);

                    if (String.IsNullOrEmpty(claimName))
                    {
                        Logger.LogError("ClaimTypes.Name is null.");
                        return new NuApplicationUser();
                    }

                    var userExist = ctx.UserAuthentications.Any(a => a.Username == claimName);

                    if (userExist)
                    {
                        return new NuApplicationUser(claimName);
                    }
                    else
                    {
                        Logger.LogError($"User not found mathcing Claim: {claimName}.");
                        return new NuApplicationUser();
                    }

                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error finding user for Claim: {claimName}.");
                    return new NuApplicationUser();
                }
            });

        }

        public override string GetUserId(ClaimsPrincipal principal)
        {
            var claimName = String.Empty;

            try
            {
                if (principal == null)
                {
                    Logger.LogError("ClaimsPrinciple is null.");
                    return Guid.Empty.ToString();
                }

                claimName = principal.FindFirstValue(ClaimTypes.Name);

                if (String.IsNullOrEmpty(claimName))
                {
                    Logger.LogError("ClaimTypes.Name is null.");
                    return Guid.Empty.ToString();
                }

                var user = ctx.UserAuthentications.SingleOrDefault(a => a.Username == claimName);

                if (user != null)
                {
                    return user.UserId.ToString();
                }
                else
                {
                    Logger.LogError($"User not found mathcing Claim: {claimName}.");
                    return Guid.Empty.ToString();
                }

            }
            catch (InvalidOperationException e)
            {
                Logger.LogError(e, $"Duplicate username in database for Claim: {claimName}.");
                return Guid.Empty.ToString();
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"Error finding user for Claim: {claimName}.");
                return Guid.Empty.ToString();
            }
        }

        public override string GetUserName(ClaimsPrincipal principal)
        {
            var claimName = String.Empty;

            try
            {
                if (principal == null)
                {
                    Logger.LogError("ClaimsPrinciple is null.");
                    return String.Empty;
                }

                claimName = principal.FindFirstValue(ClaimTypes.Name);

                if (String.IsNullOrEmpty(claimName))
                {
                    Logger.LogError("ClaimTypes.Name is null.");
                    return String.Empty;
                }

                var user = ctx.UserAuthentications.SingleOrDefault(a => a.Username == claimName);

                if (user != null)
                {
                    return user.Username;
                }
                else
                {
                    Logger.LogError($"User not found mathcing Claim: {claimName}.");
                    return String.Empty;
                }
            }
            catch (InvalidOperationException e)
            {
                Logger.LogError(e, $"Duplicate username in database for Claim: {claimName}.");
                return String.Empty;
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"Error finding user for Claim: {claimName}.");
                return String.Empty;
            }
        }

        public override Task<IList<NuApplicationUser>> GetUsersForClaimAsync(Claim claim)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetUsersForClaimAsync invoked. Claim stored in database is not supported.");
                return new List<NuApplicationUser>() as IList<NuApplicationUser>;
            });
        }

        public override Task<IdentityResult> RemoveClaimAsync(NuApplicationUser user, Claim claim)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetClaimsAsync invoked. Claim stored in database is not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("RemoveClaimAsync") });
            });
        }

        public override Task<IdentityResult> RemoveClaimsAsync(NuApplicationUser user, IEnumerable<Claim> claims)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetClaimsAsync invoked. Claims stored in database is not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("RemoveClaimsAsync") });
            });
        }

        public override Task<IdentityResult> ReplaceClaimAsync(NuApplicationUser user, Claim claim, Claim newClaim)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetClaimsAsync invoked. Claims stored in database is not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("ReplaceClaimAsync") });
            });
        }

        public override Task<string> GetUserNameAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew<string>(() => {
                try
                {
                    if (user == null || user.UserId.Equals(Guid.Empty))
                    {
                        Logger.LogError("NuApplicationUser is null or UserId is empty.");
                        return String.Empty;
                    }

                    if (!String.IsNullOrEmpty(user.UserName))
                    {
                        return user.UserName;
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.UserId == user.UserId);

                    if (u != null)
                    {
                        return u.Username;
                    }
                    else
                    {
                        throw new ArgumentNullException("User not found in database.");
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate user id in database: {user.UserId}.");
                    return String.Empty;
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error getting username for user id: {user.UserId}.");
                    return String.Empty;
                }
            });
        }

        public override Task<string> GetUserIdAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew<string>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return String.Empty;
                    }

                    if (!String.IsNullOrEmpty(user.Id))
                    {
                        return user.Id;
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    if (u != null)
                    {
                        return u.UserId.ToString();
                    }
                    else
                    {
                        throw new ArgumentNullException("User not found in database.");
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return String.Empty;
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error getting user id for Usename: {user.UserName}.");
                    return String.Empty;
                }
            });
        }

        public override Task<bool> HasPasswordAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew<bool>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return false;
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    if (u != null)
                    {
                        return (String.IsNullOrEmpty(u.Password)) ? false : true;
                    }
                    else
                    {
                        throw new ArgumentNullException("User not found in database.");
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return false;
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error determining password for Usename: {user.UserName}.");
                    return false;
                }
            });
        }

        public override Task<bool> IsLockedOutAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew<bool>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return false;
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    if (u != null)
                    {
                        return u.IsLockedOut;
                    }
                    else
                    {
                        throw new ArgumentNullException("User not found in database.");
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return false;
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error determining lockout for Usename: {user.UserName}.");
                    return false;
                }
            });
        }

        public override Task<IdentityResult> RemovePasswordAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew<IdentityResult>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return IdentityResult.Failed(new IdentityError { Description = AppMessages.NuApplicationNull });
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    int success = 0;

                    if (u != null)
                    {
                        u.Password = String.Empty;
                        ctx.SaveChanges();
                        success = 1;
                    }
                    else
                    {
                        success = -1;
                    }

                    switch (success)
                    {
                        case -1:
                            Logger.LogInformation($"Password removal failed for Username: {user.UserName}. Username not found");
                            return IdentityResult.Failed(new IdentityError { Description = AppMessages.UserNotFound(user.UserName) });
                        case 1:
                        default:
                            Logger.LogInformation($"Removed password for Username: {user.UserName}");
                            return IdentityResult.Success;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.DuplicateUsername(user.UserName) });
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error removing password for Usename: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.Error("RemovePasswordAsync", user.UserName) });
                }
            });
        }

        public override Task<IdentityResult> ResetPasswordAsync(NuApplicationUser user, string token, string newPassword)
        {
            return Task.Factory.StartNew<IdentityResult>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return IdentityResult.Failed(new IdentityError { Description = AppMessages.NuApplicationNull });
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    int success = 0;

                    if (u != null)
                    {
                        u.Password = newPassword;
                        ctx.SaveChanges();
                        success = 1;
                    }
                    else
                    {
                        success = -1;
                    }

                    switch (success)
                    {
                        case -1:
                            Logger.LogInformation($"Password reset failed for Username: {user.UserName}. Username not found");
                            return IdentityResult.Failed(new IdentityError { Description = AppMessages.UserNotFound(user.UserName) });
                        case 1:
                        default:
                            Logger.LogInformation($"Reset password for Username: {user.UserName}");
                            return IdentityResult.Success;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.DuplicateUsername(user.UserName) });
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error resetting password for Usename: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.Error("ResetPasswordAsync", user.UserName) });
                }
            });
        }

        public override Task<IdentityResult> SetEmailAsync(NuApplicationUser user, string email)
        {
            return Task.Factory.StartNew<IdentityResult>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return IdentityResult.Failed(new IdentityError { Description = AppMessages.NuApplicationNull });
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    int success = 0;

                    if (u != null)
                    {
                        var usr = ctx.Users.Include(i => i.Patients).Include(c => c.Clinicians).SingleOrDefault(w => w.UserId == u.UserId);
                        if (usr != null)
                        {
                            switch (usr.UserType)
                            {
                                case 1:
                                    usr.Clinicians.Email = email;
                                    break;
                                case 2:
                                    usr.Patients.Email = email;
                                    break;
                                case 3: // user is admin - do nothing
                                default:
                                    break;
                            }

                            ctx.SaveChanges();
                            success = 1;
                        }
                    }
                    else
                    {
                        success = -1;
                    }

                    switch (success)
                    {
                        case -1:
                            Logger.LogInformation($"Email reset failed for Username: {user.UserName}. Username not found");
                            return IdentityResult.Failed(new IdentityError { Description = AppMessages.DuplicateUsername(user.UserName) });
                        case 1:
                        default:
                            Logger.LogInformation($"Reset email for Username: {user.UserName}");
                            return IdentityResult.Success;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.DuplicateUsername(user.UserName) });
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error resetting email for Usename: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.Error("SetEmailAsync", user.UserName) });
                }
            });
        }

        public override Task<IdentityResult> SetLockoutEnabledAsync(NuApplicationUser user, bool enabled)
        {
            return Task.Factory.StartNew<IdentityResult>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return IdentityResult.Failed(new IdentityError { Description = AppMessages.NuApplicationNull });
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    int success = 0;

                    if (u != null)
                    {
                        u.IsLockedOut = enabled;
                        ctx.SaveChanges();
                        success = 1;
                    }
                    else
                    {
                        success = -1;
                    }

                    switch (success)
                    {
                        case -1:
                            Logger.LogInformation($"Setting lockout status failed for Username: {user.UserName}. Username not found");
                            return IdentityResult.Failed(new IdentityError { Description = AppMessages.UserNotFound(user.UserName) });
                        case 1:
                        default:
                            Logger.LogInformation($"Setting lockout status for Username: {user.UserName}");
                            return IdentityResult.Success;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.DuplicateUsername(user.UserName) });
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error setting lockout status for Usename: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.Error("SetLockoutEnabledAsync", user.UserName) });
                }
            });
        }

        public override Task<IdentityResult> SetLockoutEndDateAsync(NuApplicationUser user, DateTimeOffset? lockoutEnd)
        {
            return Task.Factory.StartNew<IdentityResult>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return IdentityResult.Failed(new IdentityError { Description = AppMessages.NuApplicationNull });
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    int success = 0;

                    if (u != null)
                    {
                        u.LastLockOutDate = (lockoutEnd.HasValue) ? lockoutEnd.Value.DateTime : u.LastLockOutDate;
                        ctx.SaveChanges();
                        success = 1;
                    }
                    else
                    {
                        success = -1;
                    }

                    switch (success)
                    {
                        case -1:
                            Logger.LogInformation($"Setting lockout date failed for Username: {user.UserName}. Username not found");
                            return IdentityResult.Failed(new IdentityError { Description = AppMessages.UserNotFound(user.UserName) });
                        case 1:
                        default:
                            Logger.LogInformation($"Setting lockout date for Username: {user.UserName}");
                            return IdentityResult.Success;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.DuplicateUsername(user.UserName) });
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error setting lockout status for Usename: {user.UserName}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.Error("SetLockoutEndDateAsync", user.UserName) });
                }
            });
        }

        public override Task<IdentityResult> SetUserNameAsync(NuApplicationUser user, string userName)
        {
            return Task.Factory.StartNew<IdentityResult>(() => {
                try
                {
                    if (user == null || user.UserId.Equals(Guid.Empty))
                    {
                        Logger.LogError("NuApplicationUser is null or User Id is not set.");
                        return IdentityResult.Failed(new IdentityError { Description = AppMessages.NuApplicationNull });
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.UserId == user.UserId);

                    int success = 0;

                    if (u != null)
                    {
                        u.Username = userName;
                        success = ctx.SaveChanges();
                    }
                    else
                    {
                        Logger.LogInformation($"Setting username failed for UserId: {user.UserId}. UserId not found");
                        return IdentityResult.Failed(new IdentityError { Description = "User not found." });
                    }

                    switch (success)
                    {
                        case 0:
                            Logger.LogInformation($"Setting username failed for UserId: {user.UserId}. UserId not found");
                            return IdentityResult.Failed(new IdentityError { Description = AppMessages.UserNotFound(user.UserId.ToString()) });
                        case 1:
                        default:
                            Logger.LogInformation($"Setting username for UserId: {user.UserId}");
                            return IdentityResult.Success;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate user id in database: {user.UserId}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.DuplicateUsername(user.UserId.ToString()) });
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error username for UserId: {user.UserId}.");
                    return IdentityResult.Failed(new IdentityError { Description = AppMessages.Error("SetUserNameAsync", user.UserId.ToString()) });
                }
            });
        }

        protected override Task<PasswordVerificationResult> VerifyPasswordAsync(IUserPasswordStore<NuApplicationUser> store, NuApplicationUser user, string password)
        {
            return Task.Factory.StartNew<PasswordVerificationResult>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        Logger.LogInformation("Password verification failed.");
                        return PasswordVerificationResult.Failed;
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    if (u != null)
                    {
                        var currPassword = u.Password;

                        //TODO: decrypt password

                        var decrypted = u.Password;

                        return (decrypted == password) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
                    }
                    else
                    {
                        Logger.LogInformation($"Password verification failed for Username: {user.UserName}. Username not found");
                        return PasswordVerificationResult.Failed;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    Logger.LogInformation("Password verification failed.");
                    return PasswordVerificationResult.Failed;
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error setting lockout status for Usename: {user.UserName}.");
                    Logger.LogInformation("Password verification failed.");
                    return PasswordVerificationResult.Failed;
                }
            });
        }

        protected override string CreateTwoFactorRecoveryCode()
        {
            Logger.LogError("CreateTwoFactorRecoveryCode invoked. Two factor authentication is not supported.");
            return String.Empty;
        }

        public override Task<bool> VerifyUserTokenAsync(NuApplicationUser user, string tokenProvider, string purpose, string token)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("VerifyUserTokenAsync invoked. Tokens are not supported.");
                return false;
            });
        }

        public override Task<bool> VerifyTwoFactorTokenAsync(NuApplicationUser user, string tokenProvider, string token)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("VerifyTwoFactorTokenAsync invoked. Tokens are not supported.");
                return false;
            });
        }

        public override Task<bool> VerifyChangePhoneNumberTokenAsync(NuApplicationUser user, string token, string phoneNumber)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("VerifyChangePhoneNumberTokenAsync invoked. Tokens are not supported.");
                return false;
            });
        }

        public override Task<IdentityResult> UpdateSecurityStampAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("UpdateSecurityStampAsync invoked. Security tokens are not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("UpdateSecurityStampAsync") });
            });
        }

        public override Task UpdateNormalizedUserNameAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("UpdateNormalizedUserNameAsync invoked. Username normalization is not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("UpdateNormalizedUserNameAsync") });
            });
        }

        public override Task UpdateNormalizedEmailAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("UpdateNormalizedEmailAsync invoked. Email normalization is not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("UpdateNormalizedEmailAsync") });
            });
        }

        public override Task<IdentityResult> UpdateAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("UpdateAsync invoked. Updating user is not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("UpdateAsync") });
            });
        }

        public override Task<IdentityResult> SetTwoFactorEnabledAsync(NuApplicationUser user, bool enabled)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("SetTwoFactorEnabledAsync invoked. Two factor authentication is not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("SetTwoFactorEnabledAsync") });
            });
        }

        public override Task<IdentityResult> SetPhoneNumberAsync(NuApplicationUser user, string phoneNumber)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("SetPhoneNumberAsync invoked. Phone numbers are not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("SetPhoneNumberAsync") });
            });
        }

        public override Task<IdentityResult> SetAuthenticationTokenAsync(NuApplicationUser user, string loginProvider, string tokenName, string tokenValue)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("SetAuthenticationTokenAsync invoked. Tokens are not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("SetAuthenticationTokenAsync") });
            });
        }

        public override Task<IdentityResult> ResetAuthenticatorKeyAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("ResetAuthenticatorKeyAsync invoked. Authenticator keys are not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("ResetAuthenticatorKeyAsync") });
            });
        }

        public override Task<IdentityResult> RemoveLoginAsync(NuApplicationUser user, string loginProvider, string providerKey)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("RemoveLoginAsync invoked. Login providers are not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("RemoveLoginAsync") });
            });
        }

        public override Task<IdentityResult> RemoveFromRolesAsync(NuApplicationUser user, IEnumerable<string> roles)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("RemoveFromRolesAsync invoked. Roles are not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("RemoveFromRolesAsync") });
            });
        }

        public override Task<IdentityResult> RemoveFromRoleAsync(NuApplicationUser user, string role)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("RemoveFromRoleAsync invoked. Roles are not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("RemoveFromRoleAsync") });
            });
        }

        public override Task<IdentityResult> RemoveAuthenticationTokenAsync(NuApplicationUser user, string loginProvider, string tokenName)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("RemoveAuthenticationTokenAsync invoked. Tokens are not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("RemoveAuthenticationTokenAsync") });
            });
        }

        public override void RegisterTokenProvider(string providerName, IUserTwoFactorTokenProvider<NuApplicationUser> provider)
        {
            Logger.LogError("RedeemTwoFactorRecoveryCodeAsync invoked. Two factor authentication is not supported.");
        }

        public override Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(NuApplicationUser user, string code)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("RedeemTwoFactorRecoveryCodeAsync invoked. Two factor authentication is not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("RedeemTwoFactorRecoveryCodeAsync") });
            });
        }

        public override string NormalizeKey(string key)
        {
            return key;
        }

        public override Task<bool> IsPhoneNumberConfirmedAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("IsPhoneNumberConfirmedAsync invoked. Phone number confirmations are not supported.");
                return false;
            });
        }

        public override Task<bool> IsInRoleAsync(NuApplicationUser user, string role)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("IsInRoleAsync invoked. Roles are not supported.");
                return false;
            });
        }

        public override Task<bool> IsEmailConfirmedAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("IsEmailConfirmedAsync invoked. Eamil confirmations are not supported.");
                return false;
            });
        }

        public override Task<IList<string>> GetValidTwoFactorProvidersAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetValidTwoFactorProvidersAsync invoked. Login providers are not supported.");
                return new List<string>() as IList<string>;
            });
        }

        public override Task<IList<NuApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetUsersInRoleAsync invoked. Roles are not supported.");
                return new List<NuApplicationUser>() as IList<NuApplicationUser>;
            });
        }

        public override Task<bool> GetTwoFactorEnabledAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetTwoFactorEnabledAsync invoked. Two factor authentication is not supported.");
                return false;
            });
        }

        public override Task<string> GetSecurityStampAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetSecurityStampAsync invoked. Security stamps are not supported.");
                return String.Empty;
            });
        }

        public override Task<IList<string>> GetRolesAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetRolesAsync invoked. Roles are not supported.");
                return new List<string>() as IList<string>;
            });
        }

        public override Task<string> GetPhoneNumberAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetPhoneNumberAsync invoked. Phone numbers are not supported.");
                return String.Empty;
            });
        }

        public override Task<IList<UserLoginInfo>> GetLoginsAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetLoginsAsync invoked. Login providers are not supported.");
                return new List<UserLoginInfo>() as IList<UserLoginInfo>;
            });
        }

        public override Task<string> GetAuthenticatorKeyAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetAuthenticatorKeyAsync invoked. Authenticator keys are not supported.");
                return String.Empty;
            });
        }

        public override Task<string> GetAuthenticationTokenAsync(NuApplicationUser user, string loginProvider, string tokenName)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetAuthenticationTokenAsync invoked. Tokens are not supported.");
                return String.Empty;
            });
        }

        public override Task<string> GenerateUserTokenAsync(NuApplicationUser user, string tokenProvider, string purpose)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GenerateUserTokenAsync invoked. Tokens are not supported.");
                return String.Empty;
            });
        }

        public override Task<string> GeneratePasswordResetTokenAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GeneratePasswordResetTokenAsync invoked. Tokens are not supported.");
                return String.Empty;
            });
        }

        public override Task<string> GenerateTwoFactorTokenAsync(NuApplicationUser user, string tokenProvider)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GenerateTwoFactorTokenAsync invoked. Tokens are not supported.");
                return String.Empty;
            });
        }

        public override Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(NuApplicationUser user, int number)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GenerateNewTwoFactorRecoveryCodesAsync invoked. Recovery codes are not supported.");
                return new List<string>() as IEnumerable<string>;
            });
        }

        public override string GenerateNewAuthenticatorKey()
        {
            Logger.LogError("GenerateNewAuthenticatorKey invoked. Authenticator keys are not supported.");
            return String.Empty;
        }

        public override Task<string> GenerateConcurrencyStampAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GenerateConcurrencyStampAsync invoked. Concurrency stamps are not supported.");
                return String.Empty;
            });
        }

        public override Task<string> GenerateEmailConfirmationTokenAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GenerateEmailConfirmationTokenAsync invoked. Email tokens are not supported.");
                return String.Empty;
            });
        }

        public override Task<string> GenerateChangeEmailTokenAsync(NuApplicationUser user, string newEmail)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GenerateChangeEmailTokenAsync invoked. Email tokens are not supported.");
                return String.Empty;
            });
        }

        public override Task<string> GenerateChangePhoneNumberTokenAsync(NuApplicationUser user, string phoneNumber)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("AddToRoleAsync invoked. Phone number tokens are not supported.");
                return String.Empty ;
            });
        }
                                                          
        public override Task<NuApplicationUser> FindByLoginAsync(string loginProvider, string providerKey)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("FindByLoginAsync invoked. Exteranl logins are not supported.");
                return new NuApplicationUser();
            });
        }

        public override Task<IdentityResult> DeleteAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("DeleteAsync invoked. User deletion not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("DeleteAsync") });
            });
        }

        public override Task<byte[]> CreateSecurityTokenAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("CreateSecurityTokenAsync invoked. Roles are not supported.");
                return new byte[0];
            });
        }

        public override Task<IdentityResult> CreateAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("CreateAsync invoked. User creation not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("CreateAsync") });
            });
        }

        public override Task<IdentityResult> CreateAsync(NuApplicationUser user, string password)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("CreateAsync invoked. User creation not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("CreateAsync") });
            });
        }

        public override Task<int> CountRecoveryCodesAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("CountRecoveryCodesAsync invoked. Recovery codes are not supported.");
                return 0;
            });
        }

        public override Task<IdentityResult> ConfirmEmailAsync(NuApplicationUser user, string token)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("ConfirmEmailAsync invoked. Email validation tokens are not being used.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("ConfirmEmailAsync") });
            });
        }

        public override Task<IdentityResult> ChangePhoneNumberAsync(NuApplicationUser user, string phoneNumber, string token)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("ChangePhoneNumberAsync invoked. Phone numbers are not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("ChangePhoneNumberAsync") });
            });
        }

        public override Task<IdentityResult> AddToRoleAsync(NuApplicationUser user, string role)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("AddToRoleAsync invoked. Roles are not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("AddToRoleAsync") });
            });
        }

        public override Task<IdentityResult> AddToRolesAsync(NuApplicationUser user, IEnumerable<string> roles)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("AddToRolesAsync invoked. Roles are not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("AddToRolesAsync") });
            });
        }

        public override Task<IdentityResult> AddLoginAsync(NuApplicationUser user, UserLoginInfo login)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("AddLoginAsync invoked. External logins are not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("AddLoginAsync") });
            });
        }

    }
}
