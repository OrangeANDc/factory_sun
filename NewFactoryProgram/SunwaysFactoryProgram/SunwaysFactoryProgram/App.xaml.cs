using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using SunwaysFactoryProgram.Protocol;
using SunwaysFactoryProgram.Views;
using SunwaysFactoryProgram.Views.DataViews;
using SunwaysFactoryProgram.Views.FuncViews;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace SunwaysFactoryProgram
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
            containerRegistry.Register<LoginView>();
            containerRegistry.Register<WareHouseView>();
            containerRegistry.RegisterSingleton<SerialDevice>();
            //throw new NotImplementedException();

            containerRegistry.RegisterForNavigation<PackView>();
            containerRegistry.RegisterForNavigation<FunctionView>();
            containerRegistry.RegisterForNavigation<Low_PackView>();
        }

        protected override void InitializeShell(Window shell)
        {
            if (Container.Resolve<LoginView>().ShowDialog() == false)
            {
                Application.Current.Shutdown();
            }
            base.InitializeShell(shell);
        }     
    }
}
