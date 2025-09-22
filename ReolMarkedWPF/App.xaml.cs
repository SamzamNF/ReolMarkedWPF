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

            // Kør DI-setup én gang ved opstart.
            DIContainer.Setup();

            // Bed containeren om at bygge og levere et færdigt MainWindow.
            var mainWindow = DIContainer.ServiceProvider.GetRequiredService<MainWindow>();


            // Vis LoadScreen og overfør den korrekte MainWindow instans.
            var loadScreen = new View.LoadScreen(mainWindow);

            Application.Current.MainWindow = loadScreen;
            loadScreen.Show();
        }
    }
}