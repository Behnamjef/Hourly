using System.Linq;
using Hourly.Group;
using Hourly.Profile;

namespace Hourly
{
    public static class GroupManager
    {
        public static ToDoGroup CreateNewGroup(string name)
        {
            var nextIndex = ++Prefs.UserProfile.LastGroupIndex;
            var group = new ToDoGroup {Index = nextIndex, Name = name};
            Prefs.UserProfile.AllGroups = Prefs.UserProfile.AllGroups.Append(group).ToList();
            return group;
        }
    }
}