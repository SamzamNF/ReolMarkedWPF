using System.Collections.ObjectModel;
using System.Linq;
using ReolMarkedWPF.Helpers;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;

namespace ReolMarkedWPF.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        // Repositories til at interagere med databasen
        private readonly IProductRepository _productRepository;
        private readonly IShelfVendorRepository _shelfVendorRepository;
        private readonly IShelfRepository _shelfRepository;
        private readonly IRentRepository<Rent> _rentRepository;

        // Private felter til at holde styr på UI-input
        private string _productName;
        private decimal _unitPrice;
        private int _amount;

        // Lister til datahåndtering
        private ObservableCollection<Product> _allProducts; // Holder alle produkter i hukommelsen
        private ObservableCollection<Product> _products; // Viser kun produkter for den valgte lejer
        private Product _selectedProduct;

        private ShelfVendor _selectedShelfVendor;
        private Shelf _selectedShelf;
        private ObservableCollection<ShelfVendor> _shelfVendors;
        private ObservableCollection<Shelf> _vendorShelves; // Viser kun reoler for den valgte lejer
        private bool _isVendorSelected; // Styrer om UI-elementer er aktive

        // Properties til databinding i XAML
        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(); }
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            set { _unitPrice = value; OnPropertyChanged(); }
        }

        public int Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set { _products = value; OnPropertyChanged(); }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                // Når et produkt vælges i DataGrid, opdateres input-felterne
                if (value != null)
                {
                    ProductName = value.ProductName;
                    UnitPrice = value.UnitPrice;
                    Amount = value.Amount;
                    // Vælger den korrekte reol i reol-dropdown'en
                    SelectedShelf = VendorShelves.FirstOrDefault(s => s.ShelfNumber == value.ShelfNumber);
                }
                OnPropertyChanged();
                // Fortæl alle knapper, at de skal gen-evaluere deres 'CanExecute' status
                AddProductCommand.RaiseCanExecuteChanged();
                DeleteProductCommand.RaiseCanExecuteChanged();
                EditProductCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<ShelfVendor> ShelfVendors
        {
            get => _shelfVendors;
            set { _shelfVendors = value; OnPropertyChanged(); }
        }

        public ShelfVendor SelectedShelfVendor
        {
            get => _selectedShelfVendor;
            set
            {
                _selectedShelfVendor = value;
                // Sætter flaget, der styrer om UI er aktivt
                IsVendorSelected = value != null;
                // Indlæser produkter og reoler for den valgte lejer
                LoadVendorSpecificData();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Shelf> VendorShelves
        {
            get => _vendorShelves;
            set { _vendorShelves = value; OnPropertyChanged(); }
        }

        public Shelf SelectedShelf
        {
            get => _selectedShelf;
            set { _selectedShelf = value; OnPropertyChanged(); }
        }

        public bool IsVendorSelected
        {
            get => _isVendorSelected;
            set { _isVendorSelected = value; OnPropertyChanged(); }
        }

        // Konstruktør: Initialiserer viewmodel og henter nødvendige data
        public ProductViewModel(IProductRepository productRepo, IShelfVendorRepository vendorRepo, IShelfRepository shelfRepo, IRentRepository<Rent> rentRepo)
        {
            _productRepository = productRepo;
            _shelfVendorRepository = vendorRepo;
            _shelfRepository = shelfRepo;
            _rentRepository = rentRepo;

            _allProducts = new ObservableCollection<Product>(_productRepository.GetAllProducts());
            Products = new ObservableCollection<Product>();
            ShelfVendors = new ObservableCollection<ShelfVendor>(_shelfVendorRepository.GetAllShelfVendors());
            VendorShelves = new ObservableCollection<Shelf>();
        }

        // Metode til at indlæse data (reoler og produkter) for den valgte lejer
        private void LoadVendorSpecificData()
        {
            Products.Clear();
            VendorShelves.Clear();
            ClearInputFields();

            if (SelectedShelfVendor == null) return;

            // Henter alle lejeaftaler og reoler
            var allRents = _rentRepository.GetAllRents();
            var allShelves = _shelfRepository.GetAllShelves();

            // Finder de lejeaftaler, der tilhører den valgte lejer
            var vendorRentIds = allRents
                .Where(r => r.ShelfVendorID == SelectedShelfVendor.ShelfVendorID)
                .Select(r => r.RentAgreementID)
                .ToList();

            // Finder de reoler, der er knyttet til lejerens lejeaftaler
            var vendorShelves = allShelves
                .Where(s => s.RentAgreementID.HasValue && vendorRentIds.Contains(s.RentAgreementID.Value))
                .ToList();

            foreach (var shelf in vendorShelves)
            {
                VendorShelves.Add(shelf);
            }

            // Finder de produkter, der er på lejerens reoler
            var vendorShelfNumbers = vendorShelves.Select(s => s.ShelfNumber).ToList();
            var vendorProducts = _allProducts.Where(p => vendorShelfNumbers.Contains(p.ShelfNumber));

            foreach (var product in vendorProducts)
            {
                Products.Add(product);
            }
        }

        // Metode til at tilføje et nyt produkt
        private void AddProduct()
        {
            var product = new Product
            {
                ShelfNumber = SelectedShelf.ShelfNumber,
                ProductName = this.ProductName,
                UnitPrice = this.UnitPrice,
                Amount = this.Amount
            };

            // Modtag det nye ID fra repository-laget
            int newId = _productRepository.AddProduct(product);
            // Tildel det korrekte ID til objektet, før det tilføjes til listerne
            product.ProductID = newId;

            _allProducts.Add(product);
            Products.Add(product);
            ClearInputFields();
        }

        // Metode til at slette det valgte produkt
        private void DeleteProduct()
        {
            if (SelectedProduct == null) return;

            _productRepository.DeleteProduct(SelectedProduct);
            _allProducts.Remove(SelectedProduct);
            Products.Remove(SelectedProduct);
            ClearInputFields();
        }

        // Metode til at redigere det valgte produkt
        private void EditProduct()
        {
            if (SelectedProduct == null) return;

            // Opdater direkte på det valgte produkt.
            // Product-klassen har INotifyPropertyChanged, og UI'en vil automatisk opdatere sig selv.
            SelectedProduct.ShelfNumber = SelectedShelf.ShelfNumber;
            SelectedProduct.ProductName = this.ProductName;
            SelectedProduct.UnitPrice = this.UnitPrice;
            SelectedProduct.Amount = this.Amount;

            // Gemmer ændringerne i databasen
            _productRepository.UpdateProduct(SelectedProduct);

            // Nulstiller felter og fjerner markering
            ClearInputFields();
        }

        // Hjælpe-metode til at rydde input-felter
        private void ClearInputFields()
        {
            ProductName = string.Empty;
            UnitPrice = 0;
            Amount = 0;
            SelectedShelf = null;
            SelectedProduct = null; // Udløser CanExecute-opdatering for knapperne
        }

        // Kommandoer som knapper i UI'en binder til
        public RelayCommand AddProductCommand => new RelayCommand(execute => AddProduct(), canExecute => CanAddProduct());
        public RelayCommand EditProductCommand => new RelayCommand(execute => EditProduct(), canExecute => CanEditOrDeleteProduct());
        public RelayCommand DeleteProductCommand => new RelayCommand(execute => DeleteProduct(), canExecute => CanEditOrDeleteProduct());

        // Betingelser, der afgør, om knapperne skal være aktive
        private bool CanAddProduct() =>
            // Tilføjet betingelse om, at intet produkt må være valgt
            IsVendorSelected &&
            SelectedShelf != null &&
            !string.IsNullOrWhiteSpace(ProductName) &&
            UnitPrice > 0 &&
            Amount >= 0 && // Tillad 0 i antal
            SelectedProduct == null;

        private bool CanEditOrDeleteProduct() =>
            IsVendorSelected &&
            SelectedProduct != null;
    }
}