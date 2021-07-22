using System.Threading.Tasks;
using DG.Tweening;
using Hourly.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hourly.Calendar
{
    public class TimeScroll : CommonBehaviour, IEndDragHandler
    {
        private ScrollRect ScrollRect => GetCachedComponentInChildren<ScrollRect>();

        private TimeCell[] TimeCells
        {
            get
            {
                if(_timeCells.IsNullOrEmpty())
                    _timeCells = GetComponentsInChildren<TimeCell>();

                return _timeCells;
            }
        }
        private TimeCell[] _timeCells;
        private static float MIN_SNAP_SPEED = 80;

        private int _correctIndex;

        private void SnapScroll()
        {
            var pace = 1f / (TimeCells.Length - 1);
            var floatCurrentIndex = ScrollRect.normalizedPosition.y / pace;
            _correctIndex = (int) floatCurrentIndex;

            // Should snap to upper object or lower?
            if (floatCurrentIndex - _correctIndex > 0.5f)
                _correctIndex++;

            var correctValue = _correctIndex * pace;

            // Move with animation
            DOTween.To(() => ScrollRect.normalizedPosition.y, SetScroll, correctValue, 0.5f).SetEase(Ease.OutBack);
        }

        public void GoToNumber(int number)
        {
            var lastIndex = TimeCells.Length - 1;
            _correctIndex = lastIndex - number;
            SetScroll((float)_correctIndex / lastIndex);
        }
        
        private void SetScroll(float correctValue)
        {
            ScrollRect.normalizedPosition = new Vector2(0, correctValue);
        }

        public async void OnEndDrag(PointerEventData eventData)
        {
            while (Mathf.Abs(ScrollRect.velocity.y) > MIN_SNAP_SPEED)
            {
                await Task.Delay(100);
            }

            SnapScroll();
        }

        public int GetValue()
        {
            return TimeCells.Length - _correctIndex - 1;
        }
    }
    
}