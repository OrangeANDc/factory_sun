using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.Messenager
{
    public delegate void ChangeBurninDataDelegate();
    public delegate void ChangeBurninViewDelegate();
    public class DelegateService
    {
        public event ChangeBurninDataDelegate OnChangeBurninData;
        public event ChangeBurninViewDelegate OnChangeBurninView;

        public void ChangeBurninData()
        {
            if (OnChangeBurninData != null)
                OnChangeBurninData();
        }

        public void ChangeBurninView()
        {
            if (OnChangeBurninView != null)
                OnChangeBurninView();
        }
    }


     

}
