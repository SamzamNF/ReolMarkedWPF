using ReolMarkedWPF.Helpers;

namespace ReolMarkedWPF.Models
{
    // Klassen arver fra ViewModelBase for at kunne notificere UI'en om ændringer.
    // Ikke ren "løs kobling", men nødvendig for data binding i WPF.
    public class TransactionProduct : ViewModelBase
    {
        private int _amount;

        public int TransactionID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }

        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(); // Giver besked til UI, når antallet ændres
            }
        }
    }
}