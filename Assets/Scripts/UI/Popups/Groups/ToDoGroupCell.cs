using System;
using System.Threading.Tasks;
using Hourly.Group;
using UnityEngine.UI;

namespace Hourly.UI
{
    public class ToDoGroupCell : CommonUiBehaviour
    {
        private Button Button => GetCachedComponentInChildren<Button>();
        private CustomText TitleText => GetCachedComponentInChildren<CustomText>();
        private Data _data;

        public async Task Init(Data data)
        {
            _data = data;
            TitleText.text = _data.ToDoGroup.Name;
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(OnGroupClicked);
        }

        private void OnGroupClicked()
        {
            _data.OnGroupClicked?.Invoke(_data.ToDoGroup);
        }

        public class Data
        {
            public ToDoGroup ToDoGroup;
            public Action<ToDoGroup> OnGroupClicked;
        }
    }
}