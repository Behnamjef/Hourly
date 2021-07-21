using System;

namespace Hourly.Time
{
    public static class TimeProvider
    {
        public static string GENRAL_TIME_FORMAT = "yyyy/MM/dd, H:mm";
        
        public static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}