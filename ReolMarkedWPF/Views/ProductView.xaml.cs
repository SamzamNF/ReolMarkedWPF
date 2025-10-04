using ReolMarkedWPF.ViewModels;
using System.Windows.Controls;

namespace ReolMarkedWPF.Views
{
    public partial class ProductView : Page
    {
        public ProductView(ProductViewModel viewModel)
        {
            InitializeComponent();
            // DataContext sættes til den ViewModel, der kommer fra Dependency Injection containeren
            DataContext = viewModel;
        }
    }
}