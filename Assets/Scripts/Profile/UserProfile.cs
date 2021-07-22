using System.Collections.Generic;
using Hourly.ToDo;

namespace Hourly.Profile
{
    public class UserProfile
    {
        public List<ReminderTask> AllReminderTasks;
        public int LastTaskIndex;

        public UserProfile()
        {
            AllReminderTasks = new List<ReminderTask>();
        }
    }
}