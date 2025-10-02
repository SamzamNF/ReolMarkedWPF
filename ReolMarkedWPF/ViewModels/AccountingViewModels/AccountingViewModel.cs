using ReolMarkedWPF.Helpers;
using System.Collections.ObjectModel;
using System.Windows;
using ReolMarkedWPF.Helpers;
using ReolMarkedWPF.Models.AccountingModels;
using ReolMarkedWPF.Repositories.AccountingRepository;
using System.Linq.Expressions;

namespace ReolMarkedWPF.ViewModels.AccountingViewModels
{
    public class AccountingViewModel : ViewModelBase
    {
        private readonly IAccountingRepository _accountingRepository;
        
        // Backing felter til data der hentes fra Stored Procedures
        private int? _shelfVendorID;
        private string _firstName;
        private string _lastName;
        private string _paymentInfo;
        private decimal _totalRent;
        private decimal _totalSale;
        private decimal _afterCommission;
        private decimal _payment;
        private ObservableCollection<AccountingResult> _results;

        // Backing felter til filtrering af data (Bruger DateTime (som kan kan være NULLABLE pga (?)) da DatePicker i XAML ikke kan bruges til DateOnly)
        private DateTime? _startDate;
        private DateTime? _endDate;

        // Listen der holder dataen fra stored procedures
        public ObservableCollection<AccountingResult> Results { get => _results; set { _results = value; OnPropertyChanged(); } }

        // Felter til de properties der kan bruges
        public int? ShelfVendorID { get => _shelfVendorID; set { _shelfVendorID = value; OnPropertyChanged(); } }
        public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(); OnPropertyChanged(); } }
        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); OnPropertyChanged(); } }
        public string PaymentInfo { get => _paymentInfo; set { _paymentInfo = value; OnPropertyChanged(); } }
        public decimal TotalRent { get => _totalRent; set { _totalRent = value; OnPropertyChanged(); } }
        public decimal TotalSale { get => _totalSale; set { _totalSale = value; OnPropertyChanged(); } }
        public decimal AfterCommission { get => _afterCommission; set { _afterCommission = value; OnPropertyChanged(); } }
        public decimal Payment { get => _payment; set { _payment = value; OnPropertyChanged(); } }

        // Felter til at hente data ud fr datoer til metoden
        public DateTime? StartDate { get => _startDate; set { _startDate = value; OnPropertyChanged(); } }
        public DateTime? EndDate { get => _endDate; set { _endDate = value; OnPropertyChanged(); } }

        public AccountingViewModel(IAccountingRepository repository)
        {
            _accountingRepository = repository;
            Results = new ObservableCollection<AccountingResult>();
        }


        private async Task GetResults()
        {
            // Clear den tidligere liste
            Results.Clear();
            try
            {
                // Konvertere vores DateTimes til DateOnlys
                var startDateOnly = DateOnly.FromDateTime(StartDate.Value);
                var endDateOnly = DateOnly.FromDateTime(EndDate.Value);

                // Await så programmet kører videre, mens data hentes
                var resultlist = await _accountingRepository.GetAccountingData(startDateOnly, endDateOnly);

                // Tilføjer alt dataen fra storedprocedure til vores list
                resultlist
                    .ToList()
                    .ForEach(accountingData => Results.Add(accountingData));

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved hentning af data: {ex.Message}");
            }
        }

        private async Task GetSoldResult()
        {
            Results.Clear();
            try
            {
                // Sætter ID til at være NULL hvis intet vises, hvilket henter ALT data, da stored procedure henter alt hvis intet id bliver skrevet
                // Det virker, fordi integer til ShelfVendorID er sat til at være nullable
                if(ShelfVendorID == 0)
                {
                    ShelfVendorID = null;
                }
                
                var soldList = await _accountingRepository.GetAllSales(ShelfVendorID);

                //Tilføjer data fra storedprocedure til listen
                soldList
                     .ForEach(soldData => Results.Add(soldData));

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved hentning af data: {ex.Message}");
            }
        }

        private void ResetData()
        {
            Results.Clear();
        }

        // Knapper
        public RelayCommand CallAccountingData => new RelayCommand(execute => GetResults(), canExecute => CanCallAccountData());
        public RelayCommand CallGetSoldData => new RelayCommand(execute => GetSoldResult(), canExecute => CanCallSoldData());
        public RelayCommand CallResetData => new RelayCommand(execute => ResetData());

        // Conditions

        private bool CanCallAccountData() => StartDate.HasValue && 
                                             EndDate.HasValue && 
                                             EndDate > StartDate;

        // Godtager kun talværdier, og tal som er 0 eller over
        private bool CanCallSoldData() => ShelfVendorID.HasValue && ShelfVendorID.Value >= 0;



        
    }
}
