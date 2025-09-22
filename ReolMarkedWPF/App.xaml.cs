using Microsoft.Extensions.DependencyInjection;
using ReolMarkedWPF.Services;
using System.Windows;
using System.Configuration;
using System.Windows.Navigation;

namespace ReolMarkedWPF
{
    public partial class App : Application
    {
        public INavigationService NavigationService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            NavigationService = new NavigationService(); // Replace with your actual implementation

            string connectionString = null; //Hurtigt fix for at programmet kan køre. 

            // 1. Kør vores DI-setup én gang ved opstart.
            //        DIContainer.Setup(); Udkommenteret, da den laver fejl!

            // Bed containeren om at bygge og levere et færdigt MainWindow.
            var mainWindow = DIContainer.ServiceProvider.GetRequiredService<MainWindow>();

            // 3. Opret ViewModel og injicer repository-interfacet
         //   var mainWindowViewModel = new MainViewModel(navigationService);
            var shelfVendorViewModel = new ShelfVendorViewModel(shelfVendorRepository);
            var rentAgreementViewModel = new RentAgreementViewModel(rentRepository); // Mangler repositories der skal sættes ind
            var RentAgreementChooseShelfViewModel = new RentAgreementChooseShelfViewModel();

            // Vis LoadScreen og overfør den korrekte MainWindow instans.
            var loadScreen = new View.LoadScreen(mainWindow);

            Application.Current.MainWindow = loadScreen;
            loadScreen.Show();
        }
    }
}