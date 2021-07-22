using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hourly.ToDo;
using Hourly.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Hourly.UI
{
    public class ListOfTasksPopup : Popup
    {
        [SerializeField] private ToDoTaskCell toDoTaskPrefab;
        private ScrollRect Scroll => GetCachedComponentInChildren<ScrollRect>();
        private Transform Contents => Scroll.content;

        private List<ToDoTaskCell> _reminderTaskCells = new List<ToDoTaskCell>();

        public override async Task Init(IPopupData data)
        {
            await base.Init(data);

            ClearList();
            _reminderTaskCells = new List<ToDoTaskCell>();

            var allTask = (data as Data)?.AllTasks;
            if (allTask.IsNullOrEmpty())
                return;

            allTask = allTask.Where(t => !t.IsDone).ToList();
            allTask.Sort((t1, t2) =>
            {
                if (t1?.ReminderNotificationTime?.NotificationTime == null)
                    return 1;
                if (t2?.ReminderNotificationTime?.NotificationTime == null)
                    return -1;
                return (int) t1.ReminderNotificationTime.NotificationTime?.CompareTo(t2.ReminderNotificationTime.NotificationTime);
            });
            foreach (var t in allTask)
            {
                await CreateTask(t);
            }
        }

        public async Task AddJustThisTask(ToDoTask task)
        {
            var nextTaskIndex = _reminderTaskCells.FirstOrDefault(t => t.ReminderTask.ReminderNotificationTime?.NotificationTime > task.ReminderNotificationTime?.NotificationTime)?.transform
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

        public async void AddNewTask(ToDoTask task)
        {
            await CreateTask(task);
        }

        private async Task<ToDoTaskCell> CreateTask(ToDoTask task)
        {
            var t = Instantiate(toDoTaskPrefab, Contents);
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
            public List<ToDoTask> AllTasks;
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