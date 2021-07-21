using System;
using Hourly.Repeat;

namespace Hourly
{
    public class ReminderTask
    {
        public string Title;
        public string Note;
        public int GroupIndex;
        public DateTime? NotifTime;
        public bool IsDone;
        public int TaskIndex;
        public int ParentIndex;
        public TaskRepeatingData RepeatData;
    }
}