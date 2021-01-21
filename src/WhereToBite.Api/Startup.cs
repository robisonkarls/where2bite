using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WhereToBite.Api.Extensions;
using WhereToBite.Api.Infrastructure.Filters;
using WhereToBite.Api.Infrastructure.Mappers;
using WhereToBite.Api.ServiceWorker;
using WhereToBite.Core.DataExtractor.Abstraction;
using WhereToBite.Core.DataExtractor.Concrete;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;
using WhereToBite.Infrastructure.Repositories;

namespace WhereToBite.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var dineSafeSettings = Configuration.GetSection(DineSafeSettings.DineSafe);
            
            services.AddOptions();
            services.Configure<DineSafeSettings>(dineSafeSettings);
            services.AddWhereToBiteContext(Configuration);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });
            
            services.AddMemoryCache();
            services.AddHttpClient<IDineSafeClient, DineSafeClient>();
            services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
            services.AddScoped<IDomainMapper, DomainToResponseMapper>();
            
            services.AddSingleton<IDineSafeDataExtractor, DineSafeDataExtractor>(sp =>
            {
                var serviceProvider = services.BuildServiceProvider();
                
                var repository = serviceProvider.GetService<IEstablishmentRepository>();
                var logger = serviceProvider.GetService<ILogger<DineSafeDataExtractor>>();
                var client = serviceProvider.GetService<IDineSafeClient>();
                var cache = serviceProvider.GetService<IMemoryCache>();
                var settings = dineSafeSettings.Get<DineSafeSettings>();
                
                return new DineSafeDataExtractor(repository, Options.Create(settings), logger, client, cache);
            });
            services.AddCustomSwagger();
            services.AddHostedService<DineSafeDataExtractorServiceWorker>();
            services.AddControllers(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                }).AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var pathBase = Configuration["PATH_BASE"];
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Establishments.API V1");
                    c.OAuthClientId("establishmentsswaggerui");
                    c.OAuthAppName("Establishment Swagger UI");
                });
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }
    }
}