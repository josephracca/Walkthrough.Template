using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Contracts.Infrastructure;
using Application.Models;

namespace Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection("EmailSettings").Get<EmailSettings>());
            services.AddSingleton(configuration.GetSection("AzureStorage").Get<AzureStorage>());
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IStorageService, StorageService>();
            return services;
        }
    }
}
