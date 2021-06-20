using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NetflixReviewsApp.api.Installers
{
    public interface IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services);
    }
}