using System;
using System.Linq;
using Hourly.Calendar;
using TMPro;
using UnityEngine;

namespace Hourly.UI
{
    public class AddNewTaskPopup : Popup
    {
        [SerializeField] private TMP_InputField _titleInputField;
        [SerializeField] private TMP_InputField _noteInputField;
        [SerializeField] private CustomText _reminderTimeText;

        private DateSelector DateSelector => GetCachedComponentInChildren<DateSelector>();

        private Data _data;

        private ReminderTask _reminderTask;

        public override void Init(IPopupData data)
        {
            base.Init(data);
            _data = data as Data;
            DateSelector.OnDateSelected = OnDateSelected;
            _reminderTask = _data?.ReminderTask ?? new ReminderTask();
            FillContent();
        }

        private void FillContent()
        {
            _titleInputField.text = _reminderTask.Title;
            _noteInputField.text = _reminderTask.Note;
            _reminderTimeText.text = _reminderTask.Time.ToString();
        }

        private void OnDateSelected(DateTime reminderDate)
        {
            _reminderTask.Time = reminderDate;
            _reminderTimeText.text = reminderDate.ToString();
        }

        protected override void OnShow()
        {
            base.OnShow();
            _titleInputField.Select();
        }

        protected override void OnHide()
        {
            base.OnHide();
            _titleInputField.text = "";
            _noteInputField.text = "";
            _reminderTimeText.text = "";
        }

        public void DoneEditing()
        {
            _reminderTask.Title = _titleInputField.text;
            _reminderTask.Note = _noteInputField.text;

            Prefs.AllReminderTasks = Prefs.AllReminderTasks.Append(_reminderTask).ToList();
            _data.OnFinishClicked?.Invoke(_reminderTask);

            Close();
        }

        public void ShowCalendar()
        {
            DateSelector.ShowCalendar(DateTime.Now);
        }

        public class Data : IPopupData
        {
            public Action<ReminderTask> OnFinishClicked;
            public ReminderTask ReminderTask;
        }
    }
}