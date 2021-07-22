using System;
using Hourly.Time;
using Hourly.ToDo;
using UnityEngine;
using UnityEngine.UI;

namespace Hourly.UI
{
    public class EditTaskRemindMeSection : CommonUiBehaviour
    {
        private Toggle Toggle => GetCachedComponentInChildren<Toggle>();
        [SerializeField] private CommonUiBehaviour TimeSection;
        [SerializeField] private EditTaskRepeatSection RepeatSection;
        private CustomText ReminderTimeText => TimeSection.GetCachedComponentInChildren<CustomText>();

        private ToDoTaskRemindMeData currentNotifData;

        public void Init(ToDoTask reminderTask)
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
            currentNotifData ??= new ToDoTaskRemindMeData();
            
            currentNotifData.NotificationTime = notifyTime;
            ReminderTimeText.text = notifyTime.ToString(TimeProvider.GENRAL_TIME_FORMAT);
        }

        public void SetRepeat(RepeatType repeatType)
        {
            RepeatSection.Init(new EditTaskRepeatSection.FillData
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

        public ToDoTaskRemindMeData GetSelectedNotificationTime()
        {
            return currentNotifData;
        }
    }
}