using Microsoft.Win32;
using SimpleMan.Blues.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleMan.Blues
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Mutex oneInstanceMutex;

        BackgroundManager backgroundManager;
        CommonService commonService;

        MainWindow mainWindow;
        InfoWindow infoWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            bool createNew;
            oneInstanceMutex = new Mutex(false, "BgInfo_OneInstanceMutex", out createNew);
            if (!createNew)
            {
                MessageBox.Show("Am running already!", "SimpleMan Blues", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Shutdown();
                return;
            }

            commonService = CommonService.GetInstance();

            mainWindow = new Blues.MainWindow();
            commonService.CurrentMainWindow = mainWindow;
            infoWindow = new InfoWindow();
            commonService.CurrentInfoWindow = infoWindow;

            backgroundManager = BackgroundManager.GetInstance();
            backgroundManager.InitTray();
            backgroundManager.StartUnlockTimer();

            mainWindow.Show();

            var sysAction = new SystemAction { OrderNo = 1, CurrentAction = ActionType.StartUp, ActionTime = DateTime.Now };
            commonService.InfoViewModel.SystemActions.Add(sysAction);
            backgroundManager.SaveSystemAction(sysAction);

            Microsoft.Win32.SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }

        private void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                backgroundManager.StopUnlockTimer();
                int OrderNo = commonService.InfoViewModel.SystemActions.First().OrderNo + 1;

                var sysAction = new SystemAction { OrderNo = OrderNo, CurrentAction = ActionType.Lock, ActionTime = DateTime.Now };
                commonService.InfoViewModel.SystemActions.Insert(0, sysAction);

                backgroundManager.SaveSystemAction(sysAction);
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                backgroundManager.StartUnlockTimer();
                int OrderNo = commonService.InfoViewModel.SystemActions.First().OrderNo + 1;

                var sysAction = new SystemAction { OrderNo = OrderNo, CurrentAction = ActionType.Unlock, ActionTime = DateTime.Now };
                commonService.InfoViewModel.SystemActions.Insert(0, sysAction);

                backgroundManager.SaveSystemAction(sysAction);
            }
        }
    }
}
