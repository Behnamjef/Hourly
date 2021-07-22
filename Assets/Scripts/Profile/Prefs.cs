using Hourly.Profile;
using Newtonsoft.Json;
using UnityEngine;

namespace Hourly.Profile
{
    public static class Prefs
    {
        public static UserProfile UserProfile
        {
            private set => _userProfile = value;
            get => _userProfile ?? new UserProfile();
        }

        private static UserProfile _userProfile;

        public static void LoadProfile()
        {
            UserProfile = JsonConvert.DeserializeObject<UserProfile>(PlayerPrefs.GetString("UserProfile")) ??
                          new UserProfile();
        }

        public static void SaveProfile()
        {
            if(_userProfile == null) return;
            var json = JsonConvert.SerializeObject(UserProfile);
            PlayerPrefs.SetString("UserProfile", json);
        }
    }
}