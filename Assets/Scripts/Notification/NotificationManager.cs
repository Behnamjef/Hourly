using System;
using System.Linq;
using Unity.Notifications.Android;
using UnityEngine;

namespace Hourly.Notification
{
    public class NotificationManager : SingletonBehaviour<NotificationManager>
    {
        public void SetupNotifications(ReminderTask[] allTasks)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            if (allTasks.IsNullOrEmpty()) return;

            allTasks = allTasks.Where(t => t.Time != null && t.Time > DateTime.Now).ToArray();
            var allGroups = allTasks.Select(t => t.GroupIndex).Distinct();
            foreach (var group in allGroups)
            {
                var channel = new AndroidNotificationChannel()
                {
                    Id = group.ToString(),
                    Name = group.ToString(),
                    Importance = Importance.High,
                    Description = "Generic notifications",
                };
                AndroidNotificationCenter.RegisterNotificationChannel(channel);
            }

            foreach (var task in allTasks.Where(t => t.Time != null))
            {
                var notification = new AndroidNotification
                {
                    Title = task.Title,
                    Text = task.Note,
                    FireTime = task.Time ?? new DateTime()
                };

                AndroidNotificationCenter.SendNotification(notification, task.GroupIndex.ToString());
            }
            
        }
    }
}