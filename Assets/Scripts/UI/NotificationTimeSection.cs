using System;
using Hourly.Repeat;
using Hourly.Time;
using UnityEngine;
using UnityEngine.UI;

namespace Hourly.UI
{
    public class NotificationTimeSection : CommonUiBehaviour
    {
        private Toggle Toggle => GetCachedComponentInChildren<Toggle>();
        [SerializeField] private CommonUiBehaviour TimeSection;
        [SerializeField] private RepeatSection RepeatSection;
        private CustomText ReminderTimeText => TimeSection.GetCachedComponentInChildren<CustomText>();

        private ReminderNotificationData currentNotifData;

        public void Init(ReminderTask reminderTask)
        {
            Toggle.onValueChanged.AddListener(ToggleValueChanged);
            Toggle.isOn = reminderTask.ReminderNotificationTime?.NotificationTime != null;
            ActivateTimeSection(Toggle.isOn);

            if (reminderTask.ReminderNotificationTime?.NotificationTime == null)
            {
                return;
            }

            SetDate((DateTime) reminderTask.ReminderNotificationTime?.NotificationTime);
            SetRepeat(reminderTask.ReminderNotificationTime.RepeatData.RepeatType);
        }

        private void ToggleValueChanged(bool isOn)
        {
            ActivateTimeSection(isOn);
            if (isOn)
            {
                SetDate(TimeProvider.GetCurrentTime());
                SetRepeat(RepeatType.Never);
            }
            else
                currentNotifData = null;
        }

        public void SetDate(DateTime notifyTime)
        {
            currentNotifData ??= new ReminderNotificationData();
            
            currentNotifData.NotificationTime = notifyTime;
            ReminderTimeText.text = notifyTime.ToString(TimeProvider.GENRAL_TIME_FORMAT);
        }

        public void SetRepeat(RepeatType repeatType)
        {
            RepeatSection.Init(new RepeatSection.FillData
            {
                RepeatType = repeatType,
                OnNewTypeSelected = OnRepeatTypeChanged
            });
        }

        private void OnRepeatTypeChanged(RepeatType repeatType)
        {
            currentNotifData.RepeatData = new ReminderRepeatingData(repeatType);
        }

        private void ActivateTimeSection(bool isActive)
        {
            TimeSection.SetActive(isActive);
            RepeatSection.SetActive(isActive);
        }

        private void OnDisable()
        {
            Toggle.onValueChanged.RemoveAllListeners();
        }

        public ReminderNotificationData GetSelectedNotificationTime()
        {
            return currentNotifData;
        }
    }
}