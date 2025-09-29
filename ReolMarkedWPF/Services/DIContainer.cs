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

            // Kode til at hente connection string fra json fil til DB
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Assigner config(connectionen) til vores connectionString
            string? connectionString = config.GetConnectionString("DefaultConnection");

            //Tilføjer den til vores services så den kan bruges med DI
            services.AddSingleton(connectionString);

            // Repositories
            services.AddTransient<IShelfVendorRepository, SqlShelfVendorRepository>();
            services.AddTransient<IRentRepository<Rent>, SqlRentRepository>();
            services.AddTransient<IPaymentMethodRepository, SqlPaymentMethodRepository>();
            services.AddTransient<IShelfRepository, SqlShelfRepository>();
            services.AddTransient<IProductRepository, SqlProductRepository>();
            services.AddTransient<ITransactionRepository<Transaction>, SqlTransactionRepository>();
            services.AddTransient<ITransactionProductRepository, SqlTransactionProductRepository>();



            // ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<ShelfVendorViewModel>();
            services.AddTransient<RentAgreementViewModel>();
            services.AddTransient<ShelfViewModel>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<TransactionProductViewModel>();
            services.AddTransient<TransactionViewModel>();

            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<Welcome>();
            services.AddTransient<ShelfVendorView>();
            services.AddTransient<RentAgreementView>();
            services.AddTransient<RentAgreementChooseShelfView>();
            services.AddTransient<TransactionView>();
            services.AddTransient<ShelfView>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}