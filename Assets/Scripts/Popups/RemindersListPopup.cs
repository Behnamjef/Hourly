using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hourly.UI
{
    public class RemindersListPopup : Popup
    {
        [SerializeField] private ReminderTaskCell reminderTaskPrefab;
        private ScrollRect Scroll => GetCachedComponentInChildren<ScrollRect>();
        private Transform Contents => Scroll.content;
        
        private List<ReminderTaskCell> _reminderTaskCells = new List<ReminderTaskCell>();
        
        public override void Init(IPopupData data)
        {
            ClearList();
            _reminderTaskCells = new List<ReminderTaskCell>();

            var allTask = (data as Data)?.AllTasks;
            if(allTask.IsNullOrEmpty()) 
                return;
            
            foreach (var t in allTask)
            {
                CreateTask(t);
            }
        }

        public void ClearList()
        {
            if(_reminderTaskCells.IsNullOrEmpty()) return;

            for (int i = _reminderTaskCells.Count - 1; i >= 0; i--)
            {
                Destroy(_reminderTaskCells[i].gameObject);
            }
            
            _reminderTaskCells.Clear();
        }

        public void AddNewTask(ReminderTask task)
        {
            CreateTask(task);
        }

        private ReminderTaskCell CreateTask(ReminderTask task)
        {
            var t = Instantiate(reminderTaskPrefab, Contents);
            t.Init(task,rt => MainManager.Instance.ShowAddNewItemPopup(rt));
            _reminderTaskCells.Add(t);
            return t;
        }

        public List<ReminderTaskCell> GetAllTaskCells()
        {
            return _reminderTaskCells;
        }

        public class Data : IPopupData
        {
            public List<ReminderTask> AllTasks;
        }
    }
}