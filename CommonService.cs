using SimpleMan.Blues.Model;
using SimpleMan.Blues.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMan.Blues
{
    public class CommonService
    {
        private static CommonService oneInstance;
        private MainWindow mainWindow;
        private InfoWindow infoWindow;

        public static CommonService GetInstance()
        {
            if (oneInstance == null)
                oneInstance = new CommonService();

            return oneInstance;
        }

        public InfoViewModel InfoViewModel
        {
            get
            {
                return mainWindow.InfoViewModel;
            }
        }

        public MainWindow CurrentMainWindow
        {
            set
            {
                this.mainWindow = value;
            }
            get
            {
                return mainWindow;
            }
        }

        public InfoWindow CurrentInfoWindow
        {
            set
            {
                this.infoWindow = value;
            }
            get
            {
                return infoWindow;
            }
        }
    }
}
