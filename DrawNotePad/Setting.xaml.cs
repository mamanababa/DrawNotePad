using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DrawNotePad.Commons;

namespace DrawNotePad
{
    public partial class Setting : PhoneApplicationPage
    {
        public Setting()
        {
            InitializeComponent();

            //勾选出上次设置的语言
            //choose the language of last time
            string language=SettingHelper.GetValueOrDefault(SettingHelper.LanguageKey, "en-US");
            if (language == "zh-CN")
            {
                zh_CN.IsChecked = true;
               
            }
            else
            {
                en_US.IsChecked = true;
            }

        }
        

        //根据选择结果，通过SettingHelper类的方法来设置LanguageKey并保存到独立存储
        //set LanguageKey by SettingHelper class
        private void Checked_language(object sender, RoutedEventArgs e)
        {
            if (en_US.IsChecked==true)
            {
                SettingHelper.AddOrUpdateValue(SettingHelper.LanguageKey, "en-US");
            }
            else
            {
                SettingHelper.AddOrUpdateValue(SettingHelper.LanguageKey, "zh-CN");
            }
        }
    }
}