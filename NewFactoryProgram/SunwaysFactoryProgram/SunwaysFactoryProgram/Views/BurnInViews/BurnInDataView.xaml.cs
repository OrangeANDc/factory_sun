using MaterialDesignThemes.Wpf;
using SunwaysFactoryProgram.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SunwaysFactoryProgram.Views.BurnInViews
{
    /// <summary>
    /// BurnInDataView.xaml 的交互逻辑
    /// </summary>
    public partial class BurnInDataView : Window
    {
        public BurnInDataView()
        {
            InitializeComponent();
            BurnInDataViewModel burnInDataViewModel = new BurnInDataViewModel();
            DataContext = burnInDataViewModel;
        }

        public void CombinedDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            CombinedCalendar.SelectedDate = ((BurnInDataViewModel)DataContext).StartDate;
            CombinedClock.Time = ((BurnInDataViewModel)DataContext).StartDate;
        }

        public void CombinedDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "1") &&
            CombinedCalendar.SelectedDate is DateTime selectedDate)
            {
                var combined = selectedDate.AddSeconds(CombinedClock.Time.TimeOfDay.TotalSeconds);
                ((BurnInDataViewModel)DataContext).StartDate = combined;
            }
        }

        public void CombinedDialogOpenedEventHandler2(object sender, DialogOpenedEventArgs eventArgs)
        {
            CombinedCalendar2.SelectedDate = ((BurnInDataViewModel)DataContext).EndDate;
            //CombinedClock.Time = ((BurnInDataViewModel)DataContext).Time;
        }

        public void CombinedDialogClosingEventHandler2(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "1") &&
           CombinedCalendar2.SelectedDate is DateTime selectedDate)
            {
                var combined = selectedDate.AddSeconds(CombinedClock2.Time.TimeOfDay.TotalSeconds);
                ((BurnInDataViewModel)DataContext).EndDate = combined;
            }
        }
    }
}
