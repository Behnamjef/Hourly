using System;
using System.Threading.Tasks;
using Hourly.Calendar;
using Hourly.Time;
using Hourly.ToDo;
using TMPro;
using UnityEngine;

namespace Hourly.UI
{
    public class EditTaskPopup : Popup
    {
        [SerializeField] private TMP_InputField _titleInputField;
        [SerializeField] private TMP_InputField _noteInputField;

        private DateSelector DateSelector => GetCachedComponentInChildren<DateSelector>();

        private EditTaskRemindMeSection EditTaskRemindMeSection =>
            GetCachedComponentInChildren<EditTaskRemindMeSection>();

        private Data _data;

        private ToDoTask _reminderTask;

        public override async Task Init(IPopupData data)
        {
            await base.Init(data);
            _data = data as Data;
            DateSelector.OnDateSelected = OnDateSelected;
            _reminderTask = _data?.ReminderTask ?? new ToDoTask();
            FillContent();
        }

        private void FillContent()
        {
            _titleInputField.text = _reminderTask.Title;
            _noteInputField.text = _reminderTask.Note;
            EditTaskRemindMeSection.Init(_reminderTask);
        }

        private void OnDateSelected(DateTime reminderDate)
        {
            _reminderTask.ReminderNotificationTime ??= new ToDoTaskRemindMeData();
            
            _reminderTask.ReminderNotificationTime.NotificationTime = reminderDate;
            EditTaskRemindMeSection.SetDate(reminderDate);
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
            _reminderTask.ReminderNotificationTime = EditTaskRemindMeSection.GetSelectedNotificationTime();
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
            DateSelector.ShowCalendar(EditTaskRemindMeSection.GetSelectedNotificationTime()?.NotificationTime ??
                                      TimeProvider.GetCurrentTime());
        }

        public class Data : IPopupData
        {
            public Action<ToDoTask> OnFinishClicked;
            public Action<ToDoTask> OnDeleteClicked;
            public ToDoTask ReminderTask;
        }
    }
}