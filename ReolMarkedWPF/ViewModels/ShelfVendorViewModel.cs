using ReolMarkedWPF.Helpers;
using ReolMarkedWPF.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ReolMarkedWPF.ViewModel
{
    public class ShelfVendorViewModel : ViewModelBase
    {
        public ObservableCollection<ShelfVendor> ShelfVendors { get; set; }
        public ShelfVendor SelectedShelfVendor { get; set; }

        public ICommand AddShelfVendorCommand { get; }
        public ICommand DeleteShelfVendorCommand { get; }
        public ICommand EditShelfVendorCommand { get; }

        public ShelfVendorViewModel()
        {
            AddShelfVendorCommand = new RelayCommand(AddShelfVendor);
            DeleteShelfVendorCommand = new RelayCommand(DeleteShelfVendor);
            EditShelfVendorCommand = new RelayCommand(EditShelfVendor);
        }

        private void AddShelfVendor(object obj)
        {
            // Implementeres senere
        }

        private void DeleteShelfVendor(object obj)
        {
            // Implementeres senere
        }

        private void EditShelfVendor(object obj)
        {
            // Implementeres senere
        }
    }
}