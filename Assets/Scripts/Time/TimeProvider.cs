using System;

namespace Hourly.Time
{
    public static class TimeProvider
    {
        public static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}