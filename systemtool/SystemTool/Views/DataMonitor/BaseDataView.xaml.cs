using MaterialDesignThemes.Wpf;
using Prism.Events;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SystemTool.Messenager;
using SystemTool.Model;
using SystemTool.ViewModels;
using SystemTool.Views.DataMonitor;

namespace SystemTool.Views
{
    /// <summary>
    /// BaseDataView.xaml 的交互逻辑
    /// </summary>
    public partial class BaseDataView : UserControl
    {
        public ObservableCollection<ParaModel> DataModels = new ObservableCollection<ParaModel>();
        public string Header;
        private BaseDataViewModel _dataViewModel;
        private IContainerProvider _containerProvider;
        private IEventAggregator _eventAggregator;
        public BaseDataView(IContainerProvider containerProvider, IEventAggregator ea)
        {
            InitializeComponent();
            this.DataContext = _dataViewModel = new BaseDataViewModel(containerProvider);
            _containerProvider = containerProvider;
            _eventAggregator = ea;



        }

        public void SetBaseInf(string header, ObservableCollection<ParaModel> paraModels)
        {
            tbHeader.Text = Header = header;
            DataModels = paraModels;
            RefreshView(paraModels);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((Panel)this.Parent).Children.Remove(this);

        }

        public void RefreshView(ObservableCollection<ParaModel> paraModels)
        {
            _dataViewModel.UpdateSource(paraModels);

        }

       
    }
}
