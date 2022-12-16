using Microsoft.Extensions.DependencyInjection;
using MovieMaster.Data.Interfaces;

namespace MovieMaster.Data
{
    public static class Startup
    {
        public static IServiceCollection ConfigureDataDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IMovieRepository, MovieRepository>();

            return services;
        }
    }
}