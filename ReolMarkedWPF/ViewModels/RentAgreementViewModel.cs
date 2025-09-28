using ReolMarkedWPF.Helpers;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
using System.Collections.ObjectModel;
using System.Windows;

namespace ReolMarkedWPF.ViewModels
{
    public class RentAgreementViewModel : ViewModelBase
    {
        // ViewModel
        private readonly MainViewModel _mainViewModel;
        // Giver adgang til MainViewModel navigation
        public MainViewModel MainVM => _mainViewModel;

        // Repositories
        private readonly IRentRepository<Rent> _rentRepository;
        private readonly IShelfRepository _shelfRepository;
        private readonly IShelfVendorRepository _shelfVendorRepository;

        // Felter
        private int _shelfVendorID;
        private DateOnly _startDate;
        private DateOnly _endDate;
        private int _rentAgreementID;
        private ObservableCollection<Shelf> _shelves;
        private ObservableCollection<ShelfVendor> _shelfVendor;
        private ObservableCollection<Rent> _rentAgreements;

        private Shelf _selectedShelf;
        private ShelfVendor _selectedShelfVendor;
        private Rent _selectedRentAgreement;

        // Properties
        public int RentAgreementID
        {
            get => _rentAgreementID;
            set
            {
                _rentAgreementID = value;
                OnPropertyChanged();
            }
        }
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
        // Mangler repository til at hente shelfvendor og så hente data i listerne
        public RentAgreementViewModel(IRentRepository<Rent> rentRepository, IShelfRepository shelfRepository, 
            IServiceProvider serviceProvider, IShelfVendorRepository shelfVendorRepository)
        {
            this._rentRepository = rentRepository;
            this._shelfRepository = shelfRepository;
            this._shelfVendorRepository = shelfVendorRepository;

            RentAgreements = new ObservableCollection<Rent>(_rentRepository.GetAllRents());
            Shelves = new ObservableCollection<Shelf>(_shelfRepository.GetAllShelves());
            ShelfVendors = new ObservableCollection<ShelfVendor>(_shelfVendorRepository.GetAllShelfVendors());

            
            // Henter den rigtige MainViewModel fra MainWindow, som er oprettet i App.xaml.cs, ved at hente fra den igangværende application
            // ? - null-conditional operator, tjekker at MainWindow IKKE er null - hvis det er null, køres if statement ikke
            // Hvis mainWindow ikke er null, henter den DataContext og tjekker om det er en MainViewModel instans
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow?.DataContext is MainViewModel mainViewModel)
            {
                this._mainViewModel = mainViewModel;
            }
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

            // Henter det oprettede unikke ID fra db efter det er oprettet
            int newRentID = _rentRepository.AddRent(rent);

            // Sætter det midlertidige objekts attribut til det returnede ID fra db
            rent.RentAgreementID = newRentID;

            // Tilføjer den til listen
            RentAgreements.Add(rent);

            // Finder den valgte reol og sætter dens id til den oprettede aftales ID
            // Bruger null conditonal operator, og returner null hvis den ikke finder en SelectedShelf
            var existingShelf = Shelves
                                    .FirstOrDefault(s => s.ShelfNumber == SelectedShelf?.ShelfNumber);
            if (existingShelf != null)
            {
                existingShelf.RentAgreementID = newRentID;
                _shelfRepository.UpdateShelf(existingShelf);
            }
            


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
            // Null-conditional operator (?.) sikrer at Hvis SelectedRentAgreement ikke er null, så prøv at hente RentAgreementID
            // Hvis den er null (SelectedRentAgreement) - så returnes null i stedet for at kaste en NullReferenceException
            var rentToDelete = RentAgreements
                                .FirstOrDefault(r => r.RentAgreementID == SelectedRentAgreement?.RentAgreementID);


            if (rentToDelete != null)
            {
                if (MessageBox.Show($"Er du sikker på, at du vil slette aftalen med ID - DETTE ER PERMANENT: {SelectedRentAgreement.RentAgreementID}?",
                "Bekræft sletning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _rentRepository.DeleteRent(rentToDelete);

                        
                        // Skal frigøre reolen fra sin aftale ved at finde den Reol der matcher med den valgte lejeaftale
                        var shelfToRlease = Shelves
                                            .FirstOrDefault(s => s.RentAgreementID == SelectedRentAgreement?.RentAgreementID);

                        if (shelfToRlease != null)
                        {
                            shelfToRlease.RentAgreementID = null;
                            _shelfRepository.UpdateShelf(shelfToRlease);
                        }

                        RentAgreements.Remove(rentToDelete);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
                

        
        /* Opdatere en lejeaftale ved brug af SelectedRentAgreement, så vælges det objekt ved at matche med dens ID i listen.
           Ved brug af SelectedRentAgreement property opdateres vores normale StartDate & EndDate inputfelter til det valgte objekt
           Det valgte objekt opdateres så med det man har indtastet i felterne når metoden køres, ved at sætte rentToUpdates felter = den indtastede data
         */
        private void EditRent()
        {
            var rentToUpdate = RentAgreements
                                .FirstOrDefault(r => r.RentAgreementID == SelectedRentAgreement?.RentAgreementID);

            if (rentToUpdate != null)
            {
                rentToUpdate.StartDate = this.StartDate;
                rentToUpdate.EndDate = this.EndDate;               

               try
                {
                    _rentRepository.UpdateRent(rentToUpdate);
                    //RentAgreements = new ObservableCollection<Rent>(_rentRepository.GetAllRents());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        // Metode til at automatisk frigive reoler hvis lejeaftale er udløbet
        private void ReleaseExpiredRentAgreement()
        {
            // Henter systemets nuværende dato
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach (var shelf in Shelves)
            {
                // Skip hvis reol ikke har en aktiv aftale
                if (shelf.RentAgreementID == null)
                    continue;

                // Finder Aftale som passer til reol
                var rent = RentAgreements
                            .FirstOrDefault(r => r.RentAgreementID == shelf.RentAgreementID);

                // Ekstra sikring hvis en aftale ikke findes, pga database fejl
                if (rent == null)
                    continue;

                // Hvis today er størrere end EndDate, så nulstilles reolen
                if (rent.EndDate < today)
                {
                    shelf.RentAgreementID = null;
                    _shelfRepository.UpdateShelf(shelf);
                }
            }
        }

        // Knapper
        public RelayCommand AddRentCommand => new RelayCommand(execute => AddRent(), canExecute => CanAddRent());
        public RelayCommand EditRentCommand => new RelayCommand(execute => EditRent(), canExecute => CanEditRent());
        public RelayCommand DeleteRentCommand => new RelayCommand(execute =>  DeleteRent(), canExecute => CanDeleteRent());

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
