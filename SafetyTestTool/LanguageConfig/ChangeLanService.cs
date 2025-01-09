using System;

namespace LanguageConfig
{
    public delegate void ChangeLanDelegate(string lan); 
    public class ChangeLanService
    {
        public event ChangeLanDelegate OnChangeLan;

        public void ChangeLan(string lan)
        {
            if (OnChangeLan != null)
            {
                OnChangeLan(lan);
            }
        }

    }

    public class ChangeInfo
    {
        public static ChangeLanService Service { get; set; } = new ChangeLanService();
    }
}
