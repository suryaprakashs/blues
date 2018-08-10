using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SimpleMan.Blues
{
    sealed class TrayViewModel : BindableBase {

        public TrayViewModel() {
            ExitCommand = new DelegateCommand(() => Application.Current.Shutdown());
            InfoCommand = new DelegateCommand(()=> BackgroundManager.GetInstance().ShowInfo());
        }

        public ICommand ExitCommand { get; }

        public ICommand InfoCommand { get; }
    }
}
