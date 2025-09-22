using ReolMarkedWPF.Helpers;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Navigation;

namespace ReolMarkedWPF.ViewModels
{
    public class RentAgreementViewModel : ViewModelBase
    {
        // Repositories
        private readonly IRentRepository<Rent> _rentRepository;
        
        //Navigation
        private readonly INavigationService _navigationService;

        // Felter
        private int _shelfVendorID;
        private DateOnly _startDate;
        private DateOnly _endDate;
        private ObservableCollection<Shelf> _shelves;
        private ObservableCollection<ShelfVendor> _shelfVendor;
        private ObservableCollection<Rent> _rentAgreements;

        private Shelf _selectedShelf;
        private ShelfVendor _selectedShelfVendor;
        private Rent _selectedRentAgreement;

        // Properties
        public int ShelfVendorID
        {
            get => _shelfVendorID;
            set
            {
                _shelfVendorID = value;
                OnPropertyChanged();
            }
        }
        public DateOnly StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }
        public DateOnly EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Shelf> Shelves
        {
            get => _shelves;
            private set
            {
                _shelves = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ShelfVendor> ShelfVendors
        {
            get => _shelfVendor;
            private set
            {
                _shelfVendor = value;
                OnPropertyChanged();    
            }
        }
        public ObservableCollection<Rent> RentAgreements
        {
            get => _rentAgreements;
            private set
            {
                _rentAgreements = value;
                OnPropertyChanged();
            }
        }
        public Shelf SelectedShelf
        {
            get => _selectedShelf;
            set
            {
                _selectedShelf = value;
                OnPropertyChanged();
            }
        }
        public ShelfVendor SelectedShelfVendor
        {
            get => _selectedShelfVendor;
            set
            {
                _selectedShelfVendor = value;
                OnPropertyChanged();
            }
        }
        public Rent SelectedRentAgreement
        {
            get => _selectedRentAgreement;
            set
            {
                // Sætter tekstbokse til at automatisk være det, som SelectedRentAgreement objektet er

                _selectedRentAgreement = value;
                OnPropertyChanged();
                if (value != null)
                {
                    StartDate = value.StartDate;
                    EndDate = value.EndDate;
                }
            }
        }




        // Konstruktør
        // Mangler repository til at hente shelfvendor + shelves og så hente data i listerne
        public RentAgreementViewModel(IRentRepository<Rent> repository, INavigationService navigationService)
        {
            _rentRepository = repository;
            _navigationService = navigationService;
            RentAgreements = new ObservableCollection<Rent>(_rentRepository.GetAllRents());
        }

        // Metoder
        private void AddRent()
        {
            var rent = new Rent
            {
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                ShelfVendorID = SelectedShelfVendor.ShelfVendorID,
            };

            _rentRepository.AddRent(rent);
            RentAgreements.Add(rent);

            // Mangler at implementere så en shelf får sat RentAgreementID på sig
            // Dvs mangler at kunne bruge update fra ShelfRepo

            ShelfVendorID = default;
            StartDate = default;
            EndDate = default;
        }

        
        /* Sletter en lejeaftale ved brug af SelectedRentAgreement, når man har valgt en aftale, så gemmes et objekt
           Objektet bliver gemt i en midlertidig variabel, og hvis et objekt blev fundet og gemt i variablen, så slettes
           det fra databasen og listen ved at man sætter variablen ind i begge metoder
         */
        private void DeleteRent()
        {
            var rentToDelete = RentAgreements
                                .FirstOrDefault(r => r.RentID == SelectedRentAgreement?.RentID);

            if (rentToDelete != null)
            {
                try
                {
                    _rentRepository.DeleteRent(rentToDelete);
                    RentAgreements.Remove(rentToDelete);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        
        /* Opdatere en lejeaftale ved brug af SelectedRentAgreement, så vælges det objekt ved at matche med dens ID i listen.
           Ved brug af SelectedRentAgreement property opdateres vores normale StartDate & EndDate felter til det valgte objekt
           Det valgte objekt opdateres så med det man har indtastet i felterne når metoden køres
         */
        private void EditRent()
        {
            var rentToUpdate = RentAgreements
                                .FirstOrDefault(r => r.RentID == SelectedRentAgreement.RentID);

            if (rentToUpdate != null)
            {
                rentToUpdate.StartDate = this.StartDate;
                rentToUpdate.EndDate = this.EndDate;

               try
                {
                    _rentRepository.UpdateRent(rentToUpdate);
                    RentAgreements = new ObservableCollection<Rent>(_rentRepository.GetAllRents());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        // Knapper
        public RelayCommand AddRentCommand => new RelayCommand(execute => AddRent(), canExecute => CanAddRent());
        public RelayCommand EditRentCommand => new RelayCommand(execute => EditRent(), canExecute => CanEditRent());
        public RelayCommand DeleteRentCommand => new RelayCommand(execute =>  DeleteRent(), canExecute => CanDeleteRent());
        public RelayCommand ShowShelfSelectionCommand =>
            new RelayCommand(_ => _navigationService.Navigate(new RentAgreementChooseShelfView()));

        // Conditions
        private bool CanAddRent() => StartDate != default &&
                                     EndDate != default &&
                                     SelectedShelfVendor != null &&
                                     StartDate < EndDate;

        private bool CanDeleteRent() => SelectedRentAgreement != null;

        private bool CanEditRent() => SelectedRentAgreement != null &&
                                     StartDate != default &&
                                     EndDate != default &&
                                     StartDate < EndDate;

    }
}
