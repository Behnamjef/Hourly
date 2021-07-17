using System.Linq;

namespace Hourly
{
    public static class TaskManager
    {
        public static void AddOrUpdateTask(ReminderTask task)
        {
            RemoveTask(task.TaskIndex);
            Prefs.UserProfile.AllReminderTasks = Prefs.UserProfile.AllReminderTasks.Append(task).ToList();
        }

        public static void RemoveTask(int index)
        {
            var currentTask = Prefs.UserProfile.AllReminderTasks?.Find(t => t.TaskIndex == index);
            if (currentTask != null)
                Prefs.UserProfile.AllReminderTasks.Remove(currentTask);
        }
    }
}