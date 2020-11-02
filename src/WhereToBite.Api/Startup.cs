using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WhereToBite.Api.Extensions;
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
            services.AddMemoryCache();
            services.AddHttpClient<IDineSafeClient, DineSafeClient>();
            services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
            
            services.AddSingleton<IDineSafeDataExtractor, DineSafeDataExtractor>(sp =>
            {
                var repository = sp.GetService<IEstablishmentRepository>();
                var logger = sp.GetService<ILogger<DineSafeDataExtractor>>();
                var client = sp.GetService<IDineSafeClient>();
                var cache = sp.GetService<IMemoryCache>();
                var settings = dineSafeSettings.Get<DineSafeSettings>();
                
                return new DineSafeDataExtractor(repository, Options.Create(settings), logger, client, cache);
            });
            
            services.AddHostedService<DineSafeDataExtractorServiceWorker>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}