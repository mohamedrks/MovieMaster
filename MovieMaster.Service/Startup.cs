using Microsoft.Extensions.DependencyInjection;
using MovieMaster.Data;
using MovieMaster.Service.Interfaces;

namespace MovieMaster.Service
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IMovieService, MovieService>();
            services.ConfigureDataDependencies();

            return services;
        }
    }
}