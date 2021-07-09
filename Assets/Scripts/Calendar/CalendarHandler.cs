using System;
using System.Collections;
using System.Collections.Generic;
using Hourly;
using Hourly.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Hourly.Calendar
{
    public class CalendarHandler : CommonBehaviour
    {
        [SerializeField] private CustomText _yearText;
        [SerializeField] private CustomText _monthText;
        [SerializeField] private CalendarDayCell _calendarDayCellPrefab;
        private List<CalendarDayCell> _calendarDayCells;
        private GridLayoutGroup _gridLayout => GetCachedComponentInChildren<GridLayoutGroup>();
        private CalendarDay _currentCalendarDay;

        public void ShowCalendar(DateTime processDate)
        {
            _currentCalendarDay = new CalendarDay(processDate);
            SetActive(true);

            SetTexts();
            FillCells();
        }

        private void SetTexts()
        {
            _yearText.text = _currentCalendarDay.Year.ToString();
            _monthText.text = _currentCalendarDay.MonthName;
        }

        private void FillCells()
        {
            CreateCells();
            var allDays = Calendar.GetCalendarDays(_currentCalendarDay);

            var index = 0;
            foreach (var day in allDays)
            {
                var cell = _calendarDayCells[index];
                var isToday = _currentCalendarDay.DayOfYear == day.DayOfYear;
                var isThisMonth = _currentCalendarDay.Month == day.Month;
                cell.Init(day, isToday, isThisMonth);
                index++;
            }
        }

        private void CreateCells()
        {
            if (!_calendarDayCells.IsNullOrEmpty()) return;
            _calendarDayCells = new List<CalendarDayCell>();

            for (var i = 0; i < Calendar.CALENDAR_CAPACITY; i++)
            {
                var cell = Instantiate(_calendarDayCellPrefab, _gridLayout.transform);
                _calendarDayCells.Add(cell);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_gridLayout.GetComponent<RectTransform>());
        }

        public void ShowNextMonth()
        {
            ShowCalendar(_currentCalendarDay.DateTime.AddMonths(1));
        }

        public void ShowPreviousMonth()
        {
            ShowCalendar(_currentCalendarDay.DateTime.AddMonths(-1));
        }
        
        public void ShowNextYear()
        {
            ShowCalendar(_currentCalendarDay.DateTime.AddYears(1));
        }

        public void ShowPreviousYear()
        {
            ShowCalendar(_currentCalendarDay.DateTime.AddYears(-1));
        }
    }
}