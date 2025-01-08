using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using SunwaysFactoryProgram.StaticSource;

namespace SunwaysFactoryProgram.Views
{
    public partial class FunctionView
    {
        private void cbWorkMode_WorkModeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 模式值从1开始
            int value = ((ComboBox)e.Source).SelectedIndex + 1;
            switch (value)
            {
                case 1:
                case 2:
                case 3:
                case 6:
                    cbChildMode.Visibility = Visibility.Collapsed;
                    break;
                // OffLineMode
                case 4:
                    cbChildMode.ItemsSource = new List<string>() { "Single", "Multi", "3nd together", };
                    cbChildMode.SelectedIndex = 0;
                    cbChildMode.Visibility = Visibility.Visible;
                    break;
                // ForceMode
                case 5:
                    cbChildMode.ItemsSource = new List<string>() { "ForceChger", "ForceDisChg", };
                    cbChildMode.SelectedIndex = 0;
                    cbChildMode.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void btnSetWorkMode_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("请连接串口!");
                return;
            }

            if (cbWorkMode.SelectedIndex < 0)
            {
                MessageBox.Show("请选择工作模式!");
                return;
            }


            if ((cbWorkMode.SelectedIndex + 1) == 4)
            {
                _serialDevice.SetOffLineMode((ushort)cbChildMode.SelectedIndex, cbChildMode.Text);
            }
            else if ((cbWorkMode.SelectedIndex + 1) == 5)
            {
                _serialDevice.SetForceMode((ushort)cbChildMode.SelectedIndex, cbChildMode.Text);
            }
            else
            {
                _serialDevice.SetWorkMode((ushort)(cbWorkMode.SelectedIndex + 1), cbWorkMode.Text);
            }
        }

        private void btnResetStatus_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("请连接串口!");
                return;
            }

            _serialDevice.ResetStatus();


        }

        private void btnSetSafety_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("请连接串口!");
                return;
            }

            if (cbSafety.SelectedIndex < 0)
            {
                MessageBox.Show("请选择安规!");
                return;
            }

            _serialDevice.SetSafety(cbSafety.Text, Variable._safetyDic[cbSafety.Text]);
        }

        private void btnSetBurnMode_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("请连接串口!");
                return;
            }

            if (cbBurnMode.SelectedIndex < 0)
            {
                MessageBox.Show("请选择老化模式!");
                return;
            }

            _serialDevice.SetBurnMode((ushort)cbBurnMode.SelectedIndex);


        }


        private void btnSetBatteryId_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("请连接串口!");
                return;
            }

            if (cbBatteryId.SelectedIndex < 0)
            {
                MessageBox.Show("请选择Battery ID!");
                return;
            }

            int value = cbBatteryId.SelectedIndex + 1;

            _serialDevice.SetBatteryID((ushort)value);

        }

        private void btnSetBatteryType_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("请连接串口!");
                return;
            }

            if (cbBatteryType.SelectedIndex < 0)
            {
                MessageBox.Show("请选择Battery Type!");
                return;
            }

            _serialDevice.SetBatteryType((ushort)cbBatteryType.SelectedIndex, cbBatteryType.Text);
        }

        private void btnRTC_Click(object sender, RoutedEventArgs e)
        {
            if (!_serialDevice.GetStatus())
            {
                MessageBox.Show("请连接串口!");
                return;
            }

            _serialDevice.ResetTime();
        }
    }
}
