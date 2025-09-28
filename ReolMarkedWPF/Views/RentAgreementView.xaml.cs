using System.Windows.Controls;
using ReolMarkedWPF.ViewModels;

namespace ReolMarkedWPF.Views
{
    /// <summary>
    /// Interaction logic for ShelfVendorView.xaml
    /// </summary>
    public partial class RentAgreementView : Page
    {
        public RentAgreementView(RentAgreementViewModel viewModel)
        {
            // ViewModel sendes fra DIcontainer.cs
            InitializeComponent();
            DataContext = viewModel;
        }
       
    }
}
