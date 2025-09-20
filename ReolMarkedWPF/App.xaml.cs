using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.ViewModel;
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
            // Opret andre repositories her...

            // 3. Opret ViewModel og injicer repository-interfacet
            var shelfVendorViewModel = new ShelfVendorViewModel(shelfVendorRepository);

            // 4. Opret MainWindow
            var mainWindow = new MainWindow();

            // 5. Sæt ViewModel som DataContext for MainWindow
            // View og ViewModel forbindes her
            mainWindow.DataContext = shelfVendorViewModel;

            // 6. Vis vinduet
            mainWindow.Show();
        }
    }
}