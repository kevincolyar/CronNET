using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace crondotnet
{
    public interface ICronDaemon
    {
        void AddJob(string schedule, ExecuteCronJob action);
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync();
    }

    public class CronDaemon : ICronDaemon
    {
        private readonly PeriodicTimer timer;
        private readonly List<ICronJob> cronJobs = new List<ICronJob>();
        private DateTime _last = DateTime.Now;
        private CancellationTokenSource tokenSource = null;
        private Task timerTask = null;

        public CronDaemon()
        {
            timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
        }

        public void AddJob(string schedule, ExecuteCronJob action)
        {
            var cj = new CronJob(schedule, action);
            cronJobs.Add(cj);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (timerTask == null)
            {
                tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timerTask = InternalStart(tokenSource.Token);
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            tokenSource.Cancel();
            await timerTask;
        }

        private async Task InternalStart(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await timer.WaitForNextTickAsync(cancellationToken);

                if (!cancellationToken.IsCancellationRequested && DateTime.Now.Minute != _last.Minute)
                {
                    _last = DateTime.Now;
                    foreach (ICronJob job in cronJobs)
                        job.Execute(DateTime.Now, cancellationToken);
                }
            }
        }
    }
}
