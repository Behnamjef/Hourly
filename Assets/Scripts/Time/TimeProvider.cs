using System;

namespace Hourly.Time
{
    public static class TimeProvider
    {
        public static string GENERAL_DATE_FORMAT = "yyyy/MM/dd";
        public static string GENERAL_TIME_FORMAT = "H:mm";
        public static string GENERAL_DATE_TIME_FORMAT = $"{GENERAL_DATE_FORMAT}, {GENERAL_TIME_FORMAT}";
        
        public static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        public static string GetDateFriendlyName(DateTime dateTime)
        {
            var dateName = "";
            if (GetCurrentTime().Date == dateTime.Date)
                dateName = "Today";
            else if (GetCurrentTime().AddDays(1).Date == dateTime.Date)
                dateName = "Tomorrow";
            else if (GetCurrentTime().AddDays(-1).Date == dateTime.Date)
                dateName = "Yesterday";
            else
                dateName = dateTime.ToString(GENERAL_DATE_FORMAT);

            dateName += ", " + dateTime.ToString(GENERAL_TIME_FORMAT);
            return dateName;
        }
    }
}