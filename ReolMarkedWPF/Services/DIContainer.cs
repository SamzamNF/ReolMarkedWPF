using Microsoft.Extensions.DependencyInjection;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.ViewModels;

namespace ReolMarkedWPF.Services
{
    public static class DIContainer
    {
        // En statisk property til at holde vores Service Provider, så den er tilgængelig i hele appen.
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Setup()
        {
            var services = new ServiceCollection();

            // -- REGISTRER SERVICES HER --

            // 1. Singleton: Connection String (der findes kun én instans af denne i hele appen)
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ReolmarkedDB.mdf;Integrated Security=True";
            services.AddSingleton(connectionString);

            // 2. Repositories (Transient: En ny instans oprettes hver gang, der anmodes om en)
            // "Når nogen beder om IShelfVendorRepository, så giv dem en ny SqlShelfVendorRepository."
            services.AddTransient<IShelfVendorRepository, SqlShelfVendorRepository>();
            services.AddTransient<IRentRepository<Rent>, SqlRentRepository>();
            services.AddTransient<IPaymentMethodRepository, SqlPaymentMethodRepository>();
            // Tilføj fremtidige repositories her...

            // 3. ViewModels (Transient)
            services.AddTransient<MainViewModel>();
            services.AddTransient<ShelfVendorViewModel>();
            services.AddTransient<RentAgreementViewModel>();
            // Tilføj fremtidige ViewModels her...

            // 4. Views (Transient)
            // Registrerer MainWindow, så containeren kan bygge den.
            services.AddTransient<MainWindow>();

            // Byg containeren
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}