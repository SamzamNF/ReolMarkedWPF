using ReolMarkedWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ReolMarkedWPF.Repositories;

namespace ReolMarkedWPF.ViewModels
{
    //Viewmodel til TransactionProduct

    public class TransactionProductViewModel
    {
        private ObservableCollection<TransactionProduct> orderDetails;
        public ObservableCollection<TransactionProduct> OrderDetails
        {
            get { return orderDetails; }
            set { orderDetails = value; }
        }
        private ObservableCollection<ITransactionProductRepository> _rentViewModel;
        public ObservableCollection<ITransactionProductRepository> RentViewModel
        {
            get { return _rentViewModel; }
            set { _rentViewModel = value; }
        }

        //Til brug i viewet

        private TransactionProduct _selectedOrderDetail;
        public TransactionProduct SelectedOrderDetail
        {
            get => _selectedOrderDetail;
            set
            {
                _selectedOrderDetail = value;
                //NotifyPropertyChanged(nameof(SelectedOrderDetail)); (bind til XAML senere)
            }
        }

        //Constructor

        public TransactionProductViewModel(ITransactionProductRepository repository)
        {
            OrderDetails = new ObservableCollection<TransactionProduct>();
            RentViewModel = new ObservableCollection<ITransactionProductRepository>();
            SelectedOrderDetail = null;
        }
    }
}
