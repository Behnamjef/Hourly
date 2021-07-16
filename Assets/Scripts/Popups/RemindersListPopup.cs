using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public override async Task Init(IPopupData data)
        {
            await base.Init(data);
            
            ClearList();
            _reminderTaskCells = new List<ReminderTaskCell>();

            var allTask = (data as Data)?.AllTasks;
            if (allTask.IsNullOrEmpty())
                return;

            allTask.Sort((t1, t2) =>
            {
                if (t1?.Time == null)
                    return 1;
                if (t2?.Time == null)
                    return -1;
                return (int) t1.Time?.CompareTo(t2.Time);
            });
            foreach (var t in allTask)
            {
                await CreateTask(t);
            }

            await Task.Delay(300);
            await RebuildAllRects();
        }

        public void ClearList()
        {
            if (_reminderTaskCells.IsNullOrEmpty()) return;

            for (int i = _reminderTaskCells.Count - 1; i >= 0; i--)
            {
                Destroy(_reminderTaskCells[i].gameObject);
            }

            _reminderTaskCells.Clear();
        }

        public async void AddNewTask(ReminderTask task)
        {
            await CreateTask(task);
        }

        private async Task CreateTask(ReminderTask task)
        {
            if (task.IsDone) 
                return;
            
            var t = Instantiate(reminderTaskPrefab, Contents);
            t.Init(new CellData
            {
                Reminder = task, 
                OnTaskClicked = rt => MainManager.Instance.EditThisTask(rt),
                OnTaskComplete = rt => MainManager.Instance.AddOrUpdateTask(rt)
            });
            
            _reminderTaskCells.Add(t);
        }

        public class Data : IPopupData
        {
            public List<ReminderTask> AllTasks;
        }

        // private void OnGUI()
        // {
        //     if (GUI.Button(new Rect(0, 0, 100, 100),""))
        //     {
        //         RebuildAllRects();
        //
        //     }
        // }
    }
}