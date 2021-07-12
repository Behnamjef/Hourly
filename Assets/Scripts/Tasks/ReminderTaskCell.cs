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

        public void Init(ReminderTask reminder, Action<ReminderTask> onTaskClicked)
        {
            var time = reminder.Time?.ToString("g");
            TitleText.text = reminder.Title + (time != null ? " -> " + time : "");
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