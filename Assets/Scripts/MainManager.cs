using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hourly
{
    public class MainManager : SingletonBehaviour<MainManager>
    {
        private ReminderTasksList ReminderTasksList => GetCachedComponentInChildren<ReminderTasksList>();

        private void Start()
        {
            ReminderTasksList.Init(Prefs.AllReminderTasks);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 100, 100), "Fill list"))
            {
                ReminderTasksList.Init(Prefs.AllReminderTasks);
            }
        }

        public void AddNewTask()
        {
            ReminderTasksList.AddNewTask();
            Prefs.AllReminderTasks = ReminderTasksList.GetAllTaskCells().Select(t => t.GetTask()).ToArray();
        }
    }
}