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
        private int _shelfVendorID;
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

        public ObservableCollection<AccountingResult> Results { get => _results; set { _results = value; OnPropertyChanged(); } }

        public int ShelfVendorID { get => _shelfVendorID; set { _shelfVendorID = value; OnPropertyChanged(); } }
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

        // Knapper
        public RelayCommand CallAccountingData => new RelayCommand(execute => GetResults(), canExecute => CanCallAccountData());

        // Conditions

        private bool CanCallAccountData() => StartDate.HasValue && 
                                             EndDate.HasValue && 
                                             EndDate > StartDate;


        
    }
}
