using ReolMarkedWPF.Helpers;
using ReolMarkedWPF.View;
using ReolMarkedWPF.ViewModels;
using System.Windows;
using System.Windows.Navigation;

namespace ReolMarkedWPF
{
    public partial class MainWindow : Window
    {
        private FrameNavigationService _navigationService;
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Nu eksisterer MainFrame 100%
            _navigationService = new FrameNavigationService(MainFrame);

            // Sæt DataContext
            DataContext = new MainViewModel(_navigationService);

            // Naviger til Welcome.xaml som default
            _navigationService.Navigate(new Welcome());
        }

    }
}