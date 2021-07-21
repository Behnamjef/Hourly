using System;
using System.Linq;
using Hourly.Time;
using Unity.Notifications.Android;
using UnityEngine;

namespace Hourly.Notification
{
    public class NotificationManager : SingletonBehaviour<NotificationManager>
    {
        private const string REMINDER_CHANNEL_NAME = "Reminders";

        public void SetupNotifications(ReminderTask[] allTasks)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            if (allTasks.IsNullOrEmpty()) return;

            allTasks = allTasks.Where(t => t.NotifTime != null && t.NotifTime > TimeProvider.GetCurrentTime()).ToArray();

            var channel = new AndroidNotificationChannel()
            {
                Id = REMINDER_CHANNEL_NAME,
                Name = REMINDER_CHANNEL_NAME,
                Description = "Generic notifications",
                CanShowBadge = true,
                
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);


            foreach (var task in allTasks.Where(t => !t.IsDone && t.NotifTime != null))
            {
                var notification = new AndroidNotification
                {
                    Title = task.Title,
                    Text = task.Note,
                    FireTime = task.NotifTime ?? new DateTime()
                };

                AndroidNotificationCenter.SendNotification(notification, task.GroupIndex.ToString());
            }
        }
    }
}