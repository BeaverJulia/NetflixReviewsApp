using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NetflixReviewsApp.core.Credentials;
using NetflixReviewsApp.core.OpenWrksApiSettings;
using Newtonsoft.Json;
using RestSharp;

namespace NetflixReviewsApp.core.Services
{
    public class OpenWorksApiService : IOpenWorksApiService
    {
        private readonly IMemoryCache _cache;
        private readonly OpenWrksCredentials _openWrksCredentials;
        public string Token;
        public IRestResponse Response;

        public OpenWorksApiService(OpenWrksCredentials openWrksCredentials, IMemoryCache memoryCache)
        {
            _openWrksCredentials = openWrksCredentials;
            _cache = memoryCache;
            Console.WriteLine(GetAccessToken().Result);
        }

        private async Task<string> GetAccessToken()
        {
            try
            {
                var _client = new RestClient(_openWrksCredentials.Url) {Timeout = -1};
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                request.AddHeader("Cookie",
                    "x-ms-gateway-slice=estsfd; stsservicecookie=estsfd; fpc=AvsjvgTGW7NHgRCSwHZr4tXPl6bbAQAAALmKadcOAAAA");
                request.AddParameter("grant_type", _openWrksCredentials.GrantType);
                request.AddParameter("client_id", _openWrksCredentials.ClientId);
                request.AddParameter("client_secret", _openWrksCredentials.ClientSecret);
                request.AddParameter("scope", _openWrksCredentials.Scope);
                var response = await _client.ExecuteAsync(request);
                var result = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content);

                var token = $"Bearer {result.access_token}";
                _cache.Set("Token", token);
                return response.Content;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error getting access token: {exception.Message}");
                return exception.Message;
            }
        }

        public async Task<IRestResponse> GetShows()
        {
            var _client = new RestClient(ApiRoutes.Shows) {Timeout = -1};
            var request = new RestRequest(Method.GET);
            try
            {
                Token = _cache.Get("Token").ToString();
                request.AddHeader("Authorization", Token);
                Response = await _client.ExecuteAsync(request);
                return Response;
            }
            catch (Exception e)
            {
                return Response;
            }
        }

        public async Task<IRestResponse> GetShow(string id)
        {
            var _client = new RestClient(ApiRoutes.Shows + "/id") {Timeout = -1};
            var request = new RestRequest(Method.GET);
            try
            {
                Token = _cache.Get("Token").ToString();
                request.AddHeader("Authorization", Token);
                Response = await _client.ExecuteAsync(request);
                return Response;
            }
            catch (Exception e)
            {
                return Response;
            }
        }
    }
}