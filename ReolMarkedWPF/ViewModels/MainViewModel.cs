using ReolMarkedWPF.Helpers;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.View;
using ReolMarkedWPF.Views;

namespace ReolMarkedWPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Naviger til Welcome.xaml ved opstart
            _navigationService.Navigate(new Welcome());
        }

        // Kommandoer til venstre menu
        public RelayCommand ShowRentCommand =>
            new RelayCommand(_ => _navigationService.Navigate(new RentAgreementView()));

        public RelayCommand ShowShelfVendorCommand =>
            new RelayCommand(_ => _navigationService.Navigate(new ShelfVendorView()));

        public RelayCommand ShowSettingsCommand =>
            new RelayCommand(_ => _navigationService.Navigate(new SettingsView()));
    }
}
