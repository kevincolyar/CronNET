using System;
using System.Threading;
using System.Threading.Tasks;

namespace CronNET.Interfaces
{
    public interface ICronDaemon
    {
        void Add(CronJob job);
        void Remove(CronJob job);
        void Remove(string name);
        void Clear();
        void Start(CancellationToken cancellationToken);
        Task RunAsync(Func<Task> func, CancellationToken cancellationToken, string name);
        void Stop();

        event EventHandler<string> JobExecuting;
        event EventHandler<string> JobExecuted;
    }
}