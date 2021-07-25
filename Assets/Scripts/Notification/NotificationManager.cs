using System;
using System.Linq;
using Hourly.Time;
using Hourly.ToDo;
using Hourly.Utils;
using Newtonsoft.Json;
using Unity.Notifications.Android;
using UnityEngine;
using Logger = Hourly.Utils.Logger;

namespace Hourly.Notification
{
    public static class NotificationManager
    {
        private const string REMINDER_CHANNEL_NAME = "Reminders";
        private const string SMALL_ICON_NAME = "small_icon";
        private const string LARGE_ICON_NAME = "large_icon";

        public static void SetupNotifications(ToDoTask[] allTasks)
        {
            CheckOpenedNotification();

            if (allTasks.IsNullOrEmpty()) return;

            allTasks = allTasks.Where(t =>
                t.RemindMeData?.NotificationTime != null &&
                t.RemindMeData.NotificationTime > TimeProvider.GetCurrentTime()).ToArray();

            var channel = new AndroidNotificationChannel()
            {
                Id = REMINDER_CHANNEL_NAME,
                Name = REMINDER_CHANNEL_NAME,
                Description = "Generic notifications",
                CanShowBadge = true,
                Importance = Importance.High,
                LockScreenVisibility = LockScreenVisibility.Public
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);


            foreach (var task in allTasks)
            {
                if (task.IsDone ||
                    task.RemindMeData?.NotificationTime == null)
                {
                    CancelThisNotification(task.TaskIndex);
                    continue;
                }

                ScheduleNotification(task);
            }
        }

        private static void ScheduleNotification(ToDoTask task)
        {
            var status = AndroidNotificationCenter.CheckScheduledNotificationStatus(task.TaskIndex);

            var notification = new AndroidNotification
            {
                Title = task.Title,
                FireTime = task.RemindMeData?.NotificationTime ?? new DateTime(),
                ShowTimestamp = true,
                SmallIcon = SMALL_ICON_NAME,
                ShouldAutoCancel = false
            };

            switch (status)
            {
                case NotificationStatus.Delivered:
                    CancelThisNotification(task.TaskIndex);
                    AndroidNotificationCenter.SendNotificationWithExplicitID(notification, REMINDER_CHANNEL_NAME,
                        task.TaskIndex);
                    break;
                case NotificationStatus.Scheduled:
                    AndroidNotificationCenter.UpdateScheduledNotification(task.TaskIndex, notification,
                        REMINDER_CHANNEL_NAME);
                    break;
                default:
                    AndroidNotificationCenter.SendNotificationWithExplicitID(notification, REMINDER_CHANNEL_NAME,
                        task.TaskIndex);
                    break;
            }
        }

        public static void CancelThisNotification(int index)
        {
            var status = AndroidNotificationCenter.CheckScheduledNotificationStatus(index);
            Logger.Log($"Cancel {index} notif. last status: {status}");
            AndroidNotificationCenter.CancelNotification(index);
        }

        public static void CheckOpenedNotification()
        {
            var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
            if (notificationIntentData == null) return;
            Logger.Log($"Clicked notification is {notificationIntentData.Id} notif.");
            CancelThisNotification(notificationIntentData.Id);
        }
    }
}