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
            if (!task.IsDone || task.RemindMeData == null ||
                task.RemindMeData.RepeatType == RepeatType.Never) return null;

            var childTask = Prefs.UserProfile.AllToDoTasks.Find(t => t.ParentIndex == task.TaskIndex);
            if (childTask != null) return null;

            var notificationTime = new ToDoTaskRemindMeData
            {
                NotificationTime = GetNextRepeatTime(task),
                RepeatType = task.RemindMeData.RepeatType
            };
            
            childTask = new ToDoTask
            {
                ParentIndex = task.TaskIndex,
                TaskIndex = GetNewTask().TaskIndex,
                RemindMeData = notificationTime,
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
            Prefs.UserProfile.AllToDoTasks = Prefs.UserProfile.AllToDoTasks.Append(task).ToList();
        }

        public static void RemoveTask(int index)
        {
            var currentTask = Prefs.UserProfile.AllToDoTasks?.Find(t => t.TaskIndex == index);
            if (currentTask != null)
                Prefs.UserProfile.AllToDoTasks.Remove(currentTask);
        }

        private static DateTime? GetNextRepeatTime(ToDoTask task)
        {
            return task.RemindMeData.RepeatType switch
            {
                RepeatType.Never => null,
                RepeatType.Daily => task.RemindMeData.NotificationTime?.AddDays(1),
                RepeatType.Weekly => task.RemindMeData.NotificationTime?.AddDays(7),
                RepeatType.Monthly => task.RemindMeData.NotificationTime?.AddMonths(1),
                RepeatType.Yearly => task.RemindMeData.NotificationTime?.AddYears(1),
                RepeatType.Custom => null,
                _ => null
            };
        }
    }
}