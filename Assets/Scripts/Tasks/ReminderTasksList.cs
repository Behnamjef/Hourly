using UnityEngine;
using UnityEngine.UI;

namespace Hourly
{
    public class ReminderTasksList : CommonBehaviour
    {
        [SerializeField] private ReminderTask reminderTaskPrefab;
        private VerticalLayoutGroup VerticalLayoutGroup => GetCachedComponentInChildren<VerticalLayoutGroup>();
        
        private void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                var task = Instantiate(reminderTaskPrefab, VerticalLayoutGroup.transform);
                task.name = $"Task {i}";
                task.SetTitle($"This is task {i}");
            }
        }
    }
}