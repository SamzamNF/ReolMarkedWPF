using System.Windows.Controls;
using ReolMarkedWPF.ViewModels;

namespace ReolMarkedWPF.Views
{
    /// <summary>
    /// Interaction logic for RentAgreementView.xaml
    /// </summary>
    public partial class ShelfVendorView : Page
    {
        public ShelfVendorView(ShelfVendorViewModel viewModel)
        {
            // Datacontext kommer fra DIcontainer.cs
            // Det sendes med i parameter fra DIcontainer, da ShelfVendorViewModel er oprettet der samt view
            // Så DIcontainer ved, at når der bliver spurgt om "ShelfVendorViewModel" her, så skal det sendes herover.
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
