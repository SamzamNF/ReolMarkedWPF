using ReolMarkedWPF.Helpers;

namespace ReolMarkedWPF.Models
{
    // Klassen arver fra ViewModelBase for at kunne sende notifikationer til UI'en
    public class Product : ViewModelBase
    {
        private int _productID;
        private int _shelfNumber;
        private string _productName;
        private decimal _unitPrice;
        private int _amount;

        public int ProductID
        {
            get => _productID;
            set { _productID = value; OnPropertyChanged(); }
        }

        public int ShelfNumber
        {
            get => _shelfNumber;
            set { _shelfNumber = value; OnPropertyChanged(); }
        }

        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(); }
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            set { _unitPrice = value; OnPropertyChanged(); }
        }

        public int Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(); }
        }
    }
}