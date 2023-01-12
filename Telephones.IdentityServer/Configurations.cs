using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Telephones.IdentityServer
{
    public static class Configurations
    {
        public static IEnumerable<Client> GetClients() => new List<Client>()
        {
            new Client
            {
                ClientId = "client_id_web",
                ClientSecrets = { new Secret("client_secret_web".ToSha256()) },

                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    "TelephonesAPI",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },

                RedirectUris = { "https://localhost:7236/signin-oidc" }
            },
            new Client
            {
                ClientId = "client_id",
                ClientSecrets = { new Secret("client_secret".ToSha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes =
                {
                    "TelephonesAPI"
                }
            }
        };

        public static IEnumerable<ApiResource> GetApiResorces() => new List<ApiResource>()
        {
            new ApiResource("TelephonesAPI"),
            new ApiResource("TelephonesWEB")
        };

        public static IEnumerable<IdentityResource> GetIdentityResorces() => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()  
        };
    }
}
