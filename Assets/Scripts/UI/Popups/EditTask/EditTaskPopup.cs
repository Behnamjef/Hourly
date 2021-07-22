using System;
using System.Threading.Tasks;
using Hourly.Calendar;
using Hourly.Time;
using Hourly.ToDo;
using Hourly.Utils;
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

        private ToDoTask _toDoTask;

        public override async Task Init(IPopupData data)
        {
            await base.Init(data);
            
            _data = data as Data;
            
            // On new date selected
            DateSelector.OnDateSelected = OnDateSelected;
            
            // Remind me data
            _toDoTask = _data?.ToDoTask ?? new ToDoTask();
            
            // Fill texts if editing a task
            FillContent();
        }

        // Fill content of popup
        private void FillContent()
        {
            // Fill texts
            _titleInputField.text = _toDoTask.Title;
            _noteInputField.text = _toDoTask.Note;
            
            // Fill remind me section
            EditTaskRemindMeSection.Init(_toDoTask);
        }

        // When a date selected in calendar
        private void OnDateSelected(DateTime reminderDate)
        {
            EditTaskRemindMeSection.SetDate(reminderDate);
        }

        // On show panel
        protected override async void OnShow()
        {
            base.OnShow();
            
            // If title is empty automatically select it to edit
            if(_titleInputField.text.IsNullOrEmpty())
                _titleInputField.Select();
            
            await RebuildAllRects();
        }

        public void DoneEditing()
        {
            _toDoTask.Title = _titleInputField.text;
            _toDoTask.Note = _noteInputField.text;
            _toDoTask.RemindMeData = EditTaskRemindMeSection.CurrentRemindMeData;
            _data.OnFinishClicked?.Invoke(_toDoTask);

            Close();
        }

        public void DeleteTask()
        {
            _data.OnDeleteClicked?.Invoke(_toDoTask);

            Close();
        }

        public void ShowCalendar()
        {
            DateSelector.ShowCalendar(EditTaskRemindMeSection.CurrentRemindMeData?.NotificationTime ??
                                      TimeProvider.GetCurrentTime());
        }

        public class Data : IPopupData
        {
            public Action<ToDoTask> OnFinishClicked;
            public Action<ToDoTask> OnDeleteClicked;
            public ToDoTask ToDoTask;
        }
    }
}