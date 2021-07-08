using Hourly.UI;
using TMPro;
using UnityEngine;

namespace Hourly
{
    public class ReminderTaskCell : CommonBehaviour
    {
        private TMP_InputField InputField => GetCachedComponentInChildren<TMP_InputField>();
        private ReminderTask _reminderTask;
        
        public void Init(ReminderTask reminder)
        {
            InputField.text = reminder.Title;
            _reminderTask = reminder;
        }

        public void Select()
        {
            InputField.Select();
        }

        public ReminderTask GetTask()
        {
            _reminderTask.Title = InputField.text;
            return _reminderTask;
        }
    }
}