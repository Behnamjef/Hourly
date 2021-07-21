using Hourly.Repeat;

namespace Hourly
{
    public class TaskRepeatingData
    {
        public RepeatType RepeatType;

        public TaskRepeatingData(RepeatType repeatType)
        {
            RepeatType = repeatType;
        }
    }
}