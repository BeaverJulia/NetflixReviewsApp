using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetflixReviewsApp.core.Models;
using NetflixReviewsApp.core.Models.OpenWrksModels;
using Newtonsoft.Json;
using Xunit;
using Assert = Xunit.Assert;

namespace NetflixReviewsApp.api.IntegrationTests
{
    public class ShowsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
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
            var result = JsonConvert.DeserializeObject<List<ShowWithReview>>(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();

            Assert.IsType<List<ShowWithReview>>(result);
            Assert.NotEmpty(result);
        }
    }

}