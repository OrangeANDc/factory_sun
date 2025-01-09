using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTool.DelegateService
{
    public delegate void RefreshDelegate();
    public class RefreshTabView
    {
        public event RefreshDelegate OnRefresh;

        public void Refresh()
        {
            if (OnRefresh != null)
                OnRefresh();
        }
    }
}
