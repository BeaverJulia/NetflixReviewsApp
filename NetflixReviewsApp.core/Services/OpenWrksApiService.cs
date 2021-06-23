using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NetflixReviewsApp.core.Credentials;
using NetflixReviewsApp.core.Models;
using NetflixReviewsApp.core.OpenWrksApiSettings;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace NetflixReviewsApp.core.Services
{
    public class OpenWrksApiService : IOpenWrksApiService
    {
        private readonly IMemoryCache _cache;
        private readonly OpenWrksCredentials _openWrksCredentials;
        public string Token;
        public IRestResponse Response;

        public OpenWrksApiService(OpenWrksCredentials openWrksCredentials, IMemoryCache memoryCache)
        {
            _openWrksCredentials = openWrksCredentials;
            _cache = memoryCache;
            Console.WriteLine(GetAccessToken().Result);
        }

        private async Task<string> GetAccessToken()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), _openWrksCredentials.Url))
                    {
                        var credentials = string.Format("{0}:{1}", _openWrksCredentials.ClientId, _openWrksCredentials.ClientSecret);
                        var headerValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
                        request.Headers.TryAddWithoutValidation("Authorization", "Basic YXBwbGljYW50OmcqOGdkdzI0WFg0NWdzYXdmRERjc3phQGU=");

                        var contentList = new List<string>();
                        contentList.Add($"grant_type={Uri.EscapeDataString(_openWrksCredentials.GrantType)}");
                        contentList.Add($"scope={Uri.EscapeDataString(_openWrksCredentials.Scope)}");
                        request.Content = new StringContent(string.Join("&", contentList));
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                        var response = await httpClient.SendAsync(request);
                        var result = JsonConvert.DeserializeObject<AccessTokenResponse>(await response.Content.ReadAsStringAsync());

                        var token = $"Bearer {result.access_token}";
                        _cache.Set("Token", token);
                        return response.Content.ToString();
                    }
                }
               
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error getting access token: {exception.Message}");
                return exception.Message;
            }
        }

        public async Task<IRestResponse> GetShows(PaginationFilter pagination)
        {
            
            var _client = new RestClient(ApiRoutes.Shows) {Timeout = -1};
            var request = new RestRequest(Method.GET);
            try
            {
                Token = _cache.Get("Token").ToString();
                request.AddHeader("Authorization", Token);
                request.AddParameter("page", pagination.PageNumber);
                request.AddParameter("limit", pagination.Limit);
                Response = await _client.ExecuteAsync(request);
                return Response;
            }
            catch (Exception e)
            {
                //TODO Global Error handling
                return Response;
            }
        }

        public async Task<IRestResponse> GetShow(string id)
        {
            var _client = new RestClient(ApiRoutes.Shows + "/"+id) {Timeout = -1};
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