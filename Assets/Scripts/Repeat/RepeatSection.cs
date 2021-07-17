using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Hourly.Repeat
{
    public class RepeatSection : CommonUiBehaviour
    {
        private Dropdown Dropdown => GetCachedComponentInChildren<Dropdown>();
        private FillData _data;

        public async Task Init(FillData data)
        {
            _data = data;
            var type = _data.RepeatingData.RepeatType;
            Dropdown.ClearOptions();
            Dropdown.AddOptions(Enum.GetNames(typeof(RepeatType)).ToList());
            Dropdown.value = (int) type;
            Dropdown.onValueChanged.AddListener(value => { _data.OnNewTypeSelected?.Invoke((RepeatType) value); });
        }

        public class FillData
        {
            public TaskRepeatingData RepeatingData;
            public Action<RepeatType> OnNewTypeSelected;
        }
    }
}