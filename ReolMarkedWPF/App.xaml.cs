using ReolMarkedWPF.ViewModels;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ReolMarkedWPF.Services;


namespace ReolMarkedWPF
{
    public partial class App : Application
    {


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);




            DIContainer.Setup();

            // Dette gøres kun én gang - For MainViewModel & MainWindow, resten foregår i DIcontainer

            // Hent main window fra DI
            var mainWindow = DIContainer.ServiceProvider.GetRequiredService<MainWindow>();
            
            // Henter mainviewmodel fra DI
            var mainViewModel = DIContainer.ServiceProvider.GetRequiredService<MainViewModel>();

            // Sætter datacontext fra DI
            mainWindow.DataContext = mainViewModel;

            // Vis MainWindow
            Current.MainWindow = mainWindow;
            mainWindow.Show();
        }
    }
}
