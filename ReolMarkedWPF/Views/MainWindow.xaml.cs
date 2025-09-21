using ReolMarkedWPF.View;
using System.Windows;

namespace ReolMarkedWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Welcome welcome = new Welcome();
            InitializeComponent();
            MainFrame.Navigate(new Welcome()); // Navigér til velkomst-side
        }
    }
}