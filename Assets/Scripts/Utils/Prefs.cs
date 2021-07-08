using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Hourly
{
    public static class Prefs
    {
        public static List<ReminderTask> AllReminderTasks
        {
            set
            {
                var json = JsonConvert.SerializeObject(value);
                PlayerPrefs.SetString("AllTasks", json);
            }
            get
            {
                var json = PlayerPrefs.GetString("AllTasks");
                List<ReminderTask> list = new List<ReminderTask>();
                try
                {
                    list = JsonConvert.DeserializeObject<List<ReminderTask>>(json);
                }
                catch (Exception e)
                {
                    Logger.LogError("Cannot get tasks!\n" + e.Message);
                }

                return list ?? new List<ReminderTask>();
            }
        }
    }
}