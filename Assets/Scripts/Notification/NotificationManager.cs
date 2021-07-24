using System;
using System.Linq;
using Hourly.Time;
using Hourly.ToDo;
using Hourly.Utils;
using Unity.Notifications.Android;

namespace Hourly.Notification
{
    public class NotificationManager : SingletonBehaviour<NotificationManager>
    {
        private const string REMINDER_CHANNEL_NAME = "Reminders";
        private const string SMALL_ICON_NAME = "small_icon";
        private const string LARGE_ICON_NAME = "large_icon";

        public void SetupNotifications(ToDoTask[] allTasks)
        {
            AndroidNotificationCenter.CancelAllNotifications();
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


            foreach (var task in allTasks.Where(t => !t.IsDone && t.RemindMeData?.NotificationTime != null))
            {
                var notification = new AndroidNotification
                {
                    Title = task.Title,
                    FireTime = task.RemindMeData?.NotificationTime ?? new DateTime(),
                    ShowTimestamp = true,
                    SmallIcon = SMALL_ICON_NAME,
                    ShouldAutoCancel = false
                };

                AndroidNotificationCenter.SendNotification(notification, REMINDER_CHANNEL_NAME);
            }
        }
    }
}