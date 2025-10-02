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
        private string _startDateText;
        private string _endDateText;
        private int _rentAgreementID;
        private int _shelfNumber;
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
        public int ShelfNumber
        {
            get => _shelfNumber;
            set
            {
                _shelfNumber = value;
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
        public string StartDateText
        {
            get => _startDateText;
            set
            {
                _startDateText = value;
                OnPropertyChanged();
            }
        }

        public string EndDateText
        {
            get => _endDateText;
            set
            {
                _endDateText = value;
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
                    StartDateText = value.StartDate.ToString("dd/MM/yyyy");
                    EndDateText = value.EndDate.ToString("dd/MM/yyyy");
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
            //Shelves = new ObservableCollection<Shelf>(_shelfRepository.GetAllShelves());
            ShelfVendors = new ObservableCollection<ShelfVendor>(_shelfVendorRepository.GetAllShelfVendors());

            // Kalder metoden, der opretter reol-layoutet.
            InitializeShelvesWithLayout();

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

        // Denne metode opbygger plantegningen for butikken.
        private void InitializeShelvesWithLayout()
        {
            // Hent status for alle reoler fra databasen
            var shelvesFromDb = _shelfRepository.GetAllShelves().ToDictionary(s => s.ShelfNumber);
            var layoutReadyShelves = new ObservableCollection<Shelf>();

            // Definer dimensioner og afstande for et rent layout
            double shelfWidth = 40;
            double shelfHeight = 40;
            double horizontalGap = 15; // Afstand mellem reoler i en række
            double verticalGap = 70;   // Afstand mellem rækker (gangareal)
            double startX = 120;       // Startposition fra venstre kant
            double startY = 150;       // Startposition fra toppen
            int shelvesPerRow = 20;    // Antal reoler pr. række/gang

            // Loop for at oprette 4 rækker af reoler
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < shelvesPerRow; col++)
                {
                    int shelfNumber = (row * shelvesPerRow) + col + 1;

                    // Tjek om reolen findes i databasen for at få dens status (optaget/ledig)
                    if (shelvesFromDb.TryGetValue(shelfNumber, out var dbShelf))
                    {
                        // Hvis den findes, sæt dens X og Y koordinater på objektet fra databasen og tilføj den til listen
                        dbShelf.X = startX + col * (shelfWidth + horizontalGap);
                        dbShelf.Y = startY + row * (shelfHeight + verticalGap);
                        layoutReadyShelves.Add(dbShelf);
                    }
                }
            }
            // Sæt den færdige liste som datakilde for viewet
            Shelves = layoutReadyShelves;
        }

        private void AddRent()
        {


            var rent = new Rent
            {
                StartDate = DateOnly.ParseExact(StartDateText, "dd/MM/yyyy"),
                EndDate = DateOnly.ParseExact(EndDateText, "dd/MM/yyyy"),
                ShelfVendorID = SelectedShelfVendor.ShelfVendorID,
            };

            // Henter det oprettede unikke ID fra db efter det er oprettet
            int newRentID = _rentRepository.AddRent(rent);

            // Sætter det midlertidige objekts attribut til det returnede ID fra db
            rent.RentAgreementID = newRentID;

            // Tilføjer den til listen
            RentAgreements.Add(rent);

            // Hvis SelectedShelf er valgt igennem UI og sendt til SelectShelf()
            // Sætter den valgtes reol rentagreementID, til at være ligmed det som lejeaftalen har fået
            if (SelectedShelf != null)
            {
                SelectedShelf.RentAgreementID = newRentID;
                // Opdatere den valgte reol efter dens ny RentAgreementID er sat
                _shelfRepository.UpdateShelf(SelectedShelf);
            }

            // Genindlæser UI, så den nye oprettede aftale blockere den valgte reol
            InitializeShelvesWithLayout();

            ShelfVendorID = default;
            StartDateText = null;
            EndDateText = null;
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

                        // Skal frigøre reolen fra sin aftale ved at finde den Reol der matcher med den valgte lejeaftale
                        var shelfToRlease = Shelves
                                            .FirstOrDefault(s => s.RentAgreementID == SelectedRentAgreement?.RentAgreementID);

                        if (shelfToRlease != null)
                        {
                            shelfToRlease.RentAgreementID = null;
                            _shelfRepository.UpdateShelf(shelfToRlease);

                            // Vigtigt at opdatere reolen til at være NULL først, ellers kan den ikke slettes da en Reol bruger dens foreign key
                            _rentRepository.DeleteRent(rentToDelete);

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
                rentToUpdate.StartDate = DateOnly.ParseExact(StartDateText, "dd/MM/yyyy");
                rentToUpdate.EndDate = DateOnly.ParseExact(EndDateText, "dd/MM/yyyy");

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

        // Metode til at modtage shelfnumber fra view ved brug af "CommandParameter" fra view af, som bliver sendt videre fra knappen "SelectShelfCommand" af
        private void SelectShelf(object parameter)
        {
            if (parameter is int shelfNumber)
            {
                // Opdatere tekstboksen med det korrekte nummer, som den får fra parameteren fra knappen.
                ShelfNumber = shelfNumber;
                // Sætter SelectedShelf til at være ligmed den valgte reol, ved at bruge den modtagene shelfNumber
                SelectedShelf = Shelves?
                                   .FirstOrDefault(s => s.ShelfNumber == shelfNumber);
            }
        }

        // Knapper
        public RelayCommand AddRentCommand => new RelayCommand(execute => AddRent(), canExecute => CanAddRent());
        public RelayCommand EditRentCommand => new RelayCommand(execute => EditRent(), canExecute => CanEditRent());
        public RelayCommand DeleteRentCommand => new RelayCommand(execute => DeleteRent(), canExecute => CanDeleteRent());

        // Knap til at hente ShelfNumber med
        public RelayCommand SelectShelfCommand => new RelayCommand(parameter => SelectShelf(parameter));


        // Conditions
        private bool CanAddRent() => !string.IsNullOrEmpty(StartDateText) &&
                                     !string.IsNullOrEmpty(EndDateText) &&
                                     IsValidDateRange();

        private bool CanDeleteRent() => SelectedRentAgreement != null;

        private bool CanEditRent() => SelectedRentAgreement != null &&
                                     !string.IsNullOrEmpty(StartDateText) &&
                                     !string.IsNullOrEmpty(EndDateText) &&
                                     IsValidDateRange();

        // Condition metode som parser fra string til DateOnly
        private bool IsValidDateRange() => 
            DateOnly.TryParseExact(StartDateText, "dd/MM/yyyy", out DateOnly startDate) &&
            DateOnly.TryParseExact(EndDateText, "dd/MM/yyyy", out DateOnly endDate) &&
            startDate < endDate;
    }
}