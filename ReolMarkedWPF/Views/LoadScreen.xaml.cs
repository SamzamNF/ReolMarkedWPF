using System.Windows;
using System.Windows.Threading;

namespace ReolMarkedWPF.View
{
    public partial class LoadScreen : Window
    {
        // Modtag MainWindow i konstruktøren i stedet for at oprette en ny
        public LoadScreen(MainWindow mainWindow)
        {
            InitializeComponent();

            Application.Current.MainWindow = mainWindow; // Sæt MainWindow
            Loaded += (s, e) =>
            {
                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(4)
                };
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    this.Hide();
                    mainWindow.Show(); // Vis den DI-oprettede instans
                };
                timer.Start();
            };
        }
    }
}