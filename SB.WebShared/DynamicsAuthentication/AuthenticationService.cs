using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SB.SharedModels.Actions;
using System;
using System.Threading.Tasks;

namespace SB.WebShared.DynamicsAuthentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string aadInstance = "https://login.microsoftonline.com/";

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _resource;
        private readonly string _apiVersion;
        private readonly string _tenantId;

        public AuthenticationService(CrmClientOptions crmClientOptions)
        {
            _clientId = crmClientOptions.ClientId;
            _clientSecret = crmClientOptions.ClientSecret;
            _resource = crmClientOptions.Resource;
            _apiVersion = crmClientOptions.ApiVersion;
            _tenantId = crmClientOptions.TenantId;
        }

        public async Task<string> GetToken()
        {
            var clientCred = new ClientCredential(_clientId, _clientSecret);
            var authenticationContext = new AuthenticationContext(aadInstance + _tenantId);
            var authenticationResult = await authenticationContext.AcquireTokenAsync(_resource, clientCred);

            return authenticationResult.AccessToken;
        }

        public Uri GetServiceUri()
        {
            var serviceUrl = new Uri($"{_resource}/api/data/{_apiVersion}/{ActionNames.ActionTracking}");

            return serviceUrl;
        }

        public string GetConnectionString()
        {
            return @$"Url={_resource};AuthType=ClientSecret;ClientId={_clientId};ClientSecret={_clientSecret};RequireNewInstance=true";
        }
    }
}