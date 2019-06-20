using System;
using System.Threading;
using System.Threading.Tasks;

namespace CronNET.Interfaces
{
    public interface ICronJob
    {
        string Name { get; }
        Task ExecuteAsync(DateTime dateTime, CancellationToken cancellationToken);
    }
}