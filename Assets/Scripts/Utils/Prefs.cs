using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Hourly
{
    public static class Prefs
    {
        public static ReminderTask[] AllReminderTasks
        {
            set
            {
                var json = JsonConvert.SerializeObject(value);
                PlayerPrefs.SetString("AllTasks", json);
            }
            get
            {
                var json = PlayerPrefs.GetString("AllTasks");
                ReminderTask[] list;
                try
                {
                    list = JsonConvert.DeserializeObject<ReminderTask[]>(json);
                }
                catch (Exception e)
                {
                    Logger.LogError("Cannot save tasks!\n" + e.Message);
                    return null;
                }

                return list;
            }
        }
    }
}