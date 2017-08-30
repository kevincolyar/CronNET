using System;
using System.Collections.Generic;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using CronNET.Interfaces;

namespace CronNET
{
    public class CronDaemon : ICronDaemon
    {
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(10000){AutoReset = true};
        private readonly List<ICronJob> _cronJobs = new List<ICronJob>();
        private DateTime _last= DateTime.Now;

        public CronDaemon()
        {
            _timer.Elapsed += timer_elapsed;
        }

        public void AddJob(string schedule, ThreadStart action)
        {
            var cj = new CronJob(schedule, action);
            _cronJobs.Add(cj);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            Parallel.ForEach(_cronJobs, job => job.Abort());
        }

        private void timer_elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Minute != _last.Minute)
            {
                _last = DateTime.Now;
                Parallel.ForEach(_cronJobs, job => job.Execute(DateTime.Now));
            }
        }
    }
}
