using System;
using System.Collections.ObjectModel;
using System.Windows;
using Csla;
using ReolMarkedWPF.Helpers;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;

namespace ReolMarkedWPF.ViewModel
{
    public class ShelfViewModel : ViewModelBase
    {
        private readonly IShelfRepository _shelfRepository;

        private int _shelfNumber;
        private int _rentAgreementID;
        private string _shelfType;
        private decimal _price;
        private ObservableCollection<Shelf> _shelves;
        private Shelf _selectedShelf;

        public int ShelfNumber
        {
            get => _shelfNumber;
            set
            {
                _shelfNumber = value;
                OnPropertyChanged();    // Fortæller UI'et at værdien er ændret
            }
        }

        public int RentAgreementID
        {
            get => _rentAgreementID;
            set
            {
                _rentAgreementID = value;
                OnPropertyChanged();
            }
        }

        public string ShelfType
        {
            get => _shelfType;
            set
            {
                _shelfType = value;
                OnPropertyChanged();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Shelf> Shelves
        {
            get => _shelves;
            set
            {
                _shelves = value;
                OnPropertyChanged();
            }
        }

        public Shelf SelectedShelf
        {
            get => _selectedShelf;
            set
            {
                _selectedShelf = value;
                OnPropertyChanged();

                // Refresh commands når selection ændres
                OnPropertyChanged(nameof(EditShelfCommand));
                OnPropertyChanged(nameof(DeleteShelfCommand));
            }
        }

        public ShelfViewModel(IShelfRepository repository)
        {
            this._shelfRepository = repository;
            Shelves = new ObservableCollection<Shelf>(_shelfRepository.GetAllShelves());
        }

        private void AddShelf()
        {
            var shelf = new Shelf
            {
                ShelfNumber = this.ShelfNumber,
                ShelfType = this.ShelfType,
                Price = this.Price,
                RentAgreementID = this.RentAgreementID
            };

            _shelfRepository.AddShelf(shelf);
            Shelves.Add(shelf);
        }

        private void DeleteShelf()
        {
            if (SelectedShelf == null) return;

            _shelfRepository.DeleteShelf(SelectedShelf);
            Shelves.Remove(SelectedShelf);
            SelectedShelf = null;
        }

        private void EditShelf()
        {
            if (SelectedShelf == null) return;

            // Opdater selected shelf med nye værdier fra input fields
            SelectedShelf.ShelfNumber = this.ShelfNumber;
            SelectedShelf.ShelfType = this.ShelfType;
            SelectedShelf.Price = this.Price;
            SelectedShelf.RentAgreementID = this.RentAgreementID;

            _shelfRepository.UpdateShelf(SelectedShelf);

            // Refresh UI
            OnPropertyChanged(nameof(Shelves));
        }

        // Lazy loading: Backing fields - gemmer de faktiske RelayCommand objekter i hukommelsen.
        // Lazy loading betyder basically bare "lad være med at lave noget før det er nødvendigt".
        private RelayCommand _addShelfCommand;
        private RelayCommand _editShelfCommand;
        private RelayCommand _deleteShelfCommand;

        public RelayCommand AddShelfCommand =>
            _addShelfCommand ??= new RelayCommand(execute => AddShelf(), canExecute => CanAddShelf());

        public RelayCommand EditShelfCommand =>
            _editShelfCommand ??= new RelayCommand(execute => EditShelf(), canExecute => CanEditShelf());

        public RelayCommand DeleteShelfCommand =>
            _deleteShelfCommand ??= new RelayCommand(execute => DeleteShelf(), canExecute => CanDeleteShelf());


        private bool CanAddShelf() =>
            !string.IsNullOrWhiteSpace(ShelfType) && Price > 0 && ShelfNumber > 0;

        private bool CanDeleteShelf() =>
            SelectedShelf != null;

        private bool CanEditShelf() =>
            SelectedShelf != null && !string.IsNullOrWhiteSpace(ShelfType) && Price > 0 && ShelfNumber > 0;
    }
}
