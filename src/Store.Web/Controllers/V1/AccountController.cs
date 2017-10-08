using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Store.Web.Infrastructure.ActionResults;
using Store.Web.Infrastructure.AuthProviders;
using DTO = Store.Contracts;
using BM = Store.Web.Models.BM;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Swashbuckle.Swagger.Annotations;
using Store.Web.Infrastructure.ValidationAttributes;
using System.Linq;
using Store.Web.Infrastructure.ExceptionHandling;

namespace Store.Web.Controllers.V1
{
    /// <summary>
    /// Accounts
    /// </summary>
    [Authorize]
    [RoutePrefix("api/{apiVersion}/account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountController()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="accessTokenFormat"></param>
        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }
        #endregion

        /// <summary>
        /// UserManager
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// AccessTokenFormat
        /// </summary>
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/v1/account/user-info
        /// <summary>
        /// Get user's info
        /// </summary>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("user-info")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DTO.UserInfo))]
        public DTO.UserInfo GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new DTO.UserInfo
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/v1/account/log-out
        /// <summary>
        /// Log out user
        /// </summary>
        /// <returns></returns>
        [Route("log-out")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);

            return Ok();
        }

        // GET api/v1/account/manage-info?returnUrl=%2F&generateState=true
        /// <summary>
        /// Manage info
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="generateState"></param>
        /// <returns></returns>
        [Route("manage-info")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DTO.ManageInfo))]
        public async Task<DTO.ManageInfo> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
                return null;

            List<DTO.UserLoginInfo> logins = new List<DTO.UserLoginInfo>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new DTO.UserLoginInfo
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new DTO.UserLoginInfo
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new DTO.ManageInfo
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/v1/account/change-password
        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("change-password")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> ChangePassword([FromBody]BM.ChangePassword model)
        {
            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(),
                model.OldPassword,
                model.NewPassword);

            if (result == null || !result.Succeeded)
                GetErrorResult(result);

            return Ok();
        }

        // POST api/v1/account/set-password
        /// <summary>
        /// Set password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("set-password")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> SetPassword([FromBody]BM.SetPassword model)
        {
            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (result == null || !result.Succeeded)
                GetErrorResult(result);

            return Ok();
        }

        // POST api/v1/account/add-external-login
        /// <summary>
        /// Add external login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("add-external-login")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> AddExternalLogin([FromBody]BM.AddExternalLogin model)
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
                throw new HttpException((int)HttpStatusCode.BadRequest, "The external login is already associated with an account.");

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
                GetErrorResult(result);

            return Ok();
        }

        // POST api/v1/account/remove-login
        /// <summary>
        /// Remove password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("remove-login")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> RemoveLogin([FromBody]BM.RemoveLogin model)
        {
            IdentityResult result = null;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (result == null || !result.Succeeded)
                GetErrorResult(result);
  
            return Ok();
        }

        // GET api/account/external-login
        /// <summary>
        /// Get external login
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("external-login", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));

            if (!User.Identity.IsAuthenticated)
                return new ChallengeResult(provider, this);

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
                throw new HttpException((int)HttpStatusCode.InternalServerError, string.Empty);

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                return new ChallengeResult(provider, this);
            }

            Entities.ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);
                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);

                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);

                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/v1/account/external-logins?returnUrl=%2F&generateState=true
        /// <summary>
        /// Get external logins
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="generateState"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("external-logins")]
        public IEnumerable<DTO.ExternalLogin> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<DTO.ExternalLogin> logins = new List<DTO.ExternalLogin>();

            string state = null;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                DTO.ExternalLogin login = new DTO.ExternalLogin
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };

                logins.Add(login);
            }

            return logins;
        }

        // POST api/v1/account/register
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("register")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> Register([FromBody]BM.Register model)
        {
            var user = new Entities.ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                GetErrorResult(result);

            return Ok();
        }

        // POST api/v1/account/register-external
        /// <summary>
        /// Register external
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("register-external")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> RegisterExternal([FromBody]BM.RegisterExternal model)
        {
            ExternalLoginInfo info = await Authentication.GetExternalLoginInfoAsync();

            if (info == null)
                throw new HttpException((int)HttpStatusCode.InternalServerError, string.Empty);

            var user = new Entities.ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            IdentityResult result = await UserManager.CreateAsync(user);

            if (result == null || !result.Succeeded)
                GetErrorResult(result);

            result = await UserManager.AddLoginAsync(user.Id, info.Login);

            if (result == null || !result.Succeeded)
                GetErrorResult(result);

            return Ok();
        }

        /// <summary>
        /// Dispose related resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        private IAuthenticationManager Authentication
        {
            get
            {
                return Request.GetOwinContext().Authentication;
            }
        }

        private void GetErrorResult(IdentityResult result)
        {
            string aggregatedErrorMsg = string.Empty;

            if (result.Errors != null)
                aggregatedErrorMsg = string.Join(Environment.NewLine, result.Errors);

            throw new IdentityException(aggregatedErrorMsg);
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }

            public string ProviderKey { get; set; }

            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                    return null;

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null 
                    || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                    return null;

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }
        #endregion
    }
}
