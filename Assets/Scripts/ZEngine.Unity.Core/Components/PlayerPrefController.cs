using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace ZEngine.Unity.Core.Components
{
    public class EnumDropdown<T> where T : Enum
    {
        private TMP_Dropdown mDropdown;

        private UnityAction<T> mOnValueChange;
        
        public void Init(TMP_Dropdown dropdown, UnityAction<T> onValueChange, T startingOption)
        {
            mDropdown = dropdown;
            mOnValueChange = onValueChange;
            
            var options = new List<TMP_Dropdown.OptionData>();

            int startingIdx = 0;

            int idx = 0;
            foreach (var option in Enum.GetValues(typeof(T)))
            {
                T enumOption = (T) option;
                if (Equals(enumOption, startingOption))
                {
                    startingIdx = idx;
                }
                
                options.Add(new TMP_Dropdown.OptionData(enumOption.ToString()));

                idx++;
            }

            mDropdown.options = options;

            mDropdown.value = startingIdx;
            
            mDropdown.onValueChanged.AddListener(OnValueChanged);

            // SetText(PlayerPrefs.GetFloat(Key, 60f).ToString(CultureInfo.InvariantCulture));
        }
        
        private void OnValueChanged(int optionIdx)
        {
            var optionData = mDropdown.options[optionIdx];

            var value = Enum.Parse(typeof(T), optionData.text);

            mOnValueChange((T) value);
            // Save((int) value);
        }
    }
    
    public class PlayerPrefController : MonoBehaviour
    {
        // public TMP_InputField InputField;

        private string mPreviousText;

        public string Key;
        
        public enum EnumOption
        {
            SLOW = 30,
            NORMAL = 60,
            FAST = 90,
            INSTANT = -1
        }

        public TMP_Dropdown Dropdown;
        private EnumDropdown<EnumOption> mEnumDropdown;

        public void Start()
        {
            int startingOptionValue = PlayerPrefs.GetInt(Key, (int) EnumOption.NORMAL);

            EnumOption option = (EnumOption) startingOptionValue;
            
            mEnumDropdown = new EnumDropdown<EnumOption>();
            mEnumDropdown.Init(Dropdown, OnValueChanged, option);
        }
        
        private void OnValueChanged(EnumOption enumOption)
        {
            Save((int) enumOption);
        }

        public void Save(int value)
        {
            ZBug.Info("PLAYERPREF", $"Updating key [{Key}] to {value}");
            PlayerPrefs.SetInt(Key, value);
            PlayerPrefs.Save();
        }
    }
}