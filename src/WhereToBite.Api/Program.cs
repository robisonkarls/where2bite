using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using WhereToBite.Api.Extensions;
using WhereToBite.Api.Infrastructure;
using WhereToBite.Infrastructure;

namespace WhereToBite.Api
{
    public static class Program
    {
        private static readonly string Namespace = typeof(Program).Namespace;
        private static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        
        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();
            
            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = CreateHostBuilder(configuration, args);

                Log.Information("Applying migrations ({ApplicationContext})...", AppName);
                host.MigrateDbContext<WhereToBiteContext>((context, services) =>
                {
                    var env = services.GetService<IWebHostEnvironment>();
                    var logger = services.GetService<ILogger<WhereToBiteContextSeed>>();

                    new WhereToBiteContextSeed()
                        .SeedAsync(context, env, logger)
                        .Wait();
                });

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                
                host.Run();

                return 0;
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            var logstashUrl = configuration["Serilog:LogstashgUrl"];

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .WriteTo.Http(string.IsNullOrEmpty(logstashUrl)? "http://logstash:8080" : logstashUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();

            return builder.Build();
        }

        private static IWebHost CreateHostBuilder(IConfiguration configuration, string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .CaptureStartupErrors(false)
                .ConfigureKestrel(options =>
                {
                    var (httpPort, httpsPort, grpcPort) = GetDefinedPorts(configuration);

                    options.Listen(IPAddress.Any, httpPort, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    });
                    
                    options.Listen(IPAddress.Any, httpsPort, listenOptions =>
                    {
                        listenOptions.UseHttps(FindMatchingCertificateBySubject("localhost"));
                    });
                    
                    options.Listen(IPAddress.Any, grpcPort, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http2;
                    });
                })
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .Build();
        
        private static (int httpPort, int httpsPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        {
            var httpPort = config.GetValue("HTTP_PORT", 80);
            var httpsPort = config.GetValue("HTTPS_PORT", 443);
            var grpcPort = config.GetValue("GRPC_PORT", 81);
            
            return (httpPort, httpsPort, grpcPort);
        }
        
        private static X509Certificate2 FindMatchingCertificateBySubject(string subjectCommonName)
        {
            using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            var certCollection = store.Certificates;
            var matchingCerts = new X509Certificate2Collection();
    
            foreach (var enumeratedCert in certCollection)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(subjectCommonName, enumeratedCert.GetNameInfo(X509NameType.SimpleName, forIssuer: false))
                    && DateTime.Now < enumeratedCert.NotAfter
                    && DateTime.Now >= enumeratedCert.NotBefore)
                {
                    matchingCerts.Add(enumeratedCert);
                }
            }

            if (matchingCerts.Count == 0)
            {
                throw new Exception($"Could not find a match for a certificate with subject 'CN={subjectCommonName}'.");
            }
        
            return matchingCerts[0];
        }
    }
}