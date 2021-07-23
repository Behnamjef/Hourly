using System;
using Hourly.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = Hourly.Utils.Logger;

namespace Hourly.UI
{
    public class ToDoGroupPlusCell : CommonUiBehaviour
    {
        private TMP_InputField InputField => GetCachedComponentInChildren<TMP_InputField>();
        private Data _data;

        public void Init(Data data)
        {
            _data = data;
            InputField.onSelect.RemoveAllListeners();
            InputField.onSelect.AddListener(OnPlusClicked);
        }

        private void OnPlusClicked(string arg0)
        {
            InputField.onDeselect.RemoveAllListeners();
            InputField.onDeselect.AddListener(OnSubmit);
            InputField.Select();
        }

        private void OnSubmit(string value)
        {
            if (value.IsNullOrEmpty()) return;
            _data.OnGroupCreated?.Invoke(value);
            InputField.text = "";
        }

        public class Data
        {
            public Action<string> OnGroupCreated;
        }
    }
}