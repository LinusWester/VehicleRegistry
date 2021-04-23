using Microsoft.Owin.Security.OAuth;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VehicleRegistry.Repository.Classes;
using VehicleRegistry.Repository.Interfaces;

namespace VehicleRegistry.API.Providers
{
    public class OAuthWebApiAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IAccountRepository accountRepository;

        public OAuthWebApiAuthorizationServerProvider()
        {
            accountRepository = new AzureRepository();
        }
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            var accountList = accountRepository.GetAllAccounts();
            var account = accountList.Where(o => o.Username == context.UserName).FirstOrDefault();
            if (account == null)
            {
                context.SetError("User doesn't exist");
                return;
            }

            var databasePasswordByte = Convert.FromBase64String(account.Password);
            var databasePassword = Encoding.UTF8.GetString(databasePasswordByte);

            var inputPasswordByte = Convert.FromBase64String(context.Password);
            var inputPassword = Encoding.UTF8.GetString(inputPasswordByte);


            if (inputPassword == databasePassword)
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, account.Username));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid grant", "Incorrect login credentials");
                return;
            }
        }
    }
}