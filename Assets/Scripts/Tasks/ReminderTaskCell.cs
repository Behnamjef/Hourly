using Hourly.UI;
using TMPro;
using UnityEngine;

namespace Hourly
{
    public class ReminderTaskCell : CommonBehaviour
    {
        private CustomText TitleText => GetCachedComponentInChildren<CustomText>();
        private ReminderTask _reminderTask;
        
        public void Init(ReminderTask reminder)
        {
            TitleText.text = reminder.Title;
            _reminderTask = reminder;
        }

        public ReminderTask GetTask()
        {
            _reminderTask.Title = TitleText.text;
            return _reminderTask;
        }
    }
}