using System;
using Microsoft.Extensions.DependencyInjection;

namespace AuthWebApps.AuthServices.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers authentication services.
        /// </summary>
        /// <typeparam name="TSessionData">Session data type.</typeparam>
        /// <param name="services">Service collection.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddAuthServicesInMemory<TSessionData>(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            return services
                .AddSingleton<IAccountService, AccountServiceInMemory>()
                .AddSingleton<ISessionService<TSessionData>, SessionServiceInMemory<TSessionData>>()
                .AddSingleton<IRoleService, RoleServiceInMemory>();
        }
    }
}
