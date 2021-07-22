using System;
using System.Threading.Tasks;
using Hourly.Time;
using Hourly.ToDo;
using Hourly.Utils;
using TMPro;
using UnityEngine;

namespace Hourly.UI
{
    public class ReminderTextGroup : CommonUiBehaviour
    {
        [Header("Texts")] [SerializeField] private CustomText _titleText;
        [SerializeField] private CustomText _noteText;
        [SerializeField] private CustomText _timeText;

        [Header("Colors")] [SerializeField] private Color _normalTimeColor;
        [SerializeField] private Color _passedTimeColor;

        public async Task FillTexts(ToDoTask task)
        {
            var time = task.RemindMeData?.NotificationTime;
            var timeText = time != null ? TimeProvider.GetDateFriendlyName((DateTime) time) : "";
            var repeat = task.RemindMeData?.RepeatType ?? RepeatType.Never;
            var repeatText = repeat != RepeatType.Never ? ", " + repeat : "";

            _timeText.text = !timeText.IsNullOrEmpty() ? timeText + repeatText : "";
            _titleText.text = task.Title;
            _noteText.text = task.Note;

            _timeText.color = time != null && time > TimeProvider.GetCurrentTime()
                ? _normalTimeColor
                : _passedTimeColor;
        }

        public async Task ChangeState(bool isDone)
        {
            _titleText.fontStyle = isDone ? FontStyles.Strikethrough : FontStyles.Normal;
            _noteText.fontStyle = isDone ? FontStyles.Strikethrough : FontStyles.Normal;
            _timeText.fontStyle = isDone ? FontStyles.Strikethrough : FontStyles.Normal;
        }
    }
}