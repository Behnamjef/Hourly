using System;
using System.Collections.Generic;
using Hourly.Profile;
using Newtonsoft.Json;
using UnityEngine;

namespace Hourly
{
    public static class Prefs
    {
        public static UserProfile UserProfile
        {
            private set => _userProfile = value;
            get => _userProfile ?? new UserProfile();
        }

        private static UserProfile _userProfile;
        // public static List<ReminderTask> AllReminderTasks
        // {
        //     set
        //     {
        //         var json = JsonConvert.SerializeObject(value);
        //         PlayerPrefs.SetString("AllTasks", json);
        //     }
        //     get
        //     {
        //         var json = PlayerPrefs.GetString("AllTasks");
        //         List<ReminderTask> list = new List<ReminderTask>();
        //         try
        //         {
        //             list = JsonConvert.DeserializeObject<List<ReminderTask>>(json);
        //         }
        //         catch (Exception e)
        //         {
        //             Logger.LogError("Cannot get tasks!\n" + e.Message);
        //         }
        //
        //         return list ?? new List<ReminderTask>();
        //     }
        // }

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