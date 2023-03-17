CronNET
---------------------------

CronNET is a simple C# library for running tasks based on a cron schedule.

Cron Schedules
===============

CronNET supports most cron scheduling.  See tests for supported formats.

```
*    *    *    *    *  
┬    ┬    ┬    ┬    ┬
│    │    │    │    │
│    │    │    │    │
│    │    │    │    └───── day of week (0 - 6) (Sunday=0 )
│    │    │    └────────── month (1 - 12)
│    │    └─────────────── day of month (1 - 31)
│    └──────────────────── hour (0 - 23)
└───────────────────────── min (0 - 59)
```

```
  `* * * * *`        Every minute.
  `0 * * * *`        Top of every hour.
  `0,1,2 * * * *`    Every hour at minutes 0, 1, and 2.
  `*/2 * * * *`      Every two minutes.
  `1-55 * * * *`     Every minute through the 55th minute.
  `* 1,10,20 * * *`  Every 1st, 10th, and 20th hours.
```

How to install?
===============

You can install this [NuGet package](https://www.nuget.org/packages/Iminetsoft.CronNET)

Console Example
===============

``` c#
using System.Threading;
using CronNET;

namespace CronNETExample.Console
{
    class Program
    {
        private static readonly CronDaemon cron_daemon = new CronDaemon();            

        static void Main(string[] args)
        {
            cron_daemon.add_job(new CronJob("* * * * *", task));
            cron_daemon.start();

            // Wait and sleep forever. Let the cron daemon run.
            while(true) Thread.Sleep(6000);
        }

        static void task()
        {
          Console.WriteLine("Hello, world.")
        }
    }
}
```

Windows Service Example
=======================

``` c#
using System.Threading;
using CronNET;

namespace CronNETExample.WindowsService
{
    public partial class Service : ServiceBase
    {
        private readonly CronDaemon cron_daemon = new CronDaemon();

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            cron_daemon.add_job(new CronJob("*/2 * * * *", task));
            cron_daemon.start();
        }

        protected override void OnStop()
        {
            cron_daemon.stop();
        }

        private void task()
        {
          Console.WriteLine("Hello, world.")
        }
    }
}
```
