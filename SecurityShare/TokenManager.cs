using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Common
{
    public class TokenManager
    {
        private IdentitySetting identitySetting;
        public TokenManager(IdentitySetting identitySetting)

        {
            this.identitySetting = identitySetting;

            if (this.identitySetting == null)
            {
                throw new ArgumentNullException("Identity server settings are needed");
            }
        }

        #region for the api client token
        TokenResponse clientApiToken;

        public async Task<TokenResponse> GetClientCredentialApiToken()
        {
            if (clientApiToken == null)
            {
                // discover endpoints from metadata
                var disco = await DiscoveryClient.GetAsync(identitySetting.IdentityServerUrl);
                if (disco.IsError)
                {
                    return new TokenResponse((HttpStatusCode)StatusCodes.Status511NetworkAuthenticationRequired, disco.Error, "DiscoveryClient");
                }

                // request token
                var tokenClient = new TokenClient(disco.TokenEndpoint, identitySetting.ClientId, identitySetting.ClientSecrets);
                var tokenResponse = await tokenClient.RequestClientCredentialsAsync(identitySetting.Scopes);

                if (tokenResponse.IsError)
                {
                    return new TokenResponse((HttpStatusCode)StatusCodes.Status511NetworkAuthenticationRequired, tokenResponse.Error, "TokenClient");
                }

                clientApiToken = tokenResponse;

                return clientApiToken;
            }
            else
                return await Task.FromResult(clientApiToken);
        }

        #endregion

        #region for the resource owner passwords
        public async Task<TokenResponse> GetResourceOnwerPasswordApiToken(string userName, string password)
        {
            if (clientApiToken == null)
            {
                // discover endpoints from metadata
                var disco = await DiscoveryClient.GetAsync(identitySetting.IdentityServerUrl);
                if (disco.IsError)
                {
                    return new TokenResponse((HttpStatusCode)StatusCodes.Status511NetworkAuthenticationRequired, disco.Error, "DiscoveryClient");
                }

                // request token
                var tokenClient = new TokenClient(disco.TokenEndpoint, identitySetting.ClientId, identitySetting.ClientSecrets);
                var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(userName, password, identitySetting.Scopes);

                if (tokenResponse.IsError)
                {
                    return new TokenResponse((HttpStatusCode)StatusCodes.Status511NetworkAuthenticationRequired, tokenResponse.Error, "TokenClient");
                }

                clientApiToken = tokenResponse;

                return clientApiToken;
            }
            else
                return await Task.FromResult(clientApiToken);
        }
        #endregion
    }
}
