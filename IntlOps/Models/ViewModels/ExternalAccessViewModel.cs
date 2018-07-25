using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IntlOps.Models
{
    public class ExternalAccessViewModel
    {
        public class ExternalLoginViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
        public class ExternalLoginsViewModel
        {
            public IList<UserLoginInfo> CurrentLogins { get; set; }

            public IList<AuthenticationScheme> OtherLogins { get; set; }

            public bool ShowRemoveButton { get; set; }

            public string StatusMessage { get; set; }
        }
        public class EnableAuthenticatorViewModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Verification Code")]
            public string Code { get; set; }

            [ReadOnly(true)]
            public string SharedKey { get; set; }

            public string AuthenticatorUri { get; set; }
        }
        public class LoginWithRecoveryCodeViewModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Recovery Code")]
            public string RecoveryCode { get; set; }
        }
        public class LoginWith2faViewModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; }

            [Display(Name = "Remember this machine")]
            public bool RememberMachine { get; set; }

            public bool RememberMe { get; set; }
        }
        public class GenerateRecoveryCodesViewModel
        {
            public string[] RecoveryCodes { get; set; }
        }
        public class TwoFactorAuthenticationViewModel
        {
            public bool HasAuthenticator { get; set; }

            public int RecoveryCodesLeft { get; set; }

            public bool Is2faEnabled { get; set; }
        }
    }
}
