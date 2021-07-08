using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Hourly.UI
{
    public class AddNewTaskPopup : Popup
    {
        [SerializeField] private TMP_InputField _titleInputField;
        [SerializeField] private TMP_InputField _noteInputField;
        
        private Data _data;
        
        public override void Init(IPopupData data)
        {
            base.Init(data);
            _data = data as Data;
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
        }

        public void DoneEditing()
        {
            var task = new ReminderTask();
            task.Title = _titleInputField.text;
            task.Note = _noteInputField.text;
            
            Prefs.AllReminderTasks = Prefs.AllReminderTasks.Append(task).ToList();
            _data.OnTaskAdded?.Invoke(task);
            
            Close();
        }

        public class Data : IPopupData
        {
            public Action<ReminderTask> OnTaskAdded;
        }
    }
}