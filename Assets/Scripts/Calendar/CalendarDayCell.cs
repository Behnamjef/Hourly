using Hourly.UI;

namespace Hourly.Calendar
{
    public class CalendarDayCell : CommonBehaviour
    {
        private CustomText Text => GetCachedComponentInChildren<CustomText>();
        
        public void Init(CalendarDay calendarDay,bool isToday)
        {
            Text.text = calendarDay.Day.ToString();
        }
    }
}