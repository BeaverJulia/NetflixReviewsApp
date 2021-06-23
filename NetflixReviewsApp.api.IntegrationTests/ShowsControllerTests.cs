using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NetflixReviewsApp.core.Contracts;
using NetflixReviewsApp.core.Models;
using Newtonsoft.Json;
using Xunit;

namespace NetflixReviewsApp.api.IntegrationTests
{
    public class ShowsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ShowsControllerTests(WebApplicationFactory<Startup> fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async Task Post_ShouldAddReview()
        {
            var review = new ReviewInput
            {
                Description = "test description",
                Nickname = "Nickname",
                ShowId = "s1",
                Stars = 3
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(review),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync("v1/review", content);

            response.EnsureSuccessStatusCode();
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Get_Should_ReturnListOfShows()
        {
            var response = await _client.GetAsync("v1/shows");
            var result =
                JsonConvert.DeserializeObject<PagedResponse<ShowWithReview>>(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();

            Assert.IsType<PagedResponse<ShowWithReview>>(result);
            Assert.NotNull(result);
        }
    }
}