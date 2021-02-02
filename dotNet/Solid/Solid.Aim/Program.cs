using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Solid.Aim.Contracts;
using Solid.Aim.Controllers;
using Solid.Aim.Services;
using Solid.Aim.Services.Impl;

namespace Solid.Aim
{
    internal class Program
    {
        private static IServiceProvider _services;

        private static void Main()
        {
            Console.WriteLine("Hello SOLID and IoC!");
            Setup();
            RegistrationTest();
            UserProfileTest();
        }

        private static void Setup()
        {
            var services = new ServiceCollection();

            services.AddTransient<ILogger, Logger>();

            services.AddScoped<AuthController>();
            services.AddScoped<UserController>();

            services.AddSingleton<IAccountStorage, AccountStorage>();
            services.AddSingleton<IUserStorage, UserStorage>();

            services.AddSingleton<IUserValidator, UserValidator>();
            services.AddSingleton<IAuthService, AuthService>();

            _services = services.BuildServiceProvider(true);
        }

        private static void RegistrationTest()
        {
            var scopeFactory = _services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var auth = scope.ServiceProvider.GetRequiredService<AuthController>();

            Debug.Assert(auth.Register("dima", "abc"));
            Debug.Assert(auth.Register("max", "abc"));
        }

        private static void UserProfileTest()
        {
            var scopeFactory = _services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var auth = scope.ServiceProvider.GetRequiredService<AuthController>();
            var users = scope.ServiceProvider.GetRequiredService<UserController>();
                
            var token = auth.Login("dima", "abc");
            Debug.Assert(token != null);

            users.UpsertUser(token, new UserDto
            {
                Age = 27,
                Name = "Dima"
            });

            var dima = users.GetUser(token);
            Debug.Assert(dima?.Name == "Dima");
        }
    }
}
