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
            var newTask = new ReminderTask {TaskIndex = Prefs.AllReminderTasks.Select(t => t.TaskIndex).Max() + 1};
            EditThisTask(newTask);
        }

        public void EditThisTask(ReminderTask reminderTask)
        {
            // Pass the task to popup
            AddNewTaskPopup.Init(new AddNewTaskPopup.Data {OnFinishClicked = OnTaskAdded, ReminderTask = reminderTask});
            RemindersListPopup.Close();
            AddNewTaskPopup.Show();
        }

        private void OnTaskAdded(ReminderTask task)
        {
            // Save task
            var allTasks = Prefs.AllReminderTasks;
            var currentTask = allTasks?.Find(t => t.TaskIndex == task.TaskIndex);
            if (currentTask != null)
                allTasks.Remove(currentTask);
            allTasks = allTasks.Append(task).ToList();
            Prefs.AllReminderTasks = allTasks;

            // Show task list
            ShowAllTaskPopup();
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