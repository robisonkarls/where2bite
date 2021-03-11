using System;
using System.Linq;
using System.Threading.Tasks;
using Devart.Data.PostgreSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;
using WhereToBite.Infrastructure;
using Severity = WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Severity;

namespace WhereToBite.Api.Infrastructure
{
    public class WhereToBiteContextSeed
    {
        public async Task SeedAsync(WhereToBiteContext context, IWebHostEnvironment env,
            ILogger<WhereToBiteContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(WhereToBiteContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!context.EstablishmentStatus.Any())
                {
                    await context.EstablishmentStatus.AddAsync(EstablishmentStatus.Pass);
                    await context.EstablishmentStatus.AddAsync(EstablishmentStatus.ConditionalPass);
                    await context.EstablishmentStatus.AddAsync(EstablishmentStatus.Closed);
                    
                    await context.SaveEntitiesAsync();
                }
                
                if (!context.InfractionActions.Any())
                {
                    await context.InfractionActions.AddAsync(InfractionAction.NoticeToComply);
                    await context.InfractionActions.AddAsync(InfractionAction.EducationProvided);
                    await context.InfractionActions.AddAsync(InfractionAction.CorrectedDuringInspection);
                    await context.InfractionActions.AddAsync(InfractionAction.Summons);
                    await context.InfractionActions.AddAsync(InfractionAction.SummonsAndHealthHazardOrder);
                    await context.InfractionActions.AddAsync(InfractionAction.Ticket);
                    await context.InfractionActions.AddAsync(InfractionAction.NotInCompliance);
                    await context.InfractionActions.AddAsync(InfractionAction.SummonsByLaw);
                    await context.InfractionActions.AddAsync(InfractionAction.ClosureOrder);
                    
                    await context.SaveEntitiesAsync();
                }
                
                if (!context.InspectionStatus.Any())
                {
                    await context.InspectionStatus.AddAsync(InspectionStatus.Pass);
                    await context.InspectionStatus.AddAsync(InspectionStatus.ConditionalPass);
                    await context.InspectionStatus.AddAsync(InspectionStatus.Closed);
                    
                    await context.SaveEntitiesAsync();
                }
                
                if (!context.Severities.Any())
                {
                    await context.Severities.AddAsync(Severity.Minor);
                    await context.Severities.AddAsync(Severity.Significant);
                    await context.Severities.AddAsync(Severity.Crucial);
                    await context.Severities.AddAsync(Severity.NotApplicable);
                    
                    await context.SaveEntitiesAsync();
                }
            });
        }

        private static AsyncRetryPolicy CreatePolicy(ILogger logger, string prefix, int retries = 3)
        {
            return Policy.Handle<PgSqlException>()
                .WaitAndRetryAsync(
                    retries,
                    _ => TimeSpan.FromSeconds(5),
                    (exception, _, retry, _) =>
                    {
                        logger.LogWarning(
                            exception, 
                            "[{Prefix}] Exception {ExceptionType} with message {Message} detected on attempt {Retry} of {Retries}", 
                            prefix, 
                            exception.GetType().Name, 
                            exception.Message, 
                            retry, 
                            retries);
                    }
                );
        }
    }
}