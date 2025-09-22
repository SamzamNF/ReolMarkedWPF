using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.ViewModel;
using ReolMarkedWPF.ViewModels;
using ReolMarkedWPF.Views;
using ReolMarkedWPF.Helpers;
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

            // 2. Opret instanser af konkrete repositories
            IShelfVendorRepository shelfVendorRepository = new SqlShelfVendorRepository(connectionString);
            IRentRepository<Rent> rentRepository = new SqlRentRepository(connectionString);
            // Opret andre repositories her...

            // 3. Opret ViewModel og injicer repository-interfacet
         //   var mainWindowViewModel = new MainViewModel(navigationService);
            var shelfVendorViewModel = new ShelfVendorViewModel(shelfVendorRepository);
            var rentAgreementViewModel = new RentAgreementViewModel(rentRepository); // Mangler repositories der skal sættes ind
            var RentAgreementChooseShelfViewModel = new RentAgreementChooseShelfViewModel();

            // 4. Opret Views
            var mainWindow = new MainWindow();
            //RentAgreementControl rentView = new RentAgreementControl();

            // 5. Sæt ViewModel som DataContext for deres respektive View
            // View og ViewModel forbindes her
         //   mainWindow.DataContext = mainWindowViewModel;
            //rentView.DataContext = rentAgreementViewModel; 

            
            // 6. Sætter programmet til at starte med at køre "loadScreen" vinduet
            var loadScreen = new View.LoadScreen();
            Application.Current.MainWindow = loadScreen;
            loadScreen.Show();
        }
    }
}