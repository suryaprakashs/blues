using Hardcodet.Wpf.TaskbarNotification;
using SimpleMan.Blues.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SimpleMan.Blues
{
    public class BackgroundManager
    {
        static BackgroundManager backgroundManager;

        TaskbarIcon tray;
        DispatcherTimer _timer;

        CommonService commonService;

        public static BackgroundManager GetInstance()
        {
            if (backgroundManager == null)
            {
                backgroundManager = new BackgroundManager();
            }

            return backgroundManager;
        }

        public void InitTray()
        {
            tray = Application.Current.FindResource("TrayIcon") as TaskbarIcon;
            var trayViewModel = new TrayViewModel();
            tray.DataContext = trayViewModel;
            TaskbarIcon.SetParentTaskbarIcon(Application.Current.MainWindow, tray);

            commonService = CommonService.GetInstance();
        }

        public void StartUnlockTimer()
        {
            if (_timer != null)
                _timer.Stop();

            _timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += timer_Tick;
            _timer.Start();

            commonService.CurrentMainWindow.UpdateInfo();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer dispatcher = sender as DispatcherTimer;
            string timeAsString = TimeSpan.FromSeconds(CommonService.GetInstance().InfoViewModel.ActiveForSeconds).ToString();

            commonService.InfoViewModel.ActiveForSeconds += dispatcher.Interval.Seconds;
            commonService.InfoViewModel.ActiveFor = timeAsString;            
        }

        public void StopUnlockTimer()
        {
            _timer.Stop();
            _timer = null;
            commonService.InfoViewModel.ActiveForSeconds = 0;
            commonService.InfoViewModel.ActiveFor = TimeSpan.FromSeconds(0).ToString();
        }

        public void ShowInfo()
        {
            commonService.CurrentInfoWindow.Show();
        }

        internal void SaveSystemAction(SystemAction sysAction)
        {
            FileDataManager.GetInstance().WriteFile(sysAction);
        }
    }
}
