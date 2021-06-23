using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using NetflixReviewsApp.core.Credentials;
using NetflixReviewsApp.core.Services;
using Newtonsoft.Json;

namespace NetflixReviewsApp.api.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            var openWrksCredentials = new OpenWrksCredentials();

            var path = @"OpenWrksCredentials.json";
            using (var r = new StreamReader(path))
            {
                var json = r.ReadToEnd();
                openWrksCredentials = JsonConvert.DeserializeObject<OpenWrksCredentials>(json);
            }

            services.AddSingleton(openWrksCredentials);
            services.AddScoped<IOpenWrksApiService, OpenWrksApiService>();
            services.AddScoped<IReviewsService, ReviewsService>();
            services.AddScoped<IUriService, UriService>();
            services.AddMemoryCache();
            services.AddControllers();
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo {Title = "NetflixReviewsApi", Version = "v1"});
            });
        }
    }
}