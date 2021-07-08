using System;

namespace Hourly.Calendar
{
    public class CalendarDay
    {
        private DateTime _dateTime;
        public int Day => _dateTime.Day;
        public int Month => _dateTime.Month;
        public int Year => _dateTime.Year;
        public int DayOfYear => _dateTime.DayOfYear;
        public DayOfWeek DayOfWeek => _dateTime.DayOfWeek;
        public string MonthName => _dateTime.ToString("MMMM");

        public CalendarDay(DateTime dateTime)
        {
            _dateTime = dateTime;
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