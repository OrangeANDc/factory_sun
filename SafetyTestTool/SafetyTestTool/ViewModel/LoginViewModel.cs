using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SafetyTestTool.ViewModel
{
    internal class LoginViewModel : ObservableObject
    {
        private string? _passWord;
        public string? PassWord
        {
            get => _passWord;
            set => SetProperty(ref _passWord, value);
        }
    }
}
