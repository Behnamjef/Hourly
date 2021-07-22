using System;
using System.Linq;
using System.Threading.Tasks;
using Hourly.Utils;
using TMPro;

namespace Hourly.ToDo
{
    public class RepeatSection : CommonUiBehaviour
    {
        private TMP_Dropdown Dropdown => GetCachedComponentInChildren<TMP_Dropdown>();
        private FillData _data;

        public async Task Init(FillData data)
        {
            _data = data;
            
            // Get type of repeat
            var type = _data?.RepeatType ?? RepeatType.Never;

            // If drop down was not filled before
            if (Dropdown.options.IsNullOrEmpty())
            {
                // Fill drop down with
                var options = Enum.GetNames(typeof(RepeatType)).ToList();
                Dropdown.AddOptions(options);
            }

            Dropdown.value = (int) type;
            Dropdown.onValueChanged.AddListener(value => { _data.OnNewTypeSelected?.Invoke((RepeatType) value); });
        }

        public class FillData
        {
            public RepeatType RepeatType;
            public Action<RepeatType> OnNewTypeSelected;
        }
    }
}