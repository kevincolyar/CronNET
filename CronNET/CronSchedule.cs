using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CronNET
{
    public class CronSchedule
    {
        #region Readonly Class Members

        readonly static Regex divided_regex = new Regex(@"(\*/\d+)");
        readonly static Regex range_regex = new Regex(@"(\d+\-\d+)");
        readonly static Regex wild_regex = new Regex(@"(\*)");
        readonly static Regex list_regex = new Regex(@"(((\d+,)*\d+)+)");
        readonly static Regex validation_regex = new Regex(divided_regex + "|" + range_regex + "|" + wild_regex + "|" + list_regex);

        #endregion

        #region Private Instance Members
        
        private readonly string _expression;
        public List<int> minutes; 
        public List<int> hours;
        public List<int> days_of_month;
        public List<int> months;
        public List<int> days_of_week;

        #endregion

        #region Public Constructors

        public CronSchedule()
        {
        }

        public CronSchedule(string expressions)
        {
            this._expression = expressions;
            generate();
        }

        #endregion

        #region Public Methods

        public bool is_valid()
        {
            return is_valid(this._expression);
        }

	    public bool is_valid(string expression)
	    {
	        MatchCollection matches = validation_regex.Matches(expression);
	        return matches.Count == 5;
        }

        public bool is_time(DateTime date_time)
        {
            return minutes.Contains(date_time.Minute) &&
                   hours.Contains(date_time.Hour) && 
                   days_of_month.Contains(date_time.Day) &&
                   months.Contains(date_time.Month) &&
                   days_of_week.Contains((int)date_time.DayOfWeek);
        }

        public void generate()
        {
            if(!is_valid()) return;

	        MatchCollection matches = validation_regex.Matches(this._expression);

            generate_minutes(matches[0]);
            generate_hours(matches[1]);
            generate_days_of_month(matches[2]);
            generate_months(matches[3]);
            generate_days_of_weeks(matches[4]);
        }

        public void generate_minutes(Match match)
        {
            this.minutes = generate_values(match.ToString(), 0, 60);
        }

        public void generate_hours(Match match)
        {
            this.hours = generate_values(match.ToString(), 0, 24);
        }

        public void generate_days_of_month(Match match)
        {
            this.days_of_month = generate_values(match.ToString(), 1, 32);
        }

        public void generate_months(Match match)
        {
            this.months = generate_values(match.ToString(), 1, 13);
        }

        public void generate_days_of_weeks(Match match)
        {
            this.days_of_week = generate_values(match.ToString(), 0, 7);
        }

        public List<int> generate_values(string configuration, int start, int max)
        {
            if (divided_regex.IsMatch(configuration)) return divided_array(configuration, start, max);
            if (range_regex.IsMatch(configuration)) return range_array(configuration);
            if (wild_regex.IsMatch(configuration)) return wild_array(configuration, start, max);
            if (list_regex.IsMatch(configuration)) return list_array(configuration);

            return new List<int>();
        }

        public List<int> divided_array(string configuration, int start, int max)
        {
            if(!divided_regex.IsMatch(configuration)) 
                return new List<int>();

            List<int> ret = new List<int>();
            string[] split = configuration.Split("/".ToCharArray());
            int divisor = int.Parse(split[1]);

            for(int i=start; i < max; ++i)
               if(i % divisor == 0)
                   ret.Add(i);

            return ret;
        }

        public List<int> range_array(string configuration)
        {
            if(!range_regex.IsMatch(configuration)) 
                return new List<int>();

            List<int> ret = new List<int>();
            string[] split = configuration.Split("-".ToCharArray());
            int start = int.Parse(split[0]);
            int end = int.Parse(split[1]);

            for (int i = start; i <= end; ++i)
                ret.Add(i);

            return ret;
        }

        public List<int> wild_array(string configuration, int start, int max)
        {
            if(!wild_regex.IsMatch(configuration)) 
                return new List<int>();

            List<int> ret = new List<int>();

            for (int i = start; i < max; ++i)
                ret.Add(i);

            return ret;
        }

        public List<int> list_array(string configuration)
        {
            if(!list_regex.IsMatch(configuration)) 
                return new List<int>();

            List<int> ret = new List<int>();

            string[] split = configuration.Split(",".ToCharArray());

            foreach(string s in split)
                ret.Add(int.Parse(s));

            return ret;
        }

        #endregion
    }
}
