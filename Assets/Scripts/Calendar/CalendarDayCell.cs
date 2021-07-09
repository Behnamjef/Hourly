using Hourly.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Hourly.Calendar
{
    public class CalendarDayCell : CommonBehaviour
    {
        [SerializeField] private Color normalBgColor;
        [SerializeField] private Color currentDayBgColor;
        [SerializeField] private Color otherMonthBgColor;
        
        [SerializeField] private Color normalTextColor;
        [SerializeField] private Color currentDayTextColor;
        [SerializeField] private Color otherMonthTextColor;
        
        private CustomText Text => GetCachedComponentInChildren<CustomText>();
        private Image Image => GetCachedComponentInChildren<Image>();

        public void Init(CalendarDay calendarDay, bool isToday, bool isThisMonth)
        {
            Text.text = calendarDay.Day.ToString();
            Text.color = isToday ? currentDayTextColor : isThisMonth ? normalTextColor : otherMonthTextColor;
            Image.color = isToday ? currentDayBgColor : isThisMonth ? normalBgColor : otherMonthBgColor;
        }
    }
}