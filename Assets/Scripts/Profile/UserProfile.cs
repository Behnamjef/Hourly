using System.Collections.Generic;
using Hourly.ToDo;

namespace Hourly.Profile
{
    public class UserProfile
    {
        public List<ToDoTask> AllToDoTasks;
        public int LastTaskIndex;

        public UserProfile()
        {
            AllToDoTasks = new List<ToDoTask>();
        }
    }
}