using SimpleMan.Blues.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SimpleMan.Blues.NativeMethods;

namespace SimpleMan.Blues
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public InfoViewModel InfoViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Loaded += delegate
            {
                var handle = new WindowInteropHelper(this).Handle;

                SetWindowLong(handle, GWL_EXSTYLE, GetWindowLong(handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
                SetWindowPos(handle, new IntPtr(HWND_BOTTOM), 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE);

                var wndSource = HwndSource.FromHwnd(handle);
                wndSource.AddHook(WindowProc);
            };

            Unloaded += MainWindow_Unloaded;

            InfoViewModel = new InfoViewModel();
            this.DataContext = InfoViewModel;

            UpdateInfo();
            SubscribeKeyboardHooks();
        }
        
        private GlobalKeyboardHook _globalKeyboardHook;
        
        private void SubscribeKeyboardHooks()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
            {
                Console.Out.WriteLine(e.KeyboardData.VirtualCode);
                e.Handled = false;
            }
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            _globalKeyboardHook.Dispose();
        }

        public void UpdateInfo()
        {
            int week = CountDays(DayOfWeek.Monday, DateTime.Parse(InfoViewModel.Start), DateTime.Now);
            InfoViewModel.RunningWeek = week;
            int runningDay = (DateTime.Now - DateTime.Parse(InfoViewModel.Start)).Days + 1;
            InfoViewModel.RunningDay = runningDay;
        }

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_WINDOWPOSCHANGING)
            {
                var windowPos = Marshal.PtrToStructure<WindowPos>(lParam);
                windowPos.hwndInsertAfter = new IntPtr(HWND_BOTTOM);
                windowPos.flags &= ~(uint)SWP_NOZORDER;
                handled = true;
            }
            return IntPtr.Zero;
        }

        static int CountDays(DayOfWeek day, DateTime start, DateTime end)
        {
            TimeSpan ts = end - start;                       // Total duration
            int count = (int)Math.Floor(ts.TotalDays / 7);   // Number of whole weeks
            int remainder = (int)(ts.TotalDays % 7);         // Number of remaining days
            int sinceLastDay = (int)(end.DayOfWeek - day);   // Number of days since last [day]
            if (sinceLastDay < 0) sinceLastDay += 7;         // Adjust for negative days since last [day]

            // If the days in excess of an even week are greater than or equal to the number days since the last [day], then count this one, too.
            if (remainder >= sinceLastDay) count++;

            return count;
        }
    }
}
