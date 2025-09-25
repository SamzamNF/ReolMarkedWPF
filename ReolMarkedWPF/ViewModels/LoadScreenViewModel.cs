using System;
using System.Windows;
using System.Windows.Threading;

namespace ReolMarkedWPF.ViewModels
{
    public class LoadScreenViewModel
    {
        /*private readonly Window _mainWindow;

        public LoadScreenViewModel(Window mainWindow)
        {
            _mainWindow = mainWindow;

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            var timer = (DispatcherTimer)sender!;
            timer.Stop();

            // Show the real main window and close the splash (current MainWindow)
            _mainWindow.Show();

            // Close the splash (current MainWindow) safely
            Application.Current.MainWindow?.Close();

            // Promote the real main window
            Application.Current.MainWindow = _mainWindow;
        }*/
    }
}
