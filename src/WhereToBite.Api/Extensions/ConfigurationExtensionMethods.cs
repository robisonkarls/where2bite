using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhereToBite.Infrastructure;

namespace WhereToBite.Api.Extensions
{
    internal static class ConfigurationExtensionMethods
    {
        public static IServiceCollection AddWhereToBiteContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<WhereToBiteContext>(options =>
                    {
                        options.UseNpgsql(configuration["ConnectionString"],
                            sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly("WhereToBite.Api");
                                sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(15), null);
                            });
                    }
                );
            return services;
        }
    }
}