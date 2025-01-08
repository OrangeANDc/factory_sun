using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.ViewModels
{
    class LoginViewModel : BindableBase
    {
        private string _passWord;
        public string PassWord
        {
            get => _passWord;
            set => SetProperty(ref _passWord, value);
        }
    }
}
