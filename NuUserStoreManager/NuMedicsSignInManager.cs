using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using NuUserStoreManager.EF;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NuUserStoreManager
{
    public class NuMedicsSignInManager : SignInManager<NuApplicationUser>, IDisposable
    {
        public NuMedicsGlobalContext ctx;

        public NuMedicsSignInManager(UserManager<NuApplicationUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<NuApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<NuApplicationUser>> logger, IAuthenticationSchemeProvider schemes, NuMedicsGlobalContext dbContext) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
            ctx = dbContext;
        }

        public override Task<bool> CanSignInAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew<bool>(() => {

                if (user == null)
                {
                    Logger.LogInformation($"Can user sign in failed. NuApplicationUser is null.");
                    return false;
                }

                Logger.LogInformation($"Verifying user can sign in for Username: {user.UserName}");
                return (String.IsNullOrEmpty(user.UserName)) ? false : true;
            });
        }

        
        public override Task<SignInResult> CheckPasswordSignInAsync(NuApplicationUser user, string password, bool lockoutOnFailure)
        {
            return Task.Run<SignInResult>(async () => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return SignInResult.Failed;
                    }

                    if (String.IsNullOrEmpty(password))
                    {
                        Logger.LogError("Password parameter is empty.");
                        return SignInResult.Failed;
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    if (u != null)
                    {
                        if (String.IsNullOrEmpty(u.Password))
                        {
                            Logger.LogError("User password in database is empty.");
                            return SignInResult.Failed;
                        }

                        if (password == DataEncryption.Decrypt(u.Password))
                        {
                            const string Issuer = "https://numedics.com";

                            var claims = new List<Claim> {
                                new Claim(ClaimTypes.Name, u.Username, ClaimValueTypes.String, Issuer),
                                new Claim(ClaimTypes.Sid, u.UserId.ToString(), ClaimValueTypes.Sid, Issuer )
                            };

                            var userIdentity = new ClaimsIdentity("Login");
                            userIdentity.AddClaims(claims);

                            var principle = new ClaimsPrincipal(userIdentity);

                            await Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, new AuthenticationProperties
                            {
                                AllowRefresh = false,
                                IsPersistent = false,
                                ExpiresUtc = DateTime.UtcNow.AddHours(1)
                            });

                            return SignInResult.Success;
                        }
                        else
                        {
                            if (lockoutOnFailure)
                            {
                                Logger.LogInformation($"Sign in failed for Username: {user.UserName}. Automatic lockout is enabled.");
                                return await this.LockedOut(user);
                            }
                            else
                            {
                                Logger.LogInformation($"Sign in failed for Username: {user.UserName}.");
                                return SignInResult.Failed;
                            }
                        }
                    }
                    else
                    {
                        Logger.LogInformation($"Sign in failed for Username: {user.UserName}. Username not found");
                        return SignInResult.Failed;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return SignInResult.Failed;
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error validating password for Usename: {user.UserName}.");
                    return SignInResult.Failed;
                }
            });
        }

        public override AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl, string userId = null)
        {
            Logger.LogError("ConfigureExternalAuthenticationProperties invoked. External logins are not supported.");
            return new AuthenticationProperties();
        }

        public override Task<ClaimsPrincipal> CreateUserPrincipalAsync(NuApplicationUser user)
        {
            return Task.Run<ClaimsPrincipal>(async () => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return new ClaimsPrincipal();
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    if (u != null)
                    {
                        return await CreateClaimsPrincipleAsync(u.Username, u.UserId.ToString());
                    }
                    else
                    {
                        Logger.LogInformation($"Creating ClaimsPrinicple failed for Username: {user.UserName}. Username not found");
                        return new ClaimsPrincipal();
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return new ClaimsPrincipal();
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error validating password for Usename: {user.UserName}.");
                    return new ClaimsPrincipal();
                }
            });
        }

        public override Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("ExternalLoginSignInAsync invoked. External logins are not supported.");
                return SignInResult.Failed;
            });
        }

        public override Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("ExternalLoginSignInAsync invoked. External logins are not supported.");
                return SignInResult.Failed;
            });
        }

        public override Task ForgetTwoFactorClientAsync()
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("ForgetTwoFactorClientAsync invoked. Two factor authenticaion is not supported.");
            });
        }

        public override Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetExternalAuthenticationSchemesAsync invoked. External logins are not supported.");
                return new List<AuthenticationScheme>() as IEnumerable<AuthenticationScheme>;
            });
        }

        public override Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("GetExternalLoginInfoAsync invoked. External logins are not supported.");
                return new ExternalLoginInfo(new ClaimsPrincipal(), String.Empty, String.Empty, String.Empty);
            });
        }

        public override Task<NuApplicationUser> GetTwoFactorAuthenticationUserAsync()
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("IsTwoFactorClientRememberedAsync invoked. Two factor authenticaion is not supported.");
                return new NuApplicationUser();
            });
        }

        public override bool IsSignedIn(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                Logger.LogWarning("Is user signed failed. ClaimsPrinciple is null.");
                return false;
            }

            return (principal.Identity == null) ? false : principal.Identity.IsAuthenticated;
        }

        public override Task<bool> IsTwoFactorClientRememberedAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("IsTwoFactorClientRememberedAsync invoked. Two factor authenticaion is not supported.");
                return false;
            });
        }

        public override Task<SignInResult> PasswordSignInAsync(NuApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return Task.Run<SignInResult>(async () => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return SignInResult.Failed;
                    }

                    if (String.IsNullOrEmpty(password))
                    {
                        Logger.LogError("Password parameter is empty.");
                        return SignInResult.Failed;
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    if (u != null)
                    {
                        if (String.IsNullOrEmpty(u.Password))
                        {
                            Logger.LogError("User password in database is empty.");
                            return SignInResult.Failed;
                        }

                        if (password == DataEncryption.Decrypt(u.Password))
                        {
                            await this.CreateSignInContextAsync(u.Username, u.UserId.ToString(), isPersistent);
                            return SignInResult.Success;
                        }
                        else
                        {
                            if (lockoutOnFailure)
                            {
                                Logger.LogInformation($"Sign in failed for Username: {user.UserName}. Automatic lockout is enabled.");
                                return await this.LockedOut(user);
                            }
                            else
                            {
                                Logger.LogInformation($"Sign in failed for Username: {user.UserName}.");
                                return SignInResult.Failed;
                            }
                        }
                    }
                    else
                    {
                        Logger.LogInformation($"Sign in failed for Username: {user.UserName}. Username not found");
                        return SignInResult.Failed;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return SignInResult.Failed;
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error validating password for Usename: {user.UserName}.");
                    return SignInResult.Failed;
                }
            });
        }

        public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return Task.Run<SignInResult>(async () => {
                try
                {
                    if (String.IsNullOrEmpty(userName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return SignInResult.Failed;
                    }

                    if (String.IsNullOrEmpty(password))
                    {
                        Logger.LogError("Password parameter is empty.");
                        return SignInResult.Failed;
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == userName);

                    if (u != null)
                    {
                        if (String.IsNullOrEmpty(u.Password))
                        {
                            Logger.LogError("User password in database is empty.");
                            return SignInResult.Failed;
                        }

                        if (password == DataEncryption.Decrypt(u.Password))
                        {
                            await this.CreateSignInContextAsync(u.Username, u.UserId.ToString(), isPersistent);
                            return SignInResult.Success;
                        }
                        else
                        {
                            if (lockoutOnFailure)
                            {
                                Logger.LogInformation($"Sign in failed for Username: {userName}. Automatic lockout is enabled.");
                                return await this.LockedOut(new NuApplicationUser(u.Username));
                            }
                            else
                            {
                                Logger.LogInformation($"Sign in failed for Username: {userName}.");
                                return SignInResult.Failed;
                            }
                        }
                    }
                    else
                    {
                        Logger.LogInformation($"Sign in failed for Username: {userName}. Username not found");
                        return SignInResult.Failed;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {userName}.");
                    return SignInResult.Failed;
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error validating password for Usename: {userName}.");
                    return SignInResult.Failed;
                }
            });
        }

        public override Task RefreshSignInAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("RefreshSignInAsync invoked. Cookie refresh is not supported.");
            });
        }

        public override Task RememberTwoFactorClientAsync(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("RememberTwoFactorClientAsync invoked. Two factor authenticaion is not supported.");
                return SignInResult.Failed;
            });
        }

        public override Task SignInAsync(NuApplicationUser user, bool isPersistent, string authenticationMethod = null)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("SignInAsync invoked. User must sign in with password (PasswordSignInAsync).");
            });
        }

        public override Task SignInAsync(NuApplicationUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("SignInAsync invoked. User must sign in with password (PasswordSignInAsync).");
            });
        }

        public override Task SignOutAsync()
        {
            return Task.Run(async () => {
                Logger.LogInformation("User signed out");
                await Context.SignOutAsync();
            });
        }

        public override Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("TwoFactorRecoveryCodeSignInAsync invoked. Two factor authenticaion is not supported.");
                return SignInResult.Failed;
            });
        }

        public override Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("TwoFactorRecoveryCodeSignInAsync invoked. Two factor authenticaion is not supported.");
                return SignInResult.Failed;
            });
        }

        public override Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberClient)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("TwoFactorSignInAsync invoked. Two factor authenticaion is not supported.");
                return SignInResult.Failed;
            });
        }

        public override Task<IdentityResult> UpdateExternalAuthenticationTokensAsync(ExternalLoginInfo externalLogin)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("UpdateExternalAuthenticationTokensAsync invoked. External logins are not supported.");
                return IdentityResult.Failed(new IdentityError { Description = AppMessages.NotSupported("UpdateExternalAuthenticationTokensAsync") });
            });
        }

        public override Task<NuApplicationUser> ValidateSecurityStampAsync(ClaimsPrincipal principal)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("ValidateSecurityStampAsync invoked. Security stamps are not supported.");
                return new NuApplicationUser();
            });
        }

        protected override Task<bool> IsLockedOut(NuApplicationUser user)
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
                        Logger.LogInformation($"Verifying Lockout status for Username: {user.UserName}");
                        return u.IsLockedOut;
                    }
                    else
                    {
                        Logger.LogInformation($"Verifying Lockout status failed for Username: {user.UserName}. Username not found");
                        return false;
                    }
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

        protected override Task<SignInResult> LockedOut(NuApplicationUser user)
        {
            return Task.Factory.StartNew<SignInResult>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return SignInResult.NotAllowed;
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    int success = 0;

                    if (u != null)
                    {
                        u.PasswordFailureCount = 5;
                        u.IsLockedOut = true;
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
                            Logger.LogInformation($"Locking out user failed for Username: {user.UserName}. Username not found");
                            return SignInResult.NotAllowed;
                        case 1:
                        default:
                            Logger.LogInformation($"Lockout user for Username: {user.UserName}");
                            return SignInResult.LockedOut;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return SignInResult.NotAllowed;
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error validating password for Usename: {user.UserName}.");
                    return SignInResult.NotAllowed;
                }
            });
        }

        protected override Task<SignInResult> PreSignInCheck(NuApplicationUser user)
        {
            return Task.Factory.StartNew<SignInResult>(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("NuApplicationUser is null or UserName is empty.");
                        return SignInResult.NotAllowed;
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    int success = 0;

                    if (u != null)
                    {
                        var cks = new List<bool> {
                            u.PasswordFailureCount < 5,
                            u.PasswordAnswerFailureCount < 5,
                            u.IsLockedOut.Equals(false)
                        };

                        success = cks.All(a => a == true) ? 1 : 0;
                    }
                    else
                    {
                        success = -1;
                    }

                    switch (success)
                    {
                        case 0:
                            Logger.LogInformation($"PreSignIn check failed for Username: {user.UserName}. User is locked out, password fail count is 5, or password answer count is 5");
                            return SignInResult.NotAllowed;
                        case -1:
                            Logger.LogInformation($"PreSignIn check failed for Username: {user.UserName}. Username not found");
                            return SignInResult.NotAllowed;
                        case 1:
                        default:
                            Logger.LogInformation($"PreSignIn check successful for Username: {user.UserName}");
                            return null;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                    return SignInResult.NotAllowed;
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error validating password for Usename: {user.UserName}.");
                    return SignInResult.NotAllowed;
                }
            });
        }

        protected override Task ResetLockout(NuApplicationUser user)
        {
            return Task.Factory.StartNew(() => {
                try
                {
                    if (user == null || String.IsNullOrEmpty(user.UserName))
                    {
                        Logger.LogError("Cannot reset user lockout. NuApplicationUser is null or UserName is empty.");
                    }

                    var u = ctx.UserAuthentications.SingleOrDefault(f => f.Username == user.UserName);

                    int success = 0;

                    if (u != null)
                    {
                        u.IsLockedOut = false;
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
                            Logger.LogInformation($"Lockout reset failed for Username: {user.UserName}. Username not found");
                            break;
                        case 1:
                        default:
                            Logger.LogInformation($"Lockout reset for Username: {user.UserName}");
                            break;
                    }
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogError(e, $"Duplicate username in database: {user.UserName}.");
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error validating password for Usename: {user.UserName}.");
                }
            });
        }

        protected override Task<SignInResult> SignInOrTwoFactorAsync(NuApplicationUser user, bool isPersistent, string loginProvider = null, bool bypassTwoFactor = false)
        {
            return Task.Factory.StartNew(() => {
                Logger.LogError("SignInOrTwoFactorAsync invoked. Two factor authenticaion is not supported.");
                return SignInResult.Failed;
            });
        }

        private Task CreateSignInContextAsync(string username, string userId, bool isPersistent)
        {
            return Task.Run(async () => {
                if (String.IsNullOrEmpty(userId) || String.IsNullOrEmpty(username))
                {
                    Logger.LogError("SignIn context failed. Username or UserId is empty.");
                }
                else
                {
                    var principle = await CreateClaimsPrincipleAsync(username, userId);

                    await Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, new AuthenticationProperties
                    {
                        AllowRefresh = false,
                        IsPersistent = isPersistent,
                        ExpiresUtc = DateTime.UtcNow.AddHours(1)
                    });
                }
            });
        }

        private Task<ClaimsPrincipal> CreateClaimsPrincipleAsync(string username, string userId)
        {
            return Task.Run<ClaimsPrincipal>(() => {
                if (String.IsNullOrEmpty(userId) || String.IsNullOrEmpty(username))
                {
                    Logger.LogError("Creation of ClaimsPrinciple failed. Username or UserId is empty.");
                    return new ClaimsPrincipal();
                }
                else
                {
                    const string Issuer = "https://numedics.com";

                    var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, username, ClaimValueTypes.String, Issuer),
                    new Claim(ClaimTypes.Sid, userId, ClaimValueTypes.Sid, Issuer )
                };

                    var userIdentity = new ClaimsIdentity("Login");
                    userIdentity.AddClaims(claims);

                    return new ClaimsPrincipal(userIdentity);
                }
            });
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    ((IDisposable)ctx).Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~NuMedicsSignInManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
