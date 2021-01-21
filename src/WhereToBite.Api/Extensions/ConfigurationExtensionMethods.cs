using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhereToBite.Infrastructure;

namespace WhereToBite.Api.Extensions
{
    internal static class ConfigurationExtensionMethods
    {
        public static void AddWhereToBiteContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionString"];
            
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<WhereToBiteContext>(options =>
                    {
                        options.UseNpgsql(connectionString,
                            sqlOptions =>
                            {
                                sqlOptions.UseNetTopologySuite();
                                sqlOptions.MigrationsAssembly("WhereToBite.Api");
                                sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(15), null);
                            });
                    }
                );
        }
    }
}