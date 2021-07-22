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

        public ToDoTaskRemindMeData CurrentRemindMeData { private set; get; }

        public void Init(ToDoTask toDoTask)
        {
            Toggle.onValueChanged.AddListener(ToggleValueChanged);
            CurrentRemindMeData = toDoTask.RemindMeData;

            Toggle.isOn = CurrentRemindMeData != null;
            ToggleValueChanged(Toggle.isOn);
        }

        private void ToggleValueChanged(bool isOn)
        {
            ActivateTimeSection(isOn);
            if (isOn)
            {
                CurrentRemindMeData ??= new ToDoTaskRemindMeData
                    {NotificationTime = TimeProvider.GetCurrentTime(), RepeatType = RepeatType.Never};

                SetDate((DateTime) CurrentRemindMeData.NotificationTime);
                SetRepeat(CurrentRemindMeData.RepeatType);
            }
            else
                CurrentRemindMeData = null;
        }

        public void SetDate(DateTime notifyTime)
        {
            CurrentRemindMeData.NotificationTime = notifyTime;

            ReminderTimeText.text = notifyTime.ToString(TimeProvider.GENERAL_DATE_TIME_FORMAT);
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
            CurrentRemindMeData.RepeatType = repeatType;
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
    }
}