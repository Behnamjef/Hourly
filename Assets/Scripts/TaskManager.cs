using System.Linq;

namespace Hourly
{
    public static class TaskManager
    {
        public static void AddOrUpdateTask(ReminderTask task)
        {
            RemoveTask(task.TaskIndex);
            Prefs.AllReminderTasks = Prefs.AllReminderTasks.Append(task).ToList();
        }

        public static void RemoveTask(int index)
        {
            var allTasks = Prefs.AllReminderTasks;
            var currentTask = allTasks?.Find(t => t.TaskIndex == index);
            if (currentTask != null)
                allTasks.Remove(currentTask);
            
            Prefs.AllReminderTasks = allTasks;
        }
    }
}