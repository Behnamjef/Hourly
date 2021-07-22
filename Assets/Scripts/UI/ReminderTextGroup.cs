using System.Threading.Tasks;
using Hourly.Time;
using Hourly.ToDo;
using UnityEngine;

namespace Hourly.UI
{
    public class ReminderTextGroup : CommonUiBehaviour
    {
        [SerializeField] private CustomText _titleText;
        [SerializeField] private CustomText _noteText;
        [SerializeField] private CustomText _timeText;

        public async Task FillTexts(ToDoTask task)
        {
            var time = task.RemindMeData?.NotificationTime?.ToString(TimeProvider.GENRAL_TIME_FORMAT);
            _timeText.text = time ?? "";
            _titleText.text = task.Title;
            _noteText.text = task.Note;
        }
    }
}