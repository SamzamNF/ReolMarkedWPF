using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
