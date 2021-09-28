using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SB.WebShared.DynamicsAuthentication;
using SB.WebShared.Response;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SB.WebShared.Interactors
{
    public class DynamicsInteractor : IDynamicsInteractor
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly HttpClient _httpClient;
        private readonly Uri _uri;
        private string _token;

        public DynamicsInteractor(IAuthenticationService authenticationService, HttpClient httpClient)
        {
            _authenticationService = authenticationService;
            _httpClient = httpClient;
            _uri = authenticationService.GetServiceUri();
        }

        public async Task<string> SendAction(string actionName, object obj = null)
        {
            if (string.IsNullOrWhiteSpace(actionName))
            {
                throw new Exception($"{nameof(actionName)} is null or white-space");
            }

            var json = JsonCreatorService.FormStringContent(obj, actionName);

            var response = await _httpClient.PostAsync(_uri, json);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _token = await _authenticationService.GetToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                response = await _httpClient.PostAsync(_uri, json);
            }

            var responseMessage = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new BadHttpRequestException($"Action return error: {responseMessage}");
            }

            var responseModel = JsonConvert.DeserializeObject<ResponseModel>(responseMessage);

            if (responseModel == null)
            {
                throw new BadHttpRequestException("Response not deserialize");
            }

            return responseModel.Response;
        }
    }
}