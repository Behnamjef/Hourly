using System;

namespace Hourly.ToDo
{
    public class ToDoTaskRemindMeData
    {
        public DateTime? NotificationTime;
        public RepeatType RepeatType;
    }

    public enum RepeatType
    {
        Never = 0,
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        Yearly = 4,
        Custom = 5
    }
}