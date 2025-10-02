using System.Windows.Controls;
using ReolMarkedWPF.ViewModels.AccountingViewModels;

namespace ReolMarkedWPF.Views
{
    /// <summary>
    /// Interaction logic for AccountingView.xaml
    /// </summary>
    public partial class AccountingView : Page
    {
        public AccountingView(AccountingViewModel viewmodel)
        {
            InitializeComponent();
            DataContext = viewmodel;
        }
    }
}
