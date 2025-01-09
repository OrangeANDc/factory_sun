using Prism.Commands;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SystemTool.Messenager;
using SystemTool.Model;
using SystemTool.Views.DataMonitor;

namespace SystemTool.ViewModels
{
    internal class BaseDataViewModel : BindableBase
    {
        private IContainerProvider _containerProvider;
        public BaseDataViewModel(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
            SetValueCommand = new DelegateCommand(SetValue);
        }
        private ObservableCollection<DataModel> _customContent;
        public ObservableCollection<DataModel> CustomContent
        {
            get => _customContent;
            set => SetProperty(ref _customContent, value);
        }

        private DataModel _selectedContent;
        public DataModel SelectedContent
        {
            get => _selectedContent;
            set => SetProperty(ref _selectedContent, value);
        }

        public  DelegateCommand SetValueCommand {  get; set; }
        private void SetValue()
        {
            try
            {
                if (SelectedContent != null)
                {
                    ValueSetView valueSetView = _containerProvider.Resolve<ValueSetView>();
                    valueSetView.SetViewConfig(SelectedContent);
                    if (valueSetView.ShowDialog() == true)
                    {

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public void UpdateSource(ObservableCollection<ParaModel> sources)
        {
            ObservableCollection<DataModel> datas = new ObservableCollection<DataModel>(); 
            foreach (var so in sources)
            {
                datas.Add(new DataModel { 
                    DataValue = so.DataValue, 
                    DataName = so.DataName, 
                    DataUnit = so.DataUnit,
                    DataAddress = so.DataAddress,
                    DataGain = so.DataGain,
                    DataLength = so.DataLength,
                    IsSigned = so.IsSigned,
                
                });
            }

            CustomContent = datas;
        }
    }

    
}
