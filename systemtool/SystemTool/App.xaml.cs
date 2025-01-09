using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SystemTool.Protocol;
using SystemTool.Views;
using SystemTool.Views.BaseControl;
using SystemTool.Views.DataMonitor;

namespace SystemTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<MainView>();
            //containerRegistry.RegisterSingleton<Ser>();
            containerRegistry.RegisterForNavigation<FlashView>();
            containerRegistry.RegisterForNavigation<HeadCombineView>();
            containerRegistry.RegisterForNavigation<ParaControlView>();
            containerRegistry.RegisterForNavigation<BaseControlView>();
            containerRegistry.RegisterForNavigation<AdvanceSettingView>();
            containerRegistry.RegisterForNavigation<DataMonitorView>();

            containerRegistry.RegisterSingleton<SerialDevice>();
            containerRegistry.Register<ValueSetView>();
        }
    }
}
