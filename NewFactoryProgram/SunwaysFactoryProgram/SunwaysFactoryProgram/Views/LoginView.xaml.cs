using SunwaysFactoryProgram.DBModel;
using SunwaysFactoryProgram.StaticSource;
using SupportProject;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
using SunwaysFactoryProgram.ViewModels;
using SunwaysFactoryProgram.Messenager;
using Prism.Ioc;
using Prism.DryIoc;
using Prism.Events;
using Prism.Regions;
using SunwaysFactoryProgram.Model;

namespace SunwaysFactoryProgram.Views
{
    /// <summary>
    /// loginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        private IFreeSql fsql = DBHelper.SqlEntity;
        private string INIPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\Login.Json";
        private LoginViewModel viewModel = new LoginViewModel();
        private IContainerProvider _containerProvider;
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;
        public LoginView(IContainerProvider containerProvider, IEventAggregator ea, IRegionManager region)
        {        
            InitializeComponent();
            _containerProvider = containerProvider;
            _eventAggregator = ea;
            _regionManager = region;

            this.DataContext = viewModel;
            cbProcedure.ItemsSource = fsql.Select<TBS_Procedure>().ToList(a => a.ProcedureName).Distinct().ToList();

            if (!File.Exists(INIPath))
            {
                cbProcedure.Text = fsql.Select<TBS_Procedure>().ToList(a => a.ProcedureName).Distinct().ToList().FirstOrDefault();
            }
            else
            {
                string cfg = FileHelper.GetJsonFile(INIPath);
                if (!string.IsNullOrEmpty(cfg))
                {
                    LoginConfig? loginCfg = JsonConvert.DeserializeObject<LoginConfig>(cfg);
                    if (loginCfg != null)
                    {                        
                        tbId.Text = loginCfg.Id.ToUpper();
                        cbProcedure.Text = loginCfg.ProcedureName;
                        cbStation.Text = loginCfg.StationName;
                    }
                }
            }
           
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbId.Text) || string.IsNullOrEmpty(viewModel.PassWord) ||
               string.IsNullOrEmpty(cbProcedure.Text) || string.IsNullOrEmpty(cbProcedure.Text))
            {
                MessageBox.Show("选项为空,无法登录!");
                return;
            }

            var inf = fsql.Select<TBS_User>().Where(x => x.UserId == tbId.Text).First();
            if (inf == null)
            {
                MessageBox.Show("不存在该用户");
                return;
            }
            else if (inf.PassWord.ToUpper() != viewModel.PassWord.ToUpper())
            {
                MessageBox.Show("密码错误");
                return;
            }

            LoginConfig loginCfg = new LoginConfig() {
                Id = tbId.Text,
                ProcedureName = cbProcedure.Text,
                StationName = cbStation.Text,
            };

            FileHelper.WriteJsonFile(INIPath, JsonConvert.SerializeObject(loginCfg));

            // ((Window)loginview).DialogResult = true;
            // this.Hide();

            if (cbProcedure.Text == "入库扫码")
            {
                this.Close();
                _containerProvider.Resolve<WareHouseView>().ShowDialog();
            }
            else if (cbProcedure.Text == "老化测试")
            {
                this.Close();
                BurnInView burnInView = new BurnInView(inf.UserName, cbProcedure.Text, cbStation.Text);
                burnInView.ShowDialog();
            }
            else
            {

                if (Variable.isFirstStart)
                {
                    ((Window)loginview).DialogResult = true;
                    Variable.isFirstStart = false;
                }
                else
                {
                    _containerProvider.Resolve<MainView>().Show();
                }
                _eventAggregator.GetEvent<Messenge_MainViewInf_Event>().Publish(new List<string> { inf.UserName, cbProcedure.Text, cbStation.Text });

                if (cbProcedure.Text == "包装测试")
                {

                    _regionManager.Regions["ContentRegion"].RequestNavigate("PackView");
                    
                    this.Close();
                }
                else
                {
                    _regionManager.Regions["ContentRegion"].RequestNavigate(Variable.ViewMap[cbProcedure.Text]);
                    this.Close();
                }
            }
            

        }

        private void cbProcedure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? value = sender as ComboBox;
            if (value != null)
            {
                var lists = fsql.Select<TBS_Procedure>().Where(a => a.ProcedureName == value.SelectedItem.ToString()).ToList(o => o.StationName);
                cbStation.ItemsSource = lists;
                cbStation.Text = lists.FirstOrDefault();
            }
        }
    }
}
