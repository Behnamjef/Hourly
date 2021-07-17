using System;
using System.Linq;
using System.Threading.Tasks;
using Hourly.Calendar;
using Hourly.Repeat;
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
        private RepeatSection RepeatSection => GetCachedComponentInChildren<RepeatSection>();

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
            _reminderTimeText.text = _reminderTask.Time?.ToString("g");
            RepeatSection.Init(new RepeatSection.FillData{RepeatingData = _reminderTask.RepeatData,OnNewTypeSelected =
            {
                // ToDo: Update time here!
            }});
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
            DateSelector.ShowCalendar(DateTime.Now);
        }

        public class Data : IPopupData
        {
            public Action<ReminderTask> OnFinishClicked;
            public Action<ReminderTask> OnDeleteClicked;
            public ReminderTask ReminderTask;
        }
    }
}