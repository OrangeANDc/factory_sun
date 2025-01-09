using LanguageConfig;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SafetyTestTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ChangeInfo.Service = new ChangeLanService();
            ChangeInfo.Service.OnChangeLan += Service_OnChangeLanguage;
        }
         
        private void Service_OnChangeLanguage(string lan)
        {
            ChangeLanguage(lan);
        } 

        public void ChangeLanguage(string lan)
        {
            if (lan == "Chinese")
            {
                var path = "pack://application:,,,/LanguageConfig;component/Chinese.xaml";
                Resources.MergedDictionaries[2].Source = new Uri(path);

            }
            else if (lan == "English")
            {
                var path = "pack://application:,,,/LanguageConfig;component/English.xaml";
                Resources.MergedDictionaries[2].Source = new Uri(path);
            }
            else if (lan == "Deutsch")
            {
                var path = "pack://application:,,,/LanguageConfig;component/Deutsch.xaml";
                Resources.MergedDictionaries[2].Source = new Uri(path);
            }
        }
    }
}
