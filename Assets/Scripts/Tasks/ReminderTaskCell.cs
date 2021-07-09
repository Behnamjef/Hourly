using System;
using Hourly.UI;
using TMPro;
using UnityEngine;

namespace Hourly
{
    public class ReminderTaskCell : CommonBehaviour
    {
        private CustomText TitleText => GetCachedComponentInChildren<CustomText>();
        private ReminderTask _reminderTask;
        private Action<ReminderTask> _onTaskClicked;
        
        public void Init(ReminderTask reminder,Action<ReminderTask> onTaskClicked)
        {
            TitleText.text = reminder.Title;
            _reminderTask = reminder;
            _onTaskClicked = onTaskClicked;
        }

        public ReminderTask GetTask()
        {
            _reminderTask.Title = TitleText.text;
            return _reminderTask;
        }

        public void TaskClicked()
        {
            _onTaskClicked?.Invoke(_reminderTask);
        }
    }
}