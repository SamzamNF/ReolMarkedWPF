using System.Windows;

namespace ReolMarkedWPF.View
{
    public partial class LoadScreen : Window
    {
        // Modtag MainWindow i konstruktøren i stedet for at oprette en ny
        public LoadScreen(MainWindow mainWindow)
        {
            InitializeComponent();
        }
    }
}