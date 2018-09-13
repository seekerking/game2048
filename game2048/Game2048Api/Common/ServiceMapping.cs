using Game2048EF.Repository;
using Game2048EF.Repository.Interfaces;
using Game2048EF.Services;
using Game2048EF.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Game2048Api.Common
{
    public static class ServiceMapping
    {

        public static IServiceCollection MappingRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUserReppository), typeof(UserRepository));
            services.AddScoped(typeof(IGameRepository), typeof(GameRepository));
            services.AddScoped(typeof(IOtherRepository), typeof(OtherRepository));
            return services;

        }

        public static IServiceCollection MappingService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUserService),typeof(UserService));
            services.AddScoped(typeof(IGameService), typeof(GameService));
            services.AddScoped(typeof(IOtherService), typeof(OtherService));
            return services;
        }
    }
}
