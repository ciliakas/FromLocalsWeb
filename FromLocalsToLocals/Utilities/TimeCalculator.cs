using System;
using System.Threading;

namespace FromLocalsToLocals.Web.Utilities
{
    public class TimeCalculator
    {
        public static string CalcRelativeTime(string strDate)
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            const int SecondsPerMinute = 60;
            const int SecondsPerHour = 60 * SecondsPerMinute;
            const int SecondsPerDay = 24 * SecondsPerHour;
            const int SecondsPerMonth = 30 * SecondsPerDay;

            var date = DateTime.Parse(strDate);
            var timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - date.Ticks);
            var delta = Math.Abs(timeSpan.TotalSeconds);

            if (delta < SecondsPerMinute)
            {
                if (currentCulture.Equals("lt"))
                {
                    if (timeSpan.Seconds == 1)
                        return "prieš 1 sekundę";
                    if (timeSpan.Seconds > 10 && timeSpan.Seconds < 20 || timeSpan.Seconds % 10 != 0)
                        return "prieš " + timeSpan.Seconds + " sekundes";
                    if (timeSpan.Seconds % 10 == 0) return "prieš " + timeSpan.Seconds + " sekundžių";
                }

                return timeSpan.Seconds == 1 ? "1 second ago" : timeSpan.Seconds + " seconds ago";
            }

            if (delta < 2 * SecondsPerMinute)
            {
                if (currentCulture.Equals("lt")) return "prieš 1 minutę";
                return "1 minute ago";
            }

            if (delta < SecondsPerHour)
            {
                if (currentCulture.Equals("lt"))
                {
                    if (timeSpan.Minutes > 1 && timeSpan.Minutes < 10 || timeSpan.Minutes % 10 != 0)
                        return "prieš " + timeSpan.Minutes + " minutes";
                    if (timeSpan.Minutes % 10 == 0) return "prieš " + timeSpan + " minučių";
                }

                return timeSpan.Minutes + " minutes ago";
            }

            if (delta < 2 * SecondsPerHour)
            {
                if (currentCulture.Equals("lt")) return "Prieš 1 valandą";
                return "1 hour ago";
            }

            if (delta < SecondsPerDay)
            {
                if (currentCulture.Equals("lt"))
                {
                    if (timeSpan.Hours > 1 && timeSpan.Hours < 10 || timeSpan.Hours % 10 != 0)
                        return "prieš " + timeSpan.Hours + " valandas";
                    if (timeSpan.Hours % 10 == 0) return "prieš " + timeSpan.Hours + " valandų";
                }

                return timeSpan.Hours + " hours ago";
            }

            if (delta < 2 * SecondsPerDay)
            {
                if (currentCulture.Equals("lt")) return "prieš 1 dieną";
                return "1 day ago";
            }

            if (delta < SecondsPerMonth)
            {
                if (currentCulture.Equals("lt"))
                {
                    if (timeSpan.Days > 1 && timeSpan.Days < 10 || timeSpan.Days % 10 != 0)
                        return "prieš " + timeSpan.Days + " dienas";
                    if (timeSpan.Days % 10 == 0) return "prieš " + timeSpan.Days + " dienų";
                }

                return timeSpan.Days + " days ago";
            }

            if (delta < 12 * SecondsPerMonth)
            {
                var months = Convert.ToInt32(Math.Floor((double) timeSpan.Days / 30));
                if (currentCulture.Equals("lt"))
                {
                    if (months == 1)
                        return "prieš 1 mėnesį";
                    if (months > 1 && months < 10)
                        return "prieš " + months + " mėnesius";
                    if (months >= 10) return "prieš " + months + " mėnesių";
                }

                return months <= 1 ? "1 month ago" : months + " months ago";
            }

            var years = Convert.ToInt32(Math.Floor((double) timeSpan.Days / 365));
            if (currentCulture.Equals("lt"))
            {
                if (years <= 1) return "prieš 1 metus";
                if (years > 1 && years < 10 || years > 20 && years % 10 != 0) return "prieš " + years + " metus";
                if (years % 10 == 0 || years >= 10 && years < 20) return "prieš " + years + "metų";
            }

            return years <= 1 ? "1 year ago" : years + " years ago";
        }
    }
}