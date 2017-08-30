using System;
using System.Collections.Generic;
using CronNET;
using NUnit.Framework;
using System.Threading;


namespace CronTests
{
    [TestFixture]
    public class CronScheduleTests
    {

        [Test]
        public void is_valid_test()
        {
            var cron_schedule = new CronSchedule();
            Assert.IsTrue(cron_schedule.IsValid("*/2"));
            Assert.IsTrue(cron_schedule.IsValid("* * * * *"));
            Assert.IsTrue(cron_schedule.IsValid("0 * * * *"));
            Assert.IsTrue(cron_schedule.IsValid("0,1,2 * * * *"));
            Assert.IsTrue(cron_schedule.IsValid("*/2 * * * *"));
            Assert.IsTrue(cron_schedule.IsValid("1-4 * * * *"));
            Assert.IsTrue(cron_schedule.IsValid("1-55/3 * * * *"));
            Assert.IsTrue(cron_schedule.IsValid("1,10,20 * * * *"));
            Assert.IsTrue(cron_schedule.IsValid("* 1,10,20 * * *"));
        }

        [Test]
        public static void divided_array_test()
        {
            var cron_schedule = new CronSchedule("*/2");
            List<int> results = cron_schedule.Minutes.GetRange(0,5);//("*/2", 0, 10);
            Assert.AreEqual(results.ToArray(), new int[] { 0, 2, 4, 6, 8 });
        }

        [Test]
        public static void range_array_test()
        {
            var cron_schedule = new CronSchedule("1-10");
            List<int> results = cron_schedule.Minutes.GetRange(0,10);//();
            Assert.AreEqual(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            var cs = new CronSchedule("1-10/3 20-45/4 * * *");
            results = cs.Minutes;
            Assert.AreEqual(results.ToArray(), new int[] { 3, 6, 9 });
        }

        [Test]
        public void wild_array_test()
        {
            var cron_schedule = new CronSchedule("*");
            List<int> results = cron_schedule.Minutes.GetRange(0,10);//("*", 0, 10);
            Assert.AreEqual(results.ToArray(), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }

        [Test]
        public void list_array_test()
        {
            var cron_schedule = new CronSchedule("1,2,3,4,5,6,7,8,9,10");
            List<int> results = cron_schedule.Minutes;
            Assert.AreEqual(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        [Test]
        public void generate_values_divided_test()
        {
            var cron_schedule = new CronSchedule("*/2");
            List<int> results = cron_schedule.Minutes.GetRange(0,5);//(, 0, 10);
            Assert.AreEqual(results.ToArray(), new int[] { 0, 2, 4, 6, 8 });
        }

        [Test]
        public void generate_values_range_test()
        {
            var cron_schedule = new CronSchedule("1-10");
            List<int> results = cron_schedule.Minutes.GetRange(0,10);//(, 0, 10);
            Assert.AreEqual(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        [Test]
        public void generate_minutes_test()
        {
            var cron_schedule = new CronSchedule("1,2,3 * * * *");
            Assert.AreEqual(cron_schedule.Minutes.ToArray(), new int[] { 1, 2, 3 });
        }

        [Test]
        public void generate_hours_test()
        {
            var cron_schedule = new CronSchedule("* 1,2,3 * * *");
            Assert.AreEqual(cron_schedule.Hours.ToArray(), new int[] { 1, 2, 3 });
        }

        [Test]
        public void generate_days_of_month_test()
        {
            var cron_schedule = new CronSchedule("* * 1,2,3 * *");
            Assert.AreEqual(cron_schedule.DaysOfMonth.ToArray(), new int[] { 1, 2, 3 });
        }

        [Test]
        public void generate_months_test()
        {
            var cron_schedule = new CronSchedule("* * * 1,2,3 *");
            Assert.AreEqual(cron_schedule.Months.ToArray(), new int[] { 1, 2, 3 });
        }

        [Test]
        public void generate_days_of_weeks()
        {
            var cron_schedule = new CronSchedule("* * * * 1,2,3 ");
            Assert.AreEqual(cron_schedule.DaysOfWeek.ToArray(), new int[] { 1, 2, 3 });
        }

        [Test]
        public void is_time_minute_test()
        {
            var cron_schedule = new CronSchedule("0 * * * *");
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("8:00 am")));
            Assert.IsFalse(cron_schedule.IsTime(DateTime.Parse("8:01 am")));

            cron_schedule = new CronSchedule("0-10 * * * *");
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("8:00 am")));
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("8:03 am")));

            cron_schedule = new CronSchedule("*/2 * * * *");
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("8:00 am")));
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("8:02 am")));
            Assert.IsFalse(cron_schedule.IsTime(DateTime.Parse("8:03 am")));
        }

        [Test]
        public void is_time_hour_test()
        {
            var cron_schedule = new CronSchedule("* 0 * * *");
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("12:00 am")));

            cron_schedule = new CronSchedule("* 0,12 * * *");
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("12:00 am")));
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("12:00 pm")));
        }

        [Test]
        public void is_time_day_of_month_test()
        {
            var cron_schedule = new CronSchedule("* * 1 * *");
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("2010/08/01")));
        }

        [Test]
        public void is_time_month_test()
        {
            var cron_schedule = new CronSchedule("* * * 1 *");
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("1/1/2008")));

            cron_schedule = new CronSchedule("* * * 12 *");
            Assert.IsFalse(cron_schedule.IsTime(DateTime.Parse("1/1/2008")));

            cron_schedule = new CronSchedule("* * * */3 *");
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("3/1/2008")));
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("6/1/2008")));
        }

        [Test]
        public void is_time_day_of_week_test()
        {
            var cron_schedule = new CronSchedule("* * * * 0");
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("10/12/2008")));
            Assert.IsFalse(cron_schedule.IsTime(DateTime.Parse("10/13/2008")));

            cron_schedule = new CronSchedule("* * * * */2");
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("10/14/2008")));
        }

        [Test]
        public void is_time_test()
        {
            var cron_schedule = new CronSchedule("0 0 12 10 *");
            Assert.IsTrue(cron_schedule.IsTime(DateTime.Parse("12:00:00 am 10/12/2008")));
            Assert.IsFalse(cron_schedule.IsTime(DateTime.Parse("12:01:00 am 10/12/2008")));
        }

        [Test]
        public static void JobMustStartWithinTwoMinutes()
        {
            var ss = new SemaphoreSlim(1);
            ss.Wait(0);
            var d = new CronDaemon();
            d.AddJob("*/1 * * * *", () => { ss.Release(); });
            d.Start();
            Assert.IsTrue(ss.Wait(TimeSpan.FromMinutes(2)));
            ss.Release();
        }
    }
}
