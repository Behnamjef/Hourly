using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Hourly.UI
{
    public class ReminderTextGroup : CommonUiBehaviour
    {
        [SerializeField] private CustomText _titleText;
        [SerializeField] private CustomText _noteText;
        [SerializeField] private CustomText _timeText;

        public async Task FillTexts(ReminderTask task)
        {
            var time = task.Time?.ToString("g");
            _titleText.text = task.Title;
            _noteText.text = task.Note;
            _timeText.text = time != null ? " -> " + time : "";

            await RebuildAllRects();
        }

        public float GetHeight()
        {
            return RectTransform.sizeDelta.y;
        }
    }
}