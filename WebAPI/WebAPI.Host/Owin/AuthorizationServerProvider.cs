using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using WebAPI.BLL.Services;

namespace WebAPI.Host.Owin
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly ApplicationUserManager _userManager;

        public AuthorizationServerProvider(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Factory.StartNew(() => context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = await _userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.Rejected();
                return;
            }

            var role = (await _userManager.GetRolesAsync(user.Id)).FirstOrDefault();
            var id = new ClaimsIdentity(context.Options.AuthenticationType);
            id.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            id.AddClaim(new Claim(ClaimTypes.Role, role ?? "User"));

            context.Validated(id);
        }
    }
}