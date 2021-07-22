using System;
using System.Collections.Generic;
using Hourly.Time;
using Hourly.UI;
using Hourly.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Hourly.Calendar
{
    public class DateSelector : Popup
    {
        [SerializeField] private CustomText _yearText;
        [SerializeField] private CustomText _monthText;
        [SerializeField] private CalendarDayCell _calendarDayCellPrefab;
        private List<CalendarDayCell> _calendarDayCells;
        private GridLayoutGroup _gridLayout => GetCachedComponentInChildren<GridLayoutGroup>();
        private CalendarDay _currentCalendarDay;

        private TimeSelector TimeSelector => GetCachedComponentInChildren<TimeSelector>();

        public Action<DateTime> OnDateSelected;

        public void ShowCalendar(DateTime processDate)
        {
            _currentCalendarDay = new CalendarDay(processDate);
            Show();

            SetTexts();
            FillCells();
        }

        private void SetTexts()
        {
            _yearText.text = _currentCalendarDay.Year.ToString();
            _monthText.text = _currentCalendarDay.MonthName;
            TimeSelector.Fill(_currentCalendarDay);
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
                cell.OnNewDateSelected = OnNewDateSelected;
                index++;
            }
        }

        private async void CreateCells()
        {
            if (!_calendarDayCells.IsNullOrEmpty()) return;
            _calendarDayCells = new List<CalendarDayCell>();

            for (var i = 0; i < Calendar.CALENDAR_CAPACITY; i++)
            {
                var cell = Instantiate(_calendarDayCellPrefab, _gridLayout.transform);
                _calendarDayCells.Add(cell);
            }

            await RebuildAllRects();
        }

        public void ShowNextMonth()
        {
            ShowCalendar(GetCurrentSelectedTime().AddMonths(1));
        }

        public void ShowPreviousMonth()
        {
            ShowCalendar(GetCurrentSelectedTime().AddMonths(-1));
        }

        public void ShowNextYear()
        {
            ShowCalendar(GetCurrentSelectedTime().AddYears(1));
        }

        public void ShowPreviousYear()
        {
            ShowCalendar(GetCurrentSelectedTime().AddYears(-1));
        }

        private void OnNewDateSelected(CalendarDay calendarDay)
        {
            _currentCalendarDay = calendarDay;
            var selectedTime = GetCurrentSelectedTime();
            ShowCalendar(selectedTime);
        }

        private DateTime GetCurrentSelectedTime()
        {
            var time = TimeSelector.GetTime();
            var reminderDate = new DateTime(_currentCalendarDay.Year, _currentCalendarDay.Month,
                _currentCalendarDay.Day, time.Hour, time.Minute, 0);
            return reminderDate;
        }

        public void FinishSelectingTime()
        {
            OnDateSelected?.Invoke(GetCurrentSelectedTime());
            Close();
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(DateSelector))]
    public class CalendarHandlerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var calendar = target as DateSelector;
            if (GUILayout.Button("Fill calendar"))
            {
                calendar.ShowCalendar(TimeProvider.GetCurrentTime());
            }
        }
    }
#endif
}