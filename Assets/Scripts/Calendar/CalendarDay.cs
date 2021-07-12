using System;

namespace Hourly.Calendar
{
    public class CalendarDay
    {
        public DateTime DateTime { get; }
        public int Day => DateTime.Day;
        public int Month => DateTime.Month;
        public int Year => DateTime.Year;
        public int DayOfYear => DateTime.DayOfYear;
        public DayOfWeek DayOfWeek => DateTime.DayOfWeek;
        public string MonthName => DateTime.ToString("MMMM");
        public int Hour => DateTime.Hour;
        public int Minute => DateTime.Minute;

        public CalendarDay(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public int GetDayOfWeekIndex()
        {
            return DayOfWeek switch
            {
                DayOfWeek.Saturday => 0,
                DayOfWeek.Sunday => 1,
                DayOfWeek.Monday => 2,
                DayOfWeek.Tuesday => 3,
                DayOfWeek.Wednesday => 4,
                DayOfWeek.Thursday => 5,
                DayOfWeek.Friday => 6,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}