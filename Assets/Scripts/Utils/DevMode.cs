using Hourly.Profile;
using Newtonsoft.Json;
using UnityEngine;

namespace Hourly.Utils
{
    public class DevMode : CommonBehaviour
    {
        public void CopyToClipboard()
        {
            GUIUtility.systemCopyBuffer = JsonConvert.SerializeObject(Prefs.UserProfile);
        }

        public void PasteFromClipboard()
        {
            PlayerPrefs.SetString("UserProfile", GUIUtility.systemCopyBuffer);
            Prefs.LoadProfile();
        }
    }
}