using System.Collections.Generic;
using IdentityServer4.Models;

namespace Estudos.IdentityServerHost
{
    public static class Config
    {
        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId = "mvc-client",
                ClientName = "My mvc Client",
                ClientSecrets = new List<Secret>
                {
                    new Secret("segredo".Sha256())
                },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = new List<string>
                {
                    "http://localhost:9090/signin-oidc"
                },
                AllowedScopes = new List<string>
                {
                    "openid",
                    "profile",
                    "email"
                }
            }
        }; 
        
        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };
        
        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
        {
            new ApiResource("HumanResources"),
            new ApiResource("Financial")
            {
                Scopes = new List<Scope>
                {
                    new Scope("Financial.Read"),
                    new Scope("Financial.Write")
                }
            }
            
        };
    }
}