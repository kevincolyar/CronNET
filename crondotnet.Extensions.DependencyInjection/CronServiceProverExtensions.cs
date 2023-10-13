using crondotnet;
using crondotnet.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CronServiceProverExtensions
    {
        public static IServiceCollection AddCron(this IServiceCollection services, Action<ICronDaemonOptionsBuilder>? cronBuilder = null)
        {
            services.AddHostedService<CronDaemonHostedService>();
            services.AddScoped<ICronDaemon, CronDaemon>();

            var cronDaemonOptionsBuilder = new CronDaemonOptionsBuilder(services);
            cronBuilder?.Invoke(cronDaemonOptionsBuilder);
            
            services.AddOptions<CronDaemonOptions>()
                .Configure(options => cronDaemonOptionsBuilder.Bind(options));

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
        private readonly List<JobOptions> Jobs = new List<JobOptions>();
        private IServiceCollection services;

        public CronDaemonOptionsBuilder(IServiceCollection services)
        {
            this.services = services;
        }

        public ICronDaemonOptionsBuilder AddJob(string cronExpression, ExecuteCronJob job)
        {
            Jobs.Add(new JobOptions
            {
                Expression = cronExpression,
                StaticTask = job
            });

            return this;
        }

        public ICronDaemonOptionsBuilder AddJob<TService>(string cronExpression)
            where TService : IThinService
        {
            Jobs.Add(new JobOptions
            {
                Expression = cronExpression,
                ServiceType = typeof(TService),
            });

            services.TryAddScoped(typeof(TService));

            return this;
        }

        internal void Bind(CronDaemonOptions options)
        {
            foreach (var job in Jobs)
            {
                options.Jobs.Add(job);
            }
        }
    }
}