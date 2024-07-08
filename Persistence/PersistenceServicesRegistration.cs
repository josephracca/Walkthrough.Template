using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Connection;
using Persistence.Context;
using Persistence.Identity;
using Persistence.Repositories;
using Persistence.Services;
using System;
using Application.Contracts.Connection;
using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Application.Contracts.Services;
using Application.Models;

namespace Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<Captcha>(configuration.GetSection("Captcha"));
            services.AddTransient<Captcha>();
            services.AddDbContext<TemplateDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("TemplateConnectionString")));
            services.AddScoped<IApplicationDbConnection, ApplicationDbConnection>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
