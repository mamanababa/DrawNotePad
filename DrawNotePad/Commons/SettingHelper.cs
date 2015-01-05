using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;

namespace DrawNotePad.Commons
{
    public static class SettingHelper
    {
        //
        public static readonly string LanguageKey = "LanguageKey";
        private static readonly IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        
        //把语言选择结果保存到独立存储
        //save the result of language check to Isolated storage
        public static void AddOrUpdateValue(string key, object value)
        {
            bool valueChanged = false;

            //LanguageKey不存在的话就更新
            if (settings.Contains(key))
            {
                if (settings[key] != value)
                {
                    settings[key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                settings.Add(key, value);
                valueChanged = true;
            }
            if (valueChanged)
            {
                Save();
            }
        }

        //获取LanguageKey值
        //get the LanguageKey
        public static T GetValueOrDefault<T>(string key, T defaultValue)
        {
            T value;

            if (settings.Contains(key))
            {
                value = (T)settings[key];
            }
            else
            {
                value = defaultValue;
            }
            return value;
        }

        private static void Save()
        {
            settings.Save();
        }
    }
}
