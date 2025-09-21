using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.ViewModels;
using ReolMarkedWPF.Helpers;
using System.Windows;
using ReolMarkedWPF.Services; // Tilføj denne for DIContainer

namespace ReolMarkedWPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Kør vores DI-setup én gang ved opstart.
            DIContainer.Setup();

            // Definer connectionString
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ReolmarkedDB.mdf;Integrated Security=True";

            // Opret MainWindow og FrameNavigationService
            var mainWindow = new MainWindow();
            var navigationService = new FrameNavigationService(mainWindow.MainFrame);


            // 2. Opret instanser af konkrete repositories
            IShelfVendorRepository shelfVendorRepository = new SqlShelfVendorRepository(connectionString);
            IRentRepository<Rent> rentRepository = new SqlRentRepository(connectionString);
            // Opret andre repositories her...

            // 3. Opret ViewModel og injicer repository-interfacet
            var mainViewModel = new MainViewModel(navigationService); // Fjern kommentar
            var shelfVendorViewModel = new ShelfVendorViewModel(shelfVendorRepository);
            var rentAgreementViewModel = new RentAgreementViewModel(rentRepository);

            // 4. Opret Views (mainWindow er allerede oprettet)


            // 5. Sæt ViewModel som DataContext for deres respektive View
            // View og ViewModel forbindes her
            mainWindow.DataContext = mainViewModel;
            //rentView.DataContext = rentAgreementViewModel;


            // 6. Sætter programmet til at starte med at køre "loadScreen" vinduet
            var loadScreen = new View.LoadScreen();
            Application.Current.MainWindow = loadScreen;
            loadScreen.Show();
        }
    }
}