using System;
using Hourly.UI;
using UnityEditor;
using UnityEngine;

namespace Hourly.Calendar
{
    public class TimeSelector : MonoBehaviour
    {
        [SerializeField] private TimeScroll hourScroll;
        [SerializeField] private TimeScroll minutesScroll;

        [Header("Creating object")] [SerializeField]
        private GameObject prefab;

        [SerializeField] private Transform parent;
        [SerializeField] private int cellsCountToCreate;

        public void Fill(CalendarDay calendarDay)
        {
            hourScroll.GoToNumber(calendarDay.Hour);
            minutesScroll.GoToNumber(calendarDay.Minute);
        }

        public CalendarDay GetTime()
        {
            var hour = hourScroll.GetValue();
            var minute = minutesScroll.GetValue();
            var dateTime = new DateTime(1, 1, 1, hour, minute, 0);
            return new CalendarDay(dateTime);
        }

#if UNITY_EDITOR
        public void CreateTimeCells()
        {
            for (int i = 0; i < cellsCountToCreate; i++)
            {
                var hour = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
                var text = hour.GetComponentInChildren<CustomText>();
                text.text = (i).ToString("00");
            }
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TimeSelector))]
    public class TimeSelectorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var time = target as TimeSelector;
            if (GUILayout.Button("Create objects"))
            {
                time.CreateTimeCells();
                EditorUtility.SetDirty(this);
            }
        }
    }
#endif
}