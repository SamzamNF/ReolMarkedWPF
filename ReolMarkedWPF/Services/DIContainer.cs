using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.ViewModel;
using ReolMarkedWPF.ViewModels;
using ReolMarkedWPF.Views;

namespace ReolMarkedWPF.Services
{
    public static class DIContainer
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Setup()
        {
            var services = new ServiceCollection();

            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string? connectionString = config.GetConnectionString("DefaultConnection");

            services.AddSingleton(connectionString);

            // Repositories
            services.AddTransient<IShelfVendorRepository, SqlShelfVendorRepository>();
            services.AddTransient<IRentRepository<Rent>, SqlRentRepository>();
            services.AddTransient<IPaymentMethodRepository, SqlPaymentMethodRepository>();
            services.AddTransient<IShelfRepository, SqlShelfRepository>();
            services.AddTransient<IProductRepository, SqlProductRepository>();


            // ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<ShelfVendorViewModel>();
            services.AddTransient<RentAgreementViewModel>();
            services.AddTransient<ShelfViewModel>();
            services.AddTransient<ProductViewModel>();


            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<Welcome>();
            services.AddTransient<ShelfVendorView>();
            services.AddTransient<RentAgreementView>();
            services.AddTransient<RentAgreementChooseShelfView>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}