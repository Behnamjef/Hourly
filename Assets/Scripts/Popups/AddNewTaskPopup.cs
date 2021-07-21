using System;
using System.Linq;
using System.Threading.Tasks;
using Hourly.Calendar;
using Hourly.Repeat;
using Hourly.Time;
using TMPro;
using UnityEngine;

namespace Hourly.UI
{
    public class AddNewTaskPopup : Popup
    {
        [SerializeField] private TMP_InputField _titleInputField;
        [SerializeField] private TMP_InputField _noteInputField;

        private DateSelector DateSelector => GetCachedComponentInChildren<DateSelector>();

        private NotificationTimeSection NotificationTimeSection =>
            GetCachedComponentInChildren<NotificationTimeSection>();

        private Data _data;

        private ReminderTask _reminderTask;

        public override async Task Init(IPopupData data)
        {
            await base.Init(data);
            _data = data as Data;
            DateSelector.OnDateSelected = OnDateSelected;
            _reminderTask = _data?.ReminderTask ?? new ReminderTask();
            FillContent();
        }

        private void FillContent()
        {
            _titleInputField.text = _reminderTask.Title;
            _noteInputField.text = _reminderTask.Note;
            NotificationTimeSection.Init(_reminderTask);
        }

        private void OnDateSelected(DateTime reminderDate)
        {
            _reminderTask.ReminderNotificationTime ??= new ReminderNotificationData();
            
            _reminderTask.ReminderNotificationTime.NotificationTime = reminderDate;
            NotificationTimeSection.SetDate(reminderDate);
        }

        protected override async void OnShow()
        {
            base.OnShow();
            _titleInputField.Select();
            await RebuildAllRects();
        }

        protected override void OnHide()
        {
            base.OnHide();
            _titleInputField.text = "";
            _noteInputField.text = "";
        }

        public void DoneEditing()
        {
            _reminderTask.Title = _titleInputField.text;
            _reminderTask.Note = _noteInputField.text;
            _reminderTask.ReminderNotificationTime = NotificationTimeSection.GetSelectedNotificationTime();
            _data.OnFinishClicked?.Invoke(_reminderTask);

            Close();
        }

        public void DeleteTask()
        {
            _data.OnDeleteClicked?.Invoke(_reminderTask);

            Close();
        }

        public void ShowCalendar()
        {
            DateSelector.ShowCalendar(NotificationTimeSection.GetSelectedNotificationTime()?.NotificationTime ??
                                      TimeProvider.GetCurrentTime());
        }

        public class Data : IPopupData
        {
            public Action<ReminderTask> OnFinishClicked;
            public Action<ReminderTask> OnDeleteClicked;
            public ReminderTask ReminderTask;
        }
    }
}