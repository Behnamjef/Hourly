using System;
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
            RemindersListPopup.Init(new RemindersListPopup.Data {AllTasks = Prefs.AllReminderTasks});
            AddNewTaskPopup.Init(new AddNewTaskPopup.Data {OnFinishClicked = OnTaskAdded});
            RemindersListPopup.Show();
            AddNewTaskPopup.Close();
        }

        public void ShowAddNewItemPopup()
        {
            RemindersListPopup.Close();
            AddNewTaskPopup.Show();
        }

        public void ShowAddNewItemPopup(ReminderTask reminderTask)
        {
            ShowAddNewItemPopup();
            AddNewTaskPopup.Init(new AddNewTaskPopup.Data {OnFinishClicked = OnTaskAdded, ReminderTask = reminderTask});
        }

        private void OnTaskAdded(ReminderTask task)
        {
            RemindersListPopup.Init(new RemindersListPopup.Data {AllTasks = Prefs.AllReminderTasks});
            RemindersListPopup.Show();
            AddNewTaskPopup.Close();
        }
    }
}