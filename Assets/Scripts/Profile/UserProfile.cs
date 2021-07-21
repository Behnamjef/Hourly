using System.Collections.Generic;

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