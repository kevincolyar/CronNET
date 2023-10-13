using System;
using System.Threading;
using System.Threading.Tasks;

namespace crondotnet
{
    public delegate Task ExecuteCronJob(CancellationToken cancellationToken);

    public interface ICronJob
    {
        Task Execute(DateTime startTime, CancellationToken cancellationToken);
    }

    public class CronJob : ICronJob
    {
        private readonly ICronSchedule _cronSchedule;
        private readonly ExecuteCronJob _threadStart;
        private readonly SemaphoreSlim _semaphore = new(1);

        public CronJob(string schedule, ExecuteCronJob threadStart)
        {
            _cronSchedule = new CronSchedule(schedule);
            _threadStart = threadStart;
        }

        public async Task Execute(DateTime startTime, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            if (!_cronSchedule.IsTime(startTime))
                return;

            await _threadStart(cancellationToken);
        }
    }
}
