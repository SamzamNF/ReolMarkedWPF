using Microsoft.Extensions.DependencyInjection;
using ReolMarkedWPF.Services;
using System.Windows;

namespace ReolMarkedWPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // TRIN 1: Kør DI-setup én gang ved opstart.
            DIContainer.Setup();

            // TRIN 2: Bed containeren om at bygge og levere et færdigt MainWindow.
            // Containeren er ansvarlig for at oprette MainWindow og alle dens
            // afhængigheder (som MainViewModel, INavigationService osv.) korrekt.
            var mainWindow = DIContainer.ServiceProvider.GetRequiredService<MainWindow>();


            // TRIN 3: Vis LoadScreen og overfør den korrekte MainWindow instans.
            // Vi giver den DI-oprettede mainWindow til LoadScreen, så vi sikrer,
            // at det er den korrekte, fuldt konfigurerede instans, der bliver vist.
            var loadScreen = new View.LoadScreen(mainWindow);

            Application.Current.MainWindow = loadScreen;
            loadScreen.Show();
        }
    }
}