using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ReolMarkedWPF.View
{
    /// <summary>
    /// Interaction logic for LoadScreen.xaml
    /// </summary>
    public partial class LoadScreen : Window
    {
        public LoadScreen()
        {
            InitializeComponent();

            // Start timer når vinduet loades - simulerer at programmet loader

            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow; // FORTÆL APPEN AT DET NU ER MAINWINDOW
            Loaded += (s, e) =>
            {
                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(4)
                };
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    this.Hide(); //Gemmer loadskærmen
                    mainWindow.Show(); //Åbner MainWindow
                };
                timer.Start();

                // Lukker LoadScreen vinduet
            };

        }
    }
}
