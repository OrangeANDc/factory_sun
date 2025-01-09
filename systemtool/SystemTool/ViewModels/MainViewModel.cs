using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using SystemTool.StaticSource;
using SystemTool.Views;

namespace SystemTool.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private  IRegionManager _regionManager;
        private Dictionary<string, string> _ViewRegions;
        public MainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            TitleVis = Visibility.Visible;
           

        }

        private Visibility _titleVis;

        public Visibility TitleVis
        {
            get => _titleVis;
            set => SetProperty(ref _titleVis, value);
        }



        public ICommand SelectChangeCommand { get => new DelegateCommand<object>(TreeListSelect); }

        private void TreeListSelect(object item)
        {
            var value = item as TreeViewItem;
            if (value != null)
            {
                // 判断是否是treeview的子集
                if (value.Items.Count == 0)
                {
                    string? name = value.Header.ToString();
                    if (name != null)
                    {
                        if (!Variable._viewMaps.ContainsKey(name))
                            return;
                        TitleVis = Visibility.Collapsed;
                        _regionManager.Regions["ContentRegion"].RequestNavigate(Variable._viewMaps[name]);

                        //   如果切换item从而切换了view，就将需要自动停止线程的页面关闭，不管他们的状态如何
                        var views = _regionManager.Regions["ContentRegion"].Views.ToList();
                        foreach (var view in views)
                        {
                            if (view.GetType() == typeof(BaseControlView))
                            {
                                (view as BaseControlView).ResetStatus();
                            }
                            if (view.GetType() == typeof(DataMonitorView))
                            {
                                (view as DataMonitorView).StopMonitor();
                            }
                        }

                    }
                    
                }
                else
                {
                    return;
                }
            }
        }
    }
}
