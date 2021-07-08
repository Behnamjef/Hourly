using System.Linq;
using Hourly.UI;

namespace Hourly
{
    public class MainManager : SingletonBehaviour<MainManager>
    {
        private ReminderTasksList ReminderTasksList => GetCachedComponentInChildren<ReminderTasksList>();
        
        // ToDo: Should be in the navigator class
        private Popup AddNewTaskPopup => GetCachedComponentInChildren<AddNewTaskPopup>();

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            ReminderTasksList.Init(Prefs.AllReminderTasks);
            AddNewTaskPopup.Init(new AddNewTaskPopup.Data{OnFinishClicked = AddNewTask});
        }

        public void ShowAddNewItemPopup()
        {
            AddNewTaskPopup.SetActive(true);
        }

        private void AddNewTask(ReminderTask task)
        {
            Prefs.AllReminderTasks = Prefs.AllReminderTasks.Append(task).ToList();
            ReminderTasksList.Init(Prefs.AllReminderTasks);
        }
    }
}