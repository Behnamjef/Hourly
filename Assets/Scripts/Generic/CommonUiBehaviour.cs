using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Hourly
{
    public class CommonUiBehaviour : CommonBehaviour
    {
        protected RectTransform RectTransform => GetCachedComponentInChildren<RectTransform>();

        protected async Task RebuildAllRects()
        {
            var allRects = GetComponentsInChildren<RectTransform>();
            foreach (var rect in allRects)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
                await Task.Delay(10);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
    }
}