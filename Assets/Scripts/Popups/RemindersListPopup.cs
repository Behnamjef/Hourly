using System;
using System.Collections.Generic;
using System.Linq;
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

            allTask = allTask.Where(t => !t.IsDone).ToList();
            allTask.Sort((t1, t2) =>
            {
                if (t1?.NotifTime == null)
                    return 1;
                if (t2?.NotifTime == null)
                    return -1;
                return (int) t1.NotifTime?.CompareTo(t2.NotifTime);
            });
            foreach (var t in allTask)
            {
                await CreateTask(t);
            }
        }

        public async Task AddJustThisTask(ReminderTask task)
        {
            var nextTaskIndex = _reminderTaskCells.FirstOrDefault(t => t.ReminderTask.NotifTime > task.NotifTime)?.transform
                .GetSiblingIndex() ?? _reminderTaskCells.Count;
            var newTask = await CreateTask(task);
            newTask.transform.SetSiblingIndex(nextTaskIndex);
            
            await RebuildAllRects();
        }

        protected override async void OnShow()
        {
            base.OnShow();

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

        private async Task<ReminderTaskCell> CreateTask(ReminderTask task)
        {
            var t = Instantiate(reminderTaskPrefab, Contents);
            t.Init(new CellData
            {
                Reminder = task,
                OnTaskClicked = rt => MainManager.Instance.OpenPanelToEditThisTask(rt),
                OnTaskComplete = rt => MainManager.Instance.OnTaskComplete(rt)
            });

            _reminderTaskCells.Add(t);
            return t;
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