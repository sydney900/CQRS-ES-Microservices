// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("clientservicescope",new []{ "role", "admin", "user", "clientservice", "clientservice.admin", "clientservice.user" } ),
                new IdentityResource("productservicescope",new []{ "role", "admin", "user", "productservice", "productservice.admin", "productservice.user" } ),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("client.service.api", "Client service API") {
                    ApiSecrets =
                    {
                        new Secret("client.service.api.secret".Sha256())
                    },
                },
                new ApiResource("product.service.api", "Product service API"),
                new ApiResource("api1", "API1")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            string myLocalIp = System.Environment.GetEnvironmentVariable("MYIP");
            if (string.IsNullOrEmpty(myLocalIp))
                myLocalIp = "localhost";

            return new[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "client.service.api", "product.service.api" }
                },

                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    RedirectUris = { $"http://{myLocalIp}:5001/signin-oidc" },
                    FrontChannelLogoutUri = $"http://{myLocalIp}:5001/signout-oidc",
                    PostLogoutRedirectUris = { $"http://{myLocalIp}:5001/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "client.service.api", "product.service.api", "api1" }
                },

                // SPA client using implicit flow
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SPA Client",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris =
                    {
                        $"http://{myLocalIp}:64003",
                    },
                    
                    PostLogoutRedirectUris = { $"http://{myLocalIp}:64003" },
                    AllowedCorsOrigins = { $"http://{myLocalIp}:64003" },

                    AllowedScopes = { "openid", "profile", "client.service.api", "product.service.api" }
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    ClientName = "Resource Owner Client",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedCorsOrigins = { $"http://{myLocalIp}:64003" },

                    ClientSecrets =
                    {
                        new Secret("8FEA01FC-5D0C-4285-9E69-27C04D53D7D2".Sha256())
                    },
                    AllowedScopes = { "openid", "profile", "client.service.api", "product.service.api", "api1" }
                }

            };
        }
    }
}