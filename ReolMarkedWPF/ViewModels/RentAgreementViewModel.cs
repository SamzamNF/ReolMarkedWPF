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
        public MainViewModel MainVM => _mainViewModel;

        // Repositories
        private readonly IRentRepository<Rent> _rentRepository;
        private readonly IShelfRepository _shelfRepository;
        private readonly IShelfVendorRepository _shelfVendorRepository;

        // Felter
        private DateOnly _startDate;
        private DateOnly _endDate;
        private ObservableCollection<Shelf> _shelves;
        private ObservableCollection<Rent> _rentAgreements;
        private Rent _selectedRentAgreement;

        // Properties
        public DateOnly StartDate { get => _startDate; set { _startDate = value; OnPropertyChanged(); } }
        public DateOnly EndDate { get => _endDate; set { _endDate = value; OnPropertyChanged(); } }
        public ObservableCollection<Shelf> Shelves { get => _shelves; private set { _shelves = value; OnPropertyChanged(); } }
        public ObservableCollection<Rent> RentAgreements { get => _rentAgreements; private set { _rentAgreements = value; OnPropertyChanged(); } }
        public Rent SelectedRentAgreement
        {
            get => _selectedRentAgreement;
            set
            {
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
        public RentAgreementViewModel(IRentRepository<Rent> rentRepository, IShelfRepository shelfRepository,
            IServiceProvider serviceProvider, IShelfVendorRepository shelfVendorRepository)
        {
            this._rentRepository = rentRepository;
            this._shelfRepository = shelfRepository;
            this._shelfVendorRepository = shelfVendorRepository;

            RentAgreements = new ObservableCollection<Rent>(_rentRepository.GetAllRents());

            // NYT: Kalder metoden, der opretter reol-layoutet.
            InitializeShelvesWithLayout();

            var mainWindow = Application.Current.MainWindow;
            if (mainWindow?.DataContext is MainViewModel mainViewModel)
            {
                this._mainViewModel = mainViewModel;
            }
        }

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
                    shelvesFromDb.TryGetValue(shelfNumber, out var dbShelf);

                    layoutReadyShelves.Add(new Shelf
                    {
                        ShelfNumber = shelfNumber,
                        RentAgreementID = dbShelf?.RentAgreementID, // Brug ID fra DB hvis det findes
                                                                    // Tildel X- og Y-koordinater baseret på række og kolonne
                        X = startX + col * (shelfWidth + horizontalGap),
                        Y = startY + row * (shelfHeight + verticalGap)
                    });
                }
            }
            // Sæt den færdige liste som datakilde for viewet
            Shelves = layoutReadyShelves;
        }

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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteRent()
        {
            var rentToDelete = RentAgreements
                                .FirstOrDefault(r => r.RentAgreementID == SelectedRentAgreement?.RentAgreementID);

            if (rentToDelete != null)
            {
                if (MessageBox.Show($"Er du sikker på, at du vil slette aftalen med ID: {SelectedRentAgreement.RentAgreementID}?",
                "Bekræft sletning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _rentRepository.DeleteRent(rentToDelete);

                        var shelfToRelease = Shelves
                                            .FirstOrDefault(s => s.RentAgreementID == SelectedRentAgreement?.RentAgreementID);

                        if (shelfToRelease != null)
                        {
                            shelfToRelease.RentAgreementID = null;
                            _shelfRepository.UpdateShelf(shelfToRelease);
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

        public RelayCommand EditRentCommand => new RelayCommand(execute => EditRent(), canExecute => CanEditRent());
        public RelayCommand DeleteRentCommand => new RelayCommand(execute => DeleteRent(), canExecute => CanDeleteRent());

        private bool CanDeleteRent() => SelectedRentAgreement != null;
        private bool CanEditRent() => SelectedRentAgreement != null && StartDate != default && EndDate != default && StartDate < EndDate;
    }
}