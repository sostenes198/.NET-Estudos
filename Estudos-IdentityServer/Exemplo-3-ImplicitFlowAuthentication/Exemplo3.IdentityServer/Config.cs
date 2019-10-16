// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Exemplo3.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> Apis =>
            new[]
            {
                new ApiResource("api1", "myApi")
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    ClientId = "client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes = {"api1"}
                },
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"api1"}
                },

                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    // where to redirect to after login
                    RedirectUris = {"http://localhost:5002/signin-oidc"},

                    // where to redirect to after logout
                    PostLogoutRedirectUris = {"http://localhost:5002/signout-callback-oidc"},

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };

        public static List<TestUser> GetUsers => new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "alice",
                Password = "password",

                Claims = new []
                {
                    new Claim("name", "Alice"),
                    new Claim("website", "https://alice.com")
                }
            },
            new TestUser
            {
                SubjectId = "2",
                Username = "bob",
                Password = "password",

                Claims = new []
                {
                    new Claim("name", "Bob"),
                    new Claim("website", "https://bob.com")
                }
            }
        };
    }
}