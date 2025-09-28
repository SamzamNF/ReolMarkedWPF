using System.Windows.Controls;
using ReolMarkedWPF.ViewModels;

namespace ReolMarkedWPF.Views
{
    /// <summary>
    /// Interaction logic for RentAgreementChooseShelfView.xaml
    /// </summary>
    public partial class RentAgreementChooseShelfView : Page
    {
        public RentAgreementChooseShelfView(RentAgreementViewModel viewmodel)
        {
            // ViewModel sendes fra DIcontainer.cs
            InitializeComponent();
            DataContext = viewmodel;
        }
    }
}
