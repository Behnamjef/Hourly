using System.Collections.Generic;

namespace Hourly.Calendar
{
    public static class Calendar
    {
        public const int CALENDAR_CAPACITY = 6 * 7;

        public static List<CalendarDay> GetCalendarDays(CalendarDay currentDay)
        {
            var firstDayOfMonth = currentDay.DateTime.AddDays(-(currentDay.Day - 1));
            var firstDayOfMonthCalendarDay = new CalendarDay(firstDayOfMonth);
            var firstDayOfWeekIndex = firstDayOfMonthCalendarDay.GetDayOfWeekIndex();

            var allCalendarDays = new List<CalendarDay>();
            for (int i = 0; i < CALENDAR_CAPACITY; i++)
            {
                var day = new CalendarDay(firstDayOfMonth.AddDays(i - firstDayOfWeekIndex));
                allCalendarDays.Add(day);
            }
            
            return allCalendarDays;
        }
    }
}