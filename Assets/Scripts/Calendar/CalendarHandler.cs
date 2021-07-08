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

        public void ShowCalendar(DateTime processDate)
        {
            SetActive(true);
            SetTexts(processDate);
            FillCells(processDate);
        }

        private void SetTexts(DateTime processDate)
        {
            var calendarDay = new CalendarDay(processDate);
            _yearText.text = calendarDay.Year.ToString();
            _monthText.text = calendarDay.MonthName;
        }

        private void FillCells(DateTime processDate)
        {
            CreateCells();
            var allDays = Calendar.GetCalendarDays(processDate);

            var index = 0;
            foreach (var day in allDays)
            {
                var cell = _calendarDayCells[index];
                var isToday = processDate.DayOfYear == day.DayOfYear;
                cell.Init(day, isToday);
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
    }
}