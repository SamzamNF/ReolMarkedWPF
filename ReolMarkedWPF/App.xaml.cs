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

            // 1. Kør vores DI-setup én gang ved opstart.
            DIContainer.Setup();

            // 2. Bed containeren om at bygge og levere et færdigt MainWindow.
            // Containeren finder selv ud af at lave en MainViewModel og injicere den.
            var mainWindow = DIContainer.ServiceProvider.GetRequiredService<MainWindow>();

            // 3. Vis det load-screen, du allerede har lavet (logikken flyttes hertil).
            var loadScreen = new View.LoadScreen();
            Application.Current.MainWindow = loadScreen;
            loadScreen.Show();
        }
    }
}