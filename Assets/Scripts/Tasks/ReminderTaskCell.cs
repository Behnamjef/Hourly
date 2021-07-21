using System;
using System.Threading.Tasks;
using Hourly.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Hourly
{
    public class ReminderTaskCell : CommonUiBehaviour
    {
        public ReminderTask ReminderTask => _data.Reminder;
        private ReminderTextGroup TextGroup => GetCachedComponentInChildren<ReminderTextGroup>();
        private Toggle Toggle => GetCachedComponentInChildren<Toggle>();

        private CellData _data;

        
        public async void Init(CellData data)
        {
            _data = data;

            Toggle.isOn = _data.Reminder.IsDone;
            Toggle.onValueChanged.AddListener(OnValueChanged);
            
            await TextGroup.FillTexts(_data.Reminder);
        }
        
        private void OnValueChanged(bool isDone)
        {
            // ToDo: Play animation or ...
            _data.Reminder.IsDone = isDone;
            _data.OnTaskComplete?.Invoke(_data.Reminder);
        }

        public void TaskClicked()
        {
            _data.OnTaskClicked?.Invoke(_data.Reminder);
        }
    }

    public class CellData
    {
        public ReminderTask Reminder;
        public Action<ReminderTask> OnTaskClicked;
        public Action<ReminderTask> OnTaskComplete;
    }
}