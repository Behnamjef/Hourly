using Hourly.UI;
using TMPro;
using UnityEngine;

namespace Hourly
{
    public class ReminderTask : CommonBehaviour
    {
        private TMP_InputField InputField => GetCachedComponentInChildren<TMP_InputField>();

        public void SetTitle(string text)
        {
            InputField.text = text;
        }
    }
}