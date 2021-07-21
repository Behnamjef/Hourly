using System;
using System.Linq;
using Hourly.Repeat;

namespace Hourly
{
    public static class TaskManager
    {
        public static ReminderTask GetNewTask()
        {
            var nextTaskIndex = ++Prefs.UserProfile.LastTaskIndex;
            return new ReminderTask {TaskIndex = nextTaskIndex};
        }

        public static ReminderTask TaskCompleteStateChanged(ReminderTask task)
        {
            // The done state change on cell class
            if (!task.IsDone || task.RepeatData == null || task.RepeatData.RepeatType == RepeatType.Never) return null;

            var childTask = Prefs.UserProfile.AllReminderTasks.Find(t => t.ParentIndex == task.TaskIndex);
            if (childTask != null) return null;

            childTask = new ReminderTask
            {
                ParentIndex = task.TaskIndex,
                TaskIndex = GetNewTask().TaskIndex,
                NotifTime = GetNextRepeatTime(task),
                IsDone = false,
                
                Title = task.Title,
                Note = task.Note,
                GroupIndex = task.GroupIndex,
                RepeatData = task.RepeatData,
            };

            AddOrUpdateTask(childTask);
            return childTask;
        }

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

        private static DateTime? GetNextRepeatTime(ReminderTask task)
        {
            return task.RepeatData.RepeatType switch
            {
                RepeatType.Never => null,
                RepeatType.Daily => task.NotifTime?.AddDays(1),
                RepeatType.Weekly => task.NotifTime?.AddDays(7),
                RepeatType.Monthly => task.NotifTime?.AddMonths(1),
                RepeatType.Yearly => task.NotifTime?.AddYears(1),
                RepeatType.Custom => null,
                _ => null
            };
        }
    }
}