using System.Threading.Tasks;
using Hourly.Time;
using UnityEngine;

namespace Hourly.UI
{
    public class ReminderTextGroup : CommonUiBehaviour
    {
        [SerializeField] private CustomText _titleText;
        [SerializeField] private CustomText _noteText;
        [SerializeField] private CustomText _timeText;

        public async Task FillTexts(ReminderTask task)
        {
            var time = task.NotifTime?.ToString(TimeProvider.GENRAL_TIME_FORMAT);
            _titleText.text = task.Title;
            _noteText.text = task.Note;
            _timeText.text = time ?? "Select time";
        }
    }
}