using System.Collections.Generic;

#pragma warning disable 1591

namespace Assignment.Web.Models.DTO
{
    // Models returned by AccountController actions.

    public class ExternalLogin
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfo
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfo> Logins { get; set; }

        public IEnumerable<ExternalLogin> ExternalLoginProviders { get; set; }
    }

    public class UserInfo
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfo
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}

#pragma warning restore 1591
