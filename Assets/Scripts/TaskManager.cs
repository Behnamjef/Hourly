using System;
using System.Linq;
using Hourly.Profile;
using Hourly.ToDo;

namespace Hourly
{
    public static class TaskManager
    {
        public static ToDoTask GetNewTask()
        {
            var nextTaskIndex = ++Prefs.UserProfile.LastTaskIndex;
            return new ToDoTask {TaskIndex = nextTaskIndex};
        }

        public static ToDoTask TaskCompleteStateChanged(ToDoTask task)
        {
            // The done state change on cell class
            if (!task.IsDone || task.ReminderNotificationTime?.RepeatData == null ||
                task.ReminderNotificationTime.RepeatData.RepeatType == RepeatType.Never) return null;

            var childTask = Prefs.UserProfile.AllReminderTasks.Find(t => t.ParentIndex == task.TaskIndex);
            if (childTask != null) return null;

            var notificationTime = new ToDoTaskRemindMeData
            {
                NotificationTime = GetNextRepeatTime(task),
                RepeatData = task.ReminderNotificationTime?.RepeatData
            };
            
            childTask = new ToDoTask
            {
                ParentIndex = task.TaskIndex,
                TaskIndex = GetNewTask().TaskIndex,
                ReminderNotificationTime = notificationTime,
                IsDone = false,

                Title = task.Title,
                Note = task.Note,
                GroupIndex = task.GroupIndex,
            };

            AddOrUpdateTask(childTask);
            return childTask;
        }

        public static void AddOrUpdateTask(ToDoTask task)
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

        private static DateTime? GetNextRepeatTime(ToDoTask task)
        {
            return task.ReminderNotificationTime.RepeatData.RepeatType switch
            {
                RepeatType.Never => null,
                RepeatType.Daily => task.ReminderNotificationTime.NotificationTime?.AddDays(1),
                RepeatType.Weekly => task.ReminderNotificationTime.NotificationTime?.AddDays(7),
                RepeatType.Monthly => task.ReminderNotificationTime.NotificationTime?.AddMonths(1),
                RepeatType.Yearly => task.ReminderNotificationTime.NotificationTime?.AddYears(1),
                RepeatType.Custom => null,
                _ => null
            };
        }
    }
}