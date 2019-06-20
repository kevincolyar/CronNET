using CronNET.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace CronNET
{
    public class CronDaemon : ICronDaemon
    {
        private readonly System.Timers.Timer _timer;
        private readonly List<ICronJob> _cronJobs;
        private CancellationToken _cancellationToken;

        public event EventHandler<string> JobExecuting;
        public event EventHandler<string> JobExecuted;

        public CronDaemon()
        {
            _cronJobs = new List<ICronJob>();
            _timer = new System.Timers.Timer(1000 * 60);
            _timer.Elapsed += TimerElapsed;
            _timer.Enabled = true;
        }

        public void Add(CronJob job)
        {
            job.JobExecuted += Job_JobExecuted;
            job.JobExecuting += Job_JobExecuting;
            _cronJobs.Add(job);
        }

        private void Job_JobExecuting(object sender, string name)
        {
            JobExecuting?.Invoke(sender, name);
        }

        private void Job_JobExecuted(object sender, string name)
        {
            JobExecuted?.Invoke(sender, name);
        }

        public void Start(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _cancellationToken.Register(Stop);

            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Parallel.ForEach(_cronJobs, job => job.ExecuteAsync(DateTime.Now, _cancellationToken));
        }

        public void Remove(string name)
        {
            var job = _cronJobs.First(x => x.Name == name);

            if (job != null)
                _cronJobs.Remove(job);
        }

        public void Remove(CronJob job)
        {
            _cronJobs.Remove(job);
        }

        public void Clear()
        {
            _cronJobs.Clear();
        }

        public Task RunAsync(Func<Task> func, CancellationToken cancellationToken, string name)
        {
            JobExecuting?.Invoke(this, name);

            return Task.Run(func, cancellationToken).ContinueWith(x => JobExecuted?.Invoke(this, name));
        }
    }
}
