using ReolMarkedWPF.Helpers;
using ReolMarkedWPF.Views;

namespace ReolMarkedWPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // Konstruktør som starter med et default view
        public MainViewModel()
        {
            CurrentView = new ShelfVendorControl();
        }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        // Commands til at skifte view som bindes til knapper
        public RelayCommand ShowRentViewCommand => new RelayCommand(execute => ShowRentAgreement());
        public RelayCommand ShelfVendorViewCommand => new RelayCommand(execute => ShowShelfVendor());


        // Metode til at skifte view
        private void ShowRentAgreement()
        {
            CurrentView = new RentAgreementControl();
        }

        private void ShowShelfVendor()
        {
            CurrentView = new ShelfVendorControl();
        }
    }
}
