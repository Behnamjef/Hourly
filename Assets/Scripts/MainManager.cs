using Hourly.Notification;
using Hourly.Profile;
using Hourly.ToDo;
using Hourly.UI;
using Hourly.Utils;

namespace Hourly
{
    public class MainManager : SingletonBehaviour<MainManager>
    {
        // ToDo: Should be in the navigator class
        private EditTaskPopup EditTaskPopup => GetCachedComponentInChildren<EditTaskPopup>();
        private ListOfTasksPopup RemindersListPopup => GetCachedComponentInChildren<ListOfTasksPopup>();

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            Prefs.LoadProfile();
            ShowAllTaskPopup();
        }

        public void AddNewTask()
        {
            // Create an empty task with right index
            var newTask = TaskManager.GetNewTask();
            OpenPanelToEditThisTask(newTask);
        }

        public void OpenPanelToEditThisTask(ToDoTask reminderTask)
        {
            // Pass the task to popup
            EditTaskPopup.Init(new EditTaskPopup.Data
            {
                OnFinishClicked = task =>
                {
                    AddOrUpdateTask(task);
                    ShowAllTaskPopup();
                },
                OnDeleteClicked = task =>
                {
                    OnTaskDeleted(task);
                    ShowAllTaskPopup();
                },
                ReminderTask = reminderTask
            });

            RemindersListPopup.Close();
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

            await RemindersListPopup.AddJustThisTask(childTask);
        }

        private void OnTaskDeleted(ToDoTask task)
        {
            // Remove task
            TaskManager.RemoveTask(task.TaskIndex);
        }

        private async void ShowAllTaskPopup()
        {
            await RemindersListPopup.Init(new ListOfTasksPopup.Data {AllTasks = Prefs.UserProfile.AllReminderTasks});
            RemindersListPopup.Show();
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
            if (Prefs.UserProfile.AllReminderTasks.IsNullOrEmpty())
                return;

            NotificationManager.Instance.SetupNotifications(Prefs.UserProfile.AllReminderTasks.ToArray());
        }
    }
}