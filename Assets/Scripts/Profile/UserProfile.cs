using System.Collections.Generic;
using Hourly.Group;
using Hourly.ToDo;

namespace Hourly.Profile
{
    public class UserProfile
    {
        public List<ToDoTask> AllToDoTasks;
        public List<ToDoGroup> AllGroups;
        public int LastTaskIndex;
        public int LastGroupIndex;

        public UserProfile()
        {
            AllToDoTasks = new List<ToDoTask>();
            AllGroups = new List<ToDoGroup>();
        }
    }
}