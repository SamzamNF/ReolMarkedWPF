using ReolMarkedWPF.Helpers;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace ReolMarkedWPF.ViewModels
{
    public class ShelfVendorViewModel : ViewModelBase
    {
        // Reference til repository-laget for at hente/gemme data.
        private readonly IShelfVendorRepository _repository;
        private readonly IPaymentMethodRepository _methodRepository;

        // Private felter til at holde styr på UI-input.
        private ShelfVendor _selectedShelfVendor;
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _email;
        private string _paymentInfo;
        private string _selectedPaymentOption;

        // Liste til at hente betlingsoplysninger i Combobox i UI (fra Models)
        public List<string> PaymentOptions { get; } = new List<string> { "MobilePay", "Bankoverførsel" };


        // Den offentlige liste af sælgere, som UI'en (f.eks. en ListBox) binder til.
        public ObservableCollection<ShelfVendor> ShelfVendors { get; set; }

        // Holder styr på den sælger, der er valgt i UI'en.
        public ShelfVendor SelectedShelfVendor
        {
            get => _selectedShelfVendor;
            set
            {
                _selectedShelfVendor = value;
                OnPropertyChanged(); // Giver UI besked om, at værdien er ændret.
                if (value != null)
                {
                    // Opdaterer tekstboksene med den valgte sælgers data.
                    FirstName = value.FirstName;
                    LastName = value.LastName;
                    PhoneNumber = value.PhoneNumber;
                    Email = value.Email;

                }
            }
        }

        // Properties til at binde til tekstbokse i UI for indtastning.
        public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(); } }
        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); } }
        public string PhoneNumber { get => _phoneNumber; set { _phoneNumber = value; OnPropertyChanged(); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string PaymentInfo { get => _paymentInfo; set { _paymentInfo = value; OnPropertyChanged(); } }
        public string SelectedPaymentOption { get => _selectedPaymentOption; set { _selectedPaymentOption = value; OnPropertyChanged(); } }



        // Kommandoer, som knapper i UI'en binder til.
        public RelayCommand AddShelfVendorCommand { get; }
        public RelayCommand EditShelfVendorCommand { get; }
        public RelayCommand DeleteShelfVendorCommand { get; }

        // Konstruktør: Initialiserer ViewModel'en.
        public ShelfVendorViewModel(IShelfVendorRepository repository, IPaymentMethodRepository methodRepository)
        {
            _repository = repository;
            _methodRepository = methodRepository;

            // Henter alle sælgere fra databasen, når view-modellen oprettes.
            ShelfVendors = new ObservableCollection<ShelfVendor>(_repository.GetAllShelfVendors());

            // Initialiserer kommandoer med deres metoder og betingelser (CanExecute).
            AddShelfVendorCommand = new RelayCommand(execute => AddShelfVendor(), canExecute => CanAdd());
            EditShelfVendorCommand = new RelayCommand(execute => EditShelfVendor(), canExecute => CanEditOrDelete());
            DeleteShelfVendorCommand = new RelayCommand(execute => DeleteShelfVendor(), canExecute => CanEditOrDelete());
            _methodRepository = methodRepository;
        }

        //Metoder, til brug for Combobox betalingsoplysninger

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Metode til at tilføje en ny sælger.
        private void AddShelfVendor()
        {
            var newVendor = new ShelfVendor
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                PhoneNumber = this.PhoneNumber,
                Email = this.Email
            };

            // Henter ID direkte fra DB efter det oprettes
            int newId = _repository.AddShelfVendor(newVendor); // Gemmer i DB.
            ShelfVendors.Add(newVendor); // Opdaterer listen i UI.

            var paymentMethod = new PaymentMethod
            {
                ShelfVendorID = newId,
                //SelectedPaymentOption kommer fra ComboBox i view, som sender det til property her
                PaymentOption = this.SelectedPaymentOption,
                PaymentInfo = this.PaymentInfo
            };

            _methodRepository.AddPaymentMethod(paymentMethod);

            ClearFields(); // Nulstiller input-felter.
        }

        // Metode til at redigere den valgte sælger.
        private void EditShelfVendor()
        {
            var vendorToUpdate = ShelfVendors.FirstOrDefault(v => v.ShelfVendorID == SelectedShelfVendor.ShelfVendorID);
            if (vendorToUpdate != null)
            {
                // Opdaterer objektet med data fra tekstboksene.
                vendorToUpdate.FirstName = this.FirstName;
                vendorToUpdate.LastName = this.LastName;
                vendorToUpdate.PhoneNumber = this.PhoneNumber;
                vendorToUpdate.Email = this.Email;

                _repository.UpdateShelfVendor(vendorToUpdate); // Gemmer ændringer i DB.

                // Nødvendigt for at UI'en opdaterer listen visuelt.
                int index = ShelfVendors.IndexOf(vendorToUpdate);
                ShelfVendors[index] = vendorToUpdate;
            }
        }

        // Metode til at slette den valgte sælger.
        private void DeleteShelfVendor()
        {
            if (SelectedShelfVendor != null)
            {
                // Viser en bekræftelsesdialog før sletning.
                if (MessageBox.Show($"Er du sikker på, at du vil slette {SelectedShelfVendor.FirstName}?", "Bekræft sletning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _repository.DeleteShelfVendor(SelectedShelfVendor); // Sletter fra DB.
                    ShelfVendors.Remove(SelectedShelfVendor); // Fjerner fra listen i UI.
                    ClearFields();
                }
            }
        }

        // Hjælpe-metode til at rydde alle input-felter.
        private void ClearFields()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
            SelectedShelfVendor = null;
            PaymentInfo = string.Empty;
        }

        // Betingelser, der afgør, om knapperne skal være aktive.
        private bool CanAdd() => !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(Email)
                                   && !string.IsNullOrEmpty(PaymentInfo) && !string.IsNullOrEmpty(SelectedPaymentOption);
        private bool CanEditOrDelete() => SelectedShelfVendor != null;
    }
}





