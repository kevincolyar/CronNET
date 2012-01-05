using System;
using System.Threading;

namespace CronNET
{
    public class CronJob
    {
        private readonly CronSchedule _cron_schedule = new CronSchedule();
        private readonly ThreadStart _thread_start;
        private Thread _thread;

        public CronJob(string schedule, ThreadStart thread_start)
        {
            _cron_schedule = new CronSchedule(schedule);
            _thread_start = thread_start;
            _thread = new Thread(thread_start);
        }

        public void execute(DateTime date_time)
        {
            if (!_cron_schedule.is_time(date_time))
                return;

            if (_thread.ThreadState == ThreadState.Running)
                return;

             _thread = new Thread(_thread_start);
             _thread.Start(); 
        }

        public void abort()
        {
          _thread.Abort();  
        }

    }
}
