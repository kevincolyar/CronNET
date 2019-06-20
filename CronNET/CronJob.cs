using CronNET.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CronNET
{
    public class CronJob : ICronJob
    {
        private ICollection<ICronSchedule> _cronSchedules;
        private Func<Task> _func;

        internal event EventHandler<string> JobExecuted;
        internal event EventHandler<string> JobExecuting;

        public string Name { get; private set; }

        public CronJob(Func<Task> func, string name, params string[] cronPatterns)
        {
            Name = name;
            _func = func;
            _cronSchedules = new List<ICronSchedule>();

            foreach (var item in cronPatterns)
            {
                _cronSchedules.Add(new CronSchedule(item));
            }
        }

        public Task ExecuteAsync(DateTime dateTime, CancellationToken cancellationToken)
        {
            foreach (var cronSchedule in _cronSchedules)
            {
                if (!cronSchedule.IsTime(dateTime))
                    continue;

                JobExecuting?.Invoke(this, Name);

                return Task.Run(_func, cancellationToken).ContinueWith(x => JobExecuted?.Invoke(this, Name));
            }

            return Task.CompletedTask;
        }
    }
}