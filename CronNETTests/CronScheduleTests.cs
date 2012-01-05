using System;
using System.Collections.Generic;
using CronNET;
using MbUnit.Framework;

namespace CronTests
{
    [TestFixture]
    public class CronScheduleTests
    {

        [Test]
        public void is_valid_test()
        {
            CronSchedule cron_schedule = new CronSchedule();
            Assert.IsTrue(cron_schedule.is_valid("* * * * *"));
            Assert.IsTrue(cron_schedule.is_valid("0 * * * *"));
            Assert.IsTrue(cron_schedule.is_valid("0,1,2 * * * *"));
            Assert.IsTrue(cron_schedule.is_valid("*/2 * * * *"));
            Assert.IsTrue(cron_schedule.is_valid("1-4 * * * *"));
            Assert.IsTrue(cron_schedule.is_valid("1-55 * * * *"));
            Assert.IsTrue(cron_schedule.is_valid("1,10,20 * * * *"));
            Assert.IsTrue(cron_schedule.is_valid("* 1,10,20 * * *"));
        }

        [Test]
        public void divided_array_test()
        {
            CronSchedule cron_schedule = new CronSchedule();
            List<int> results = cron_schedule.divided_array("*/2", 0, 10);
            ArrayAssert.AreEqual(results.ToArray(), new int[]{0, 2, 4, 6, 8 } );
        }

        [Test]
        public void range_array_test()
        {
            CronSchedule cron_schedule = new CronSchedule();
            List<int> results = cron_schedule.range_array("1-10");
            ArrayAssert.AreEqual(results.ToArray(), new int[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } );
        }
        [Test]
        public void wild_array_test()
        {
            CronSchedule cron_schedule = new CronSchedule();
            List<int> results = cron_schedule.wild_array("*", 0, 10);
            ArrayAssert.AreEqual(results.ToArray(), new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 } );
        }
        [Test]
        public void list_array_test()
        {
            CronSchedule cron_schedule = new CronSchedule();
            List<int> results = cron_schedule.list_array("1,2,3,4,5,6,7,8,9,10");
            ArrayAssert.AreEqual(results.ToArray(), new int[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } );
        }

        [Test]
        public void generate_values_divided_test()
        {
           CronSchedule cron_schedule = new CronSchedule();
            List<int> results = cron_schedule.generate_values("*/2", 0, 10);
            ArrayAssert.AreEqual(results.ToArray(), new int[]{0, 2, 4, 6, 8 } );
        }

        [Test]
        public void generate_values_range_test()
        {
           CronSchedule cron_schedule = new CronSchedule();
            List<int> results = cron_schedule.generate_values("1-10", 0, 10);
            ArrayAssert.AreEqual(results.ToArray(), new int[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } );
        }

        [Test]
        public void generate_values_wild_test()
        {
            CronSchedule cron_schedule = new CronSchedule();
            List<int> results = cron_schedule.generate_values("*", 0, 10);
            ArrayAssert.AreEqual(results.ToArray(), new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 } );
        }

        [Test]
        public void generate_values_list_test()
        {
            CronSchedule cron_schedule = new CronSchedule();
            List<int> results = cron_schedule.generate_values("1,2,3,4,5,6,7,8,9,10", 0, 10);
            ArrayAssert.AreEqual(results.ToArray(), new int[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } );
        }

        [Test]
        public void generate_minutes_test()
        {
            CronSchedule cron_schedule = new CronSchedule("1,2,3 * * * *");
            ArrayAssert.AreEqual(cron_schedule.minutes.ToArray(), new int[]{ 1, 2, 3  } );
        }

        [Test]
        public void generate_hours_test()
        {
            CronSchedule cron_schedule = new CronSchedule("* 1,2,3 * * *");
            ArrayAssert.AreEqual(cron_schedule.hours.ToArray(), new int[]{ 1, 2, 3  } );
        }

        [Test]
        public void generate_days_of_month_test()
        {
            CronSchedule cron_schedule = new CronSchedule("* * 1,2,3 * *");
            ArrayAssert.AreEqual(cron_schedule.days_of_month.ToArray(), new int[]{ 1, 2, 3  } );
        }

        [Test]
        public void generate_months_test()
        {
            CronSchedule cron_schedule = new CronSchedule("* * * 1,2,3 *");
            ArrayAssert.AreEqual(cron_schedule.months.ToArray(), new int[]{ 1, 2, 3  } );
        }

        [Test]
        public void generate_days_of_weeks()
        {
            CronSchedule cron_schedule = new CronSchedule("* * * * 1,2,3 " );
            ArrayAssert.AreEqual(cron_schedule.days_of_week.ToArray(), new int[]{ 1, 2, 3  } );
        }

        [Test]
        public void is_time_minute_test()
        {
            CronSchedule cron_schedule = new CronSchedule("0 * * * *");
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("8:00 am")));
            Assert.IsFalse(cron_schedule.is_time(DateTime.Parse("8:01 am")));

            cron_schedule = new CronSchedule("0-10 * * * *");
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("8:00 am")));
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("8:03 am")));

            cron_schedule = new CronSchedule("*/2 * * * *");
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("8:00 am")));
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("8:02 am")));
            Assert.IsFalse(cron_schedule.is_time(DateTime.Parse("8:03 am")));
        }

        [Test]
        public void is_time_hour_test()
        {
            CronSchedule cron_schedule = new CronSchedule("* 0 * * *");
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("12:00 am")));

            cron_schedule = new CronSchedule("* 0,12 * * *");
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("12:00 am")));
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("12:00 pm")));
        }

        [Test]
        public void is_time_day_of_month_test()
        {
            CronSchedule cron_schedule = new CronSchedule("* * 1 * *");
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("10/1/08")));
        }

        [Test]
        public void is_time_month_test()
        {
            CronSchedule cron_schedule = new CronSchedule("* * * 1 *");
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("1/1/08")));

            cron_schedule = new CronSchedule("* * * 12 *");
            Assert.IsFalse(cron_schedule.is_time(DateTime.Parse("1/1/08")));

            cron_schedule = new CronSchedule("* * * */3 *");
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("3/1/08")));
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("6/1/08")));
        }

        [Test]
        public void is_time_day_of_week_test()
        {
            CronSchedule cron_schedule = new CronSchedule("* * * * 0");
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("10/12/08")));
            Assert.IsFalse(cron_schedule.is_time(DateTime.Parse("10/13/08")));

            cron_schedule = new CronSchedule("* * * * */2");
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("10/14/08")));
        }

        [Test]
        public void is_time_test()
        {
            CronSchedule cron_schedule = new CronSchedule("0 0 12 10 *");
            Assert.IsTrue(cron_schedule.is_time(DateTime.Parse("12:00:00 am 10/12/08")));
            Assert.IsFalse(cron_schedule.is_time(DateTime.Parse("12:01:00 am 10/12/08")));
        }
    }
}
