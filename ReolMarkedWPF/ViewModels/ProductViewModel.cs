using System.Collections.ObjectModel;
using ReolMarkedWPF.Helpers;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;

namespace ReolMarkedWPF.ViewModels
{
    internal class ProductViewModel : ViewModelBase
    {
        private readonly IProductRepository _productRepository;

        private int _productID;
        private int _shelfNumber;
        private string _productName;
        private decimal _unitPrice;
        private int _amount;
        private ObservableCollection<Product> _products;
        private Product _selectedProduct;

        public int ProductID
        {
            get => _productID;
            set
            {
                _productID = value;
                OnPropertyChanged();
            }
        }

        public int ShelfNumber
        {
            get => _shelfNumber;
            set
            {
                _shelfNumber = value;
                OnPropertyChanged();
            }
        }

        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged();
            }
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                _unitPrice = value;
                OnPropertyChanged();
            }
        }

        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();

                OnPropertyChanged(nameof(EditProductCommand));
                OnPropertyChanged(nameof(DeleteProductCommand));
            }
        }

        public ProductViewModel(IProductRepository repository)      // Der er en rød streg under ProductViewModel ?
        {
            this._productRepository = repository;
            Products = new ObservableCollection<Product>(_productRepository.GetAllProducts());
        }

        private void AddProduct()
        {
            var product = new Product
            {
                ProductID = this.ProductID,
                ShelfNumber = this.ShelfNumber,
                ProductName = this.ProductName,
                UnitPrice = this.UnitPrice,
                Amount = this.Amount
            };

            _productRepository.AddProduct(product);
            Products.Add(product);
        }

        private void DeleteProduct()
        {
            if (SelectedProduct == null) return;

            _productRepository.DeleteProduct(SelectedProduct);
            Products.Remove(SelectedProduct);
            SelectedProduct = null;
        }

        private void EditProduct()
        {
            if (SelectedProduct == null) return;

            SelectedProduct.ProductID = this.ProductID;
            SelectedProduct.ShelfNumber = this.ShelfNumber;
            SelectedProduct.ProductName = this.ProductName;
            SelectedProduct.UnitPrice = this.UnitPrice;
            SelectedProduct.Amount = this.Amount;

            _productRepository.UpdateProduct(SelectedProduct);

            OnPropertyChanged(nameof(Products));
        }

        // Lazy loading: Backing fields - gemmer de faktiske RelayCommand objekter i hukommelsen.
        // Lazy loading betyder basically bare "lad være med at lave noget før det er nødvendigt".
        private RelayCommand _addProductCommand;
        private RelayCommand _editProductCommand;
        private RelayCommand _deleteProductCommand;

        public RelayCommand AddProductCommand =>
            _addProductCommand ??= new RelayCommand(execute => AddProduct(), canExecute => CanAddProduct());

        public RelayCommand EditProductCommand =>
            _editProductCommand ??= new RelayCommand(execute => EditProduct(), canExecute => CanEditProduct());

        public RelayCommand DeleteProductCommand =>
            _deleteProductCommand ??= new RelayCommand(execute => DeleteProduct(), canExecute => CanDeleteProduct());


        private bool CanAddProduct() =>
            !string.IsNullOrWhiteSpace(ProductName) && UnitPrice > 0 && Amount > 0;

        private bool CanDeleteProduct() =>
            SelectedProduct != null;

        private bool CanEditProduct() =>
            SelectedProduct != null && !string.IsNullOrWhiteSpace(ProductName) && UnitPrice > 0 && Amount > 0;
    }
}
