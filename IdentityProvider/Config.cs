using System;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace IdentityProvider
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "resapi",

                    // This API defines two scopes.
                    Scopes =
                    {
                        new Scope()
                        {
                            Name = "resapi.full_access",
                            DisplayName = "Full access to Resource API",
                        },
                        new Scope
                        {
                            Name = "resapi.read_only",
                            DisplayName = "Read only access to Resource API"
                        }
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "clientApp",
 
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
 
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes = { "resapi.read_only" }
                }
            };
        }
    }
}
