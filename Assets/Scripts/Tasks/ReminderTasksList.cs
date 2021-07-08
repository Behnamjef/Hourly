using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hourly
{
    public class ReminderTasksList : CommonBehaviour
    {
        [SerializeField] private ReminderTaskCell reminderTaskPrefab;
        private VerticalLayoutGroup VerticalLayoutGroup => GetCachedComponentInChildren<VerticalLayoutGroup>();

        private List<ReminderTaskCell> _reminderTaskCells = new List<ReminderTaskCell>();
        
        public void Init(ReminderTask[] allTask)
        {
            ClearList();
            _reminderTaskCells = new List<ReminderTaskCell>();
            
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

            for (int i = _reminderTaskCells.Count - 1; i <= 0; i--)
            {
                Destroy(_reminderTaskCells[i].gameObject);
            }
            
            _reminderTaskCells.Clear();
        }

        public void AddNewTask()
        {
            var cell = CreateTask(new ReminderTask());
            cell.Select();
        }

        private ReminderTaskCell CreateTask(ReminderTask task)
        {
            var t = Instantiate(reminderTaskPrefab, VerticalLayoutGroup.transform);
            t.Init(task);
            _reminderTaskCells.Add(t);
            return t;
        }

        public List<ReminderTaskCell> GetAllTaskCells()
        {
            return _reminderTaskCells;
        }
    }
}