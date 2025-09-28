using ReolMarkedWPF.Models;
using System.Collections.ObjectModel;
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.Helpers;

namespace ReolMarkedWPF.ViewModels
{
    //Viewmodel til TransactionProduct

    public class TransactionProductViewModel : ViewModelBase
    {
        private ObservableCollection<TransactionProduct> _orderDetails;
        private ObservableCollection<TransactionProduct> _allOrderDetails;


        // Denne liste bruges til at holde på "indkøbskurven" i TransactionViewModel
        public ObservableCollection<TransactionProduct> OrderDetails
        {
            get => _orderDetails;
            set
            {
                _orderDetails = value;
                OnPropertyChanged();
            }

        }
        // Denne liste holder på ALLE TranskationProdukter, som kan bruges til det samlede datagrid i view
        public ObservableCollection<TransactionProduct> AllOrderDetails
        {
            get => _allOrderDetails;
            set
            {
                _allOrderDetails = value;
                OnPropertyChanged();
            }

        }
        private readonly ITransactionProductRepository _transactionProductRepository;

        //Til brug i viewet
        private TransactionProduct _selectedOrderDetail;
        public TransactionProduct SelectedOrderDetail
        {
            get => _selectedOrderDetail;
            set
            {
                _selectedOrderDetail = value;
                OnPropertyChanged();
            }
        }

        //Constructor

        public TransactionProductViewModel(ITransactionProductRepository repository)
        {
            this._transactionProductRepository = repository;
            AllOrderDetails = new ObservableCollection<TransactionProduct>(_transactionProductRepository.GetAllTransactionProducts());
            // Tilføjelse: Oprettelse af tom liste
            OrderDetails = new ObservableCollection<TransactionProduct>();
            SelectedOrderDetail = null;
        }
    }
}
