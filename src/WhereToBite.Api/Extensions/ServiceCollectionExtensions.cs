using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;

namespace WhereToBite.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WhereToBite - Establishments HTTP API",
                    Version = "v1",
                    Description = "Establishment Service HTTP API"
                });
            });
            return services;
        }
    }
}