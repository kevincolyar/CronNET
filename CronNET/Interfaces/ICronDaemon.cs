using System.Threading;

namespace CronNET.Interfaces
{
    public interface ICronDaemon
    {
        void AddJob(string schedule, ThreadStart action);
        void Start();
        void Stop();
    }
}