using System.Collections;
using System.Linq;
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
            StartCoroutine(RebuildUI());
        }

        private IEnumerator RebuildUI()
        {
            yield return new WaitForEndOfFrame();
            var allRects = GetComponentsInChildren<LayoutGroup>().Select(r => r.GetComponent<RectTransform>());
            foreach (var rect in allRects)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            }

            yield return new WaitForEndOfFrame();
            foreach (var rect in allRects)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            }
        }
    }
}