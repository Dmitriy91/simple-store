using System.Collections.Generic;

namespace Assignment.Web.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginDTO
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoDTO
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoDTO> Logins { get; set; }

        public IEnumerable<ExternalLoginDTO> ExternalLoginProviders { get; set; }
    }

    public class UserInfoDTO
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoDTO
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
