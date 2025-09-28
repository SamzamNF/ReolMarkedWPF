using ReolMarkedWPF.ViewModels;
using System.Windows.Controls;

namespace ReolMarkedWPF.Views
{
    public partial class TransactionView : Page
    {
        public TransactionView(TransactionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}