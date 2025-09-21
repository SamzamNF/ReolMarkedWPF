using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.ViewModel;
using ReolMarkedWPF.ViewModels;
using ReolMarkedWPF.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ReolMarkedWPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Definer connection string
            string connectionString = "Database-Connection-String";

            // 2. Opret instanser af konkrete repositories
            IShelfVendorRepository shelfVendorRepository = new SqlShelfVendorRepository(connectionString);
            IRentRepository<Rent> rentRepository = new SqlRentRepository(connectionString);
            // Opret andre repositories her...

            // 3. Opret ViewModel og injicer repository-interfacet
            var mainWindowViewModel = new MainViewModel();
            var shelfVendorViewModel = new ShelfVendorViewModel(shelfVendorRepository);
            var rentAgreementViewModel = new RentAgreementViewModel(rentRepository); // Mangler repositories der skal sættes ind

            // 4. Opret Views
            var mainWindow = new MainWindow();
            RentAgreementControl rentView = new RentAgreementControl();

            // 5. Sæt ViewModel som DataContext for deres respektive View
            // View og ViewModel forbindes her
            mainWindow.DataContext = mainWindowViewModel;
            rentView.DataContext = rentAgreementViewModel;

            // 6. Vis vinduet (Kun for mainwindow, da de andre vinduer er UserControl der findes i MainWindow)

            var loadScreen = new View.LoadScreen();
            Application.Current.MainWindow = loadScreen; 
            loadScreen.Show();
        }
    }
}