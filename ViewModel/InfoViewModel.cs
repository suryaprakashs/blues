using Prism.Mvvm;
using SimpleMan.Blues.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMan.Blues.ViewModel
{
    public class InfoViewModel : BindableBase
    {
        private string _start = "23 April 2018";
        public string Start
        {
            get
            {
                return _start;
            }
        }

        int runningWeek = 0;
        public int RunningWeek
        {
            get
            {
                return runningWeek;
            }
            set
            {
                runningWeek = value;
                RaisePropertyChanged(nameof(RunningWeek));
            }
        }


        int activeForSeconds = 5;
        public int ActiveForSeconds
        {
            get
            {
                return activeForSeconds;
            }
            set
            {
                activeForSeconds = value;
                RaisePropertyChanged(nameof(ActiveForSeconds));
            }
        }

        string activeFor = "00:00:00";
        public string ActiveFor
        {
            get
            {
                return activeFor;
            }
            set
            {
                activeFor = value;
                RaisePropertyChanged(nameof(ActiveFor));
            }
        }

        int runningDay = 0;
        public int RunningDay
        {
            get
            {
                return runningDay;
            }
            set
            {
                runningDay = value;
                RaisePropertyChanged(nameof(RunningDay));
            }
        }

        ObservableCollection<SystemAction> systemActions = new ObservableCollection<SystemAction>();
        public ObservableCollection<SystemAction> SystemActions
        {
            get
            {
                return systemActions;
            }
            set
            {
                systemActions = value;
                RaisePropertyChanged(nameof(SystemActions));
            }
        }
    }
}
