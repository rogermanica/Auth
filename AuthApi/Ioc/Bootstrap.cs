using System;
using AuthApi.Dados;
using AuthApi.Util;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AuthApi.Ioc
{
    public static class Bootstrap
    {
        public static IServiceCollection RegisterServices(IServiceCollection services)
        {
            #region Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion
            
            #region Services
            services.AddSingleton<JwtSettings>();
            services.AddScoped<IJwtService, JwtService>();
            #endregion

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<AuthenticatedUser>();

            var assembly = AppDomain.CurrentDomain.Load("AuthApi");

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestsValidationMiddleware<,>));
            services.AddMediatR(assembly);

            return services;
        }
    }
}