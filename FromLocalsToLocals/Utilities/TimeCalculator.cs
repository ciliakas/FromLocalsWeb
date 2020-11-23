using System;

namespace FromLocalsToLocals.Utilities
{
    public  class TimeCalculator 
    {
        public static string CalcRelativeTime(string strDate)
        {
            const int SecondsPerMinute = 60;
            const int SecondsPerHour = 60 * SecondsPerMinute;
            const int SecondsPerDay = 24 * SecondsPerHour;
            const int SecondsPerMonth = 30 * SecondsPerDay;

            var date = DateTime.Parse(strDate);
            var timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - date.Ticks);
            double delta = Math.Abs(timeSpan.TotalSeconds);

            if (delta < SecondsPerMinute)
            {
                return timeSpan.Seconds == 1 ? "1 second ago" : timeSpan.Seconds + " seconds ago";
            }
            else if (delta < 2 * SecondsPerMinute)
            {
                return "1 minute ago";
            }
            else if (delta < SecondsPerHour)
            {
                return timeSpan.Minutes + " minutes ago";
            }
            else if (delta < 2 * SecondsPerHour)
            {
                return "1 hour ago";
            }
            else if (delta < SecondsPerDay)
            {
                return timeSpan.Hours + " hours ago";
            }
            else if (delta < 2 * SecondsPerDay)
            {
                return "1 day ago";
            }
            else if (delta < SecondsPerMonth)
            {
                return timeSpan.Days + " days ago";
            }
            else if (delta < 12 * SecondsPerMonth)
            {
                int months = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 30));
                return months <= 1 ? "1 month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 365));
                return years <= 1 ? "1 year ago" : years + " years ago";
            }
        }
    }
}
