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
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Helpers;

namespace ReolMarkedWPF.Views
{
    /// <summary>
    /// Interaction logic for RentAgreementView.xaml
    /// </summary>
    public partial class RentAgreementView : Page
    {
        public RentAgreementView()
        {
            InitializeComponent();
            // Get the same navigation service instance that MainViewModel uses
            var navigationService = ((App)App.Current).NavigationService;
            // Create repository instance
            var rentRepository = new SqlRentRepository();
            
            DataContext = new RentAgreementViewModel(rentRepository, navigationService);
        }

    }
}
