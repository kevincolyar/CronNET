using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace crondotnet.Extensions.DependencyInjection
{
    public class CronDaemonHostedService : IHostedService
    {
        public CronDaemonHostedService(
                IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        private AsyncServiceScope Scope { get; set; }

        private IServiceProvider ServiceProvider { get; }

        private ICronDaemon CronDaemon { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scope = ServiceProvider.CreateAsyncScope();

            CronDaemon = Scope.ServiceProvider.GetRequiredService<ICronDaemon>();
            var options = Scope.ServiceProvider.GetRequiredService<IOptions<CronDaemonOptions>>();

            foreach (var job in options.Value.Jobs)
            {
                if (job.StaticTask != null)
                {
                    CronDaemon.AddJob(job.Expression, job.StaticTask);
                }
                else if (job.ServiceType != null)
                {
                    var service = Scope.ServiceProvider.GetRequiredService(job.ServiceType) as IThinService;
                    if (service != null)
                    {
                        CronDaemon.AddJob(job.Expression, new ThinServiceCronWrapper(service).RunService);
                    }
                }
                else
                {
                    // nothing set up for the run task.
                }
            }

            await CronDaemon.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await CronDaemon.StopAsync();
            await Scope.DisposeAsync();
        }

        private class ThinServiceCronWrapper
        {
            public ThinServiceCronWrapper(IThinService service)
            {
                Service = service;
            }

            private IThinService Service { get; }

            public async Task RunService(CancellationToken cancellationToken)
            {
                await Service.Run(cancellationToken);
            }
        }
    }
}