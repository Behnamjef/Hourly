using System;
using System.Linq;
using Hourly.Notification;
using Hourly.UI;
using Newtonsoft.Json;

namespace Hourly
{
    public class MainManager : SingletonBehaviour<MainManager>
    {
        // ToDo: Should be in the navigator class
        private Popup AddNewTaskPopup => GetCachedComponentInChildren<AddNewTaskPopup>();
        private Popup RemindersListPopup => GetCachedComponentInChildren<RemindersListPopup>();

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            ShowAllTaskPopup();
        }

        public void AddNewTask()
        {
            // Create an empty task with right index
            var nextTaskIndex = 0;
            var allTaskIndexes = Prefs.AllReminderTasks?.Select(t => t.TaskIndex);
            if (!allTaskIndexes.IsNullOrEmpty())
                nextTaskIndex = allTaskIndexes.Max() + 1;
            var newTask = new ReminderTask {TaskIndex = nextTaskIndex};
            EditThisTask(newTask);
        }

        public void EditThisTask(ReminderTask reminderTask)
        {
            // Pass the task to popup
            AddNewTaskPopup.Init(new AddNewTaskPopup.Data
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
            AddNewTaskPopup.Show();
        }

        public void AddOrUpdateTask(ReminderTask task)
        {
            // Save task
            TaskManager.AddOrUpdateTask(task);
        }

        private void OnTaskDeleted(ReminderTask task)
        {
            // Remove task
            TaskManager.RemoveTask(task.TaskIndex);
        }

        private void ShowAllTaskPopup()
        {
            RemindersListPopup.Init(new RemindersListPopup.Data {AllTasks = Prefs.AllReminderTasks});
            RemindersListPopup.Show();
            AddNewTaskPopup.Close();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetupNotifications();
        }

        private void OnApplicationQuit()
        {
            SetupNotifications();
        }

        private static void SetupNotifications()
        {
            if (Prefs.AllReminderTasks.IsNullOrEmpty())
                return;

            NotificationManager.Instance.SetupNotifications(Prefs.AllReminderTasks.ToArray());
        }
    }
}