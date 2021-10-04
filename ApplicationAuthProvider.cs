using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VMSAPIWEB.Models;

namespace VMSAPIWEB
{
    public class ApplicationAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            AuthRepository authRepository = new AuthRepository();
            var Valid = authRepository.dynValidateUser(context.UserName,
            context.Password);

            if (Valid != null)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("Username", context.UserName));
                identity.AddClaim(new Claim("Password", context.Password));
                if (Valid.GetType() == typeof(User_tbl))
                {
                    var propertyDictionary = new Dictionary<string, string> { { "user_name", Valid.Name }, { "user_Id", Valid.CustomerId.ToString() } };
                    var properties = new AuthenticationProperties(propertyDictionary);
                    AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);
                    context.Validated(ticket);
                }
                else
                {
                    var propertyDictionary = new Dictionary<string, string> { { "user_name", Valid.Fname }, { "user_Id", Valid.UserId.ToString() } };
                    var properties = new AuthenticationProperties(propertyDictionary);
                    AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);
                    context.Validated(ticket);
                }
            }
            else
            {
                context.SetError("invalid_grant", "The user name or password is      incorrect.");
                return;
            }
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                if (property.Key == "user_name" || property.Key == "user_Id")
                {
                    context.AdditionalResponseParameters.Add(property.Key, property.Value);
                }
            }
            return Task.FromResult<object>(null);
        }
    }
}