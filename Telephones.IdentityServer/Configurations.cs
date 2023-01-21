using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Security.Claims;

namespace Telephones.IdentityServer
{
    public static class Configurations
    {
        public static IEnumerable<Client> GetClients() => new List<Client>()
        {
            new Client
            {
                ClientId = "client_id_wpf",
                ClientSecrets = { new Secret("client_secret_wpf".ToSha256()) },

                AllowAccessTokensViaBrowser = true,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes =
                {
                    "TelephonesWPF",
                    "TelephonesAPI",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },

                RequireConsent = false,
                RequireClientSecret = false,

                AllowOfflineAccess = true
            },
            new Client
            {
                ClientId = "client_id_web",
                ClientSecrets = { new Secret("client_secret_web".ToSha256()) },

                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    "TelephonesWEB",
                    "TelephonesAPI",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },

                RedirectUris = { "https://localhost:7236/signin-oidc" },
                PostLogoutRedirectUris = {"https://localhost:7236/signout-callback-oidc"},

                RequireConsent = false,

                AllowOfflineAccess = true
            },
            new Client
            {
                ClientId = "client_id",
                ClientSecrets = { new Secret("client_secret".ToSha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes =
                {
                    "TelephonesAPI",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },

                AllowOfflineAccess = true
            }
        };

        public static IEnumerable<ApiResource> GetApiResorces() => new List<ApiResource>()
        {
            new ApiResource("TelephonesAPI"),
            new ApiResource("TelephonesWEB"),
            new ApiResource("TelephonesWPF")
        };

        public static IEnumerable<IdentityResource> GetIdentityResorces() => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()  
        };

        public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope>()
        {
            new ApiScope("TelephonesWPF"),
            new ApiScope("TelephonesAPI"),
            new ApiScope("TelephonesWEB")
        };
    }
}
