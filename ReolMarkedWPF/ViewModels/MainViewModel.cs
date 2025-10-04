using Microsoft.Extensions.DependencyInjection;
using ReolMarkedWPF.Helpers;
using ReolMarkedWPF.Views;

namespace ReolMarkedWPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;


        // ServiceProvider bliver sendt fra DIcontainer, og det kommer fra at der bliver spurgt om et MainViewModel i app.xaml.cs
        // DI-Container ser så der er en konstruktør og ved at den skal sende sig selv afsted.
        // Dette virker, fordi MainViewModel er oprettet i DIcontainer.cs
        // Det gør, at MainViewModel kan hente andre services og views fra containeren efter behov.
        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            // Start på ShelfVendor-siden (Dette kan ændres nemt ved at skifte linjen under)
            CurrentView = _serviceProvider.GetRequiredService<Welcome>();
        }

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ShowRentViewCommand => new RelayCommand(_ => ShowRentAgreement());
        public RelayCommand ShowShelfVendorViewCommand => new RelayCommand(_ => ShowShelfVendor());
        public RelayCommand ShowCreateRentViewCommand => new RelayCommand(_ => ShowChooseShelfForRent());
        public RelayCommand ShowTransactionViewCommand => new RelayCommand(_ => ShowTransactionView());
        public RelayCommand ShowShelfViewCommand => new RelayCommand(_ => ShowShelfView());
        public RelayCommand ShowAccountingViewCommand => new RelayCommand(_ => ShowAccountingView());
        public RelayCommand ShowProductViewCommand => new RelayCommand(_ => ShowProductView());



        private void ShowChooseShelfForRent()
        {
            CurrentView = _serviceProvider.GetRequiredService<RentAgreementChooseShelfView>();
        }

        private void ShowShelfVendor()
        {
            CurrentView = _serviceProvider.GetRequiredService<ShelfVendorView>();
        }
        private void ShowRentAgreement()
        {
            CurrentView = _serviceProvider.GetRequiredService<RentAgreementView>();
        }
        private void ShowTransactionView()
        {
            CurrentView = _serviceProvider.GetRequiredService<TransactionView>();
        }
        private void ShowShelfView()
        {
            CurrentView = _serviceProvider.GetRequiredService<ShelfView>();
        }
        private void ShowAccountingView()
        {
            CurrentView = _serviceProvider.GetRequiredService<AccountingView>();
        }
        private void ShowProductView()
        {
            CurrentView = _serviceProvider.GetRequiredService<ProductView>();
        }
    }
}
