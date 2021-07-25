using System.Linq;
using Hourly.Notification;
using Hourly.Profile;
using Hourly.Time;
using Hourly.ToDo;
using Hourly.UI;
using Hourly.Utils;

namespace Hourly
{
    public class MainManager : SingletonBehaviour<MainManager>
    {
        // ToDo: Should be in the navigator class
        private EditTaskPopup EditTaskPopup => GetCachedComponentInChildren<EditTaskPopup>();
        private ListOfTasksPopup ListOfTasksPopup => GetCachedComponentInChildren<ListOfTasksPopup>();
        private ListOfGroupsPopup ListOfGroupsPopup => GetCachedComponentInChildren<ListOfGroupsPopup>();

        private int _currentGroupIndex;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            Prefs.LoadProfile();
            ShowAllGroupsPopup();
        }

        public void AddNewTask()
        {
            // Create an empty task with right index
            var newTask = TaskManager.GetNewTask();
            newTask.GroupIndex = _currentGroupIndex;
            OpenPanelToEditThisTask(newTask);
        }

        public void OpenPanelToEditThisTask(ToDoTask toDoTask)
        {
            // Pass the task to popup
            EditTaskPopup.Init(new EditTaskPopup.Data
            {
                OnFinishClicked = task =>
                {
                    AddOrUpdateTask(task);
                    ShowToDoListPopup(_currentGroupIndex);
                },
                OnDeleteClicked = task =>
                {
                    OnTaskDeleted(task);
                    ShowToDoListPopup(_currentGroupIndex);
                },
                ToDoTask = toDoTask
            });

            ListOfGroupsPopup.Close();
            ListOfTasksPopup.Close();
            EditTaskPopup.Show();
        }

        public void AddOrUpdateTask(ToDoTask task)
        {
            // Save task
            TaskManager.AddOrUpdateTask(task);
        }

        public async void OnTaskComplete(ToDoTask task)
        {
            // Handle completing a task
            var childTask = TaskManager.TaskCompleteStateChanged(task);
            if (childTask == null) return;

            await ListOfTasksPopup.AddJustThisTask(childTask);
        }

        private void OnTaskDeleted(ToDoTask task)
        {
            // Remove task
            TaskManager.RemoveTask(task.TaskIndex);
            NotificationManager.CancelThisNotification(task.TaskIndex);
        }

        public async void ShowAllGroupsPopup()
        {
            var showData = new ListOfGroupsPopup.Data
            {
                AllGroups = Prefs.UserProfile.AllGroups,
                OnGroupCellClicked = g => ShowToDoListPopup(g.Index)
            };
            await ListOfGroupsPopup.Init(showData);
            ListOfGroupsPopup.Show();
            ListOfTasksPopup.Close();
            EditTaskPopup.Close();
        }

        private async void ShowToDoListPopup(int groupIndex)
        {
            _currentGroupIndex = groupIndex;
            var showData = new ListOfTasksPopup.Data
            {
                AllTasks = Prefs.UserProfile.AllToDoTasks.Where(task =>
                        groupIndex == -1 ||
                        task.GroupIndex == groupIndex || groupIndex == 0 &&
                        task.RemindMeData != null && task.RemindMeData.NotificationTime.IsToday())
                    .ToList(),
                OnTaskCellClicked = OpenPanelToEditThisTask,
                OnTaskCellComplete = OnTaskComplete
            };
            await ListOfTasksPopup.Init(showData);
            ListOfGroupsPopup.Close();
            ListOfTasksPopup.Show();
            EditTaskPopup.Close();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            Prefs.SaveProfile();
            SetupNotifications();
        }

        private void OnApplicationQuit()
        {
            Prefs.SaveProfile();
            SetupNotifications();
        }

        private static void SetupNotifications()
        {
            if (Prefs.UserProfile.AllToDoTasks.IsNullOrEmpty())
                return;

            NotificationManager.SetupNotifications(Prefs.UserProfile.AllToDoTasks.ToArray());
        }
    }
}