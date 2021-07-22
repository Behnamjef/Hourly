using System;
using Hourly.UI;
using UnityEngine.UI;

namespace Hourly.ToDo
{
    public class ToDoTaskCell : CommonUiBehaviour
    {
        public ToDoTask ToDoTask => _data.ToDoTask;
        private ReminderTextGroup TextGroup => GetCachedComponentInChildren<ReminderTextGroup>();
        private Toggle Toggle => GetCachedComponentInChildren<Toggle>();

        private CellData _data;

        
        public async void Init(CellData data)
        {
            _data = data;

            Toggle.isOn = _data.ToDoTask.IsDone;
            Toggle.onValueChanged.AddListener(OnValueChanged);
            
            await TextGroup.FillTexts(_data.ToDoTask);
        }
        
        private async void OnValueChanged(bool isDone)
        {
            // ToDo: Play animation or ...
            _data.ToDoTask.IsDone = isDone;
            _data.OnTaskComplete?.Invoke(_data.ToDoTask);
            await TextGroup.ChangeState(isDone);
        }

        public void TaskClicked()
        {
            _data.OnTaskClicked?.Invoke(_data.ToDoTask);
        }
    }

    public class CellData
    {
        public ToDoTask ToDoTask;
        public Action<ToDoTask> OnTaskClicked;
        public Action<ToDoTask> OnTaskComplete;
    }
}