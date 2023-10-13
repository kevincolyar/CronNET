using crondotnet;
using crondotnet.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CronServiceProverExtensions
    {
        public static IServiceCollection AddCron(this IServiceCollection services, Action<ICronDaemonOptionsBuilder>? optionsAction = null)
        {
            services.AddSingleton<CronDaemonHostedService>();
            services.AddScoped<ICronDaemon, CronDaemon>();

            var cronDaemonOptionsBuilder = new CronDaemonOptionsBuilder();
            optionsAction?.Invoke(cronDaemonOptionsBuilder);
            services.ConfigureOptions(cronDaemonOptionsBuilder.Build());

            return services;
        }
    }

    public interface ICronDaemonOptionsBuilder
    {
        ICronDaemonOptionsBuilder AddJob(string cronExpression, ExecuteCronJob job);

        ICronDaemonOptionsBuilder AddJob<TService>(string cronExpression) where TService : IThinService;
    }

    public class CronDaemonOptionsBuilder : ICronDaemonOptionsBuilder
    {
        private readonly CronDaemonOptions Options = new CronDaemonOptions();

        public ICronDaemonOptionsBuilder AddJob(string cronExpression, ExecuteCronJob job)
        {
            Options.Jobs.Add(new JobOptions
            {
                Expression = cronExpression,
                StaticTask = job
            });

            return this;
        }

        public ICronDaemonOptionsBuilder AddJob<TService>(string cronExpression)
            where TService : IThinService
        {
            Options.Jobs.Add(new JobOptions
            {
                Expression = cronExpression,
                ServiceType = typeof(TService),
            });

            return this;
        }

        internal CronDaemonOptions Build()
        {
            return Options;
        }
    }
}