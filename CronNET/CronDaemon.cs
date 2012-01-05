using System;
using System.Collections.Generic;
using System.Timers;

namespace Cron
{
    public class CronDaemon
    {
        private readonly Timer timer = new Timer(60000);
        private readonly List<CronJob> cron_jobs = new List<CronJob>();

        public CronDaemon()
        {
            timer.Elapsed += timer_elapsed;
        }

        public void add_job(CronJob cron_job)
        {
           cron_jobs.Add(cron_job); 
        }

        public void start()
        {
            timer.Start();
        }

        public void stop()
        {
            timer.Stop();

            foreach (CronJob job in cron_jobs)
                job.abort();
        }

        private void timer_elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (CronJob job in cron_jobs)
                job.execute(DateTime.Now);
        }
    }
}
