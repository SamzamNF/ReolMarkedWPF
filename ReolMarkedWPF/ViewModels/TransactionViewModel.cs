using System.Collections.ObjectModel;
using System.Windows;
using ReolMarkedWPF.Helpers;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
#nullable disable


namespace ReolMarkedWPF.ViewModels
{
    public class TransactionViewModel : ViewModelBase
    {
        // ViewModel (Ved ikke om det er løs kobling?)
        private readonly TransactionProductViewModel _transactionProductViewModel;
        // Denne bruges til bindings af TransactionProductViewModel
        public TransactionProductViewModel TpVm => _transactionProductViewModel;


        // Repositories
        private readonly ITransactionRepository<Transaction> _transactionRepository;
        private readonly IShelfRepository _shelfRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITransactionProductRepository _transactionProductRepository;

        // Felter
        private DateOnly _transactionDate;
        private ObservableCollection<Transaction> _transactions;
        private ObservableCollection<Shelf> _shelves;
        private ObservableCollection<Product> _products;
        private ObservableCollection<Product> _shelfProducts;
        private Transaction _selectedtransaction;
        private Shelf _selectedShelf;
        private Product _selectedProduct;



        // Properties 
        public DateOnly TransactionDate
        {
            get => _transactionDate;
            set
            {
                _transactionDate = value;
                OnPropertyChanged();
            }
        }
        // Vælger en transaktion
        public Transaction SelectedTransaction
        {
            get => _selectedtransaction;
            set
            {
                _selectedtransaction = value;
                OnPropertyChanged();
                // Sætter tekstbokse til at automatisk være det, som SelectedTransaction objektet er
                this.TransactionDate = value.TransactionDate;
            }
        }
        // Vælger en Reol (Til en combobox evt) - Opdatere automatisk listen med ShelfProducts til at kun have produkter,
        // der hører til den valgte reol
        public Shelf SelectedShelf
        {
            get => _selectedShelf;
            set
            {
                _selectedShelf = value;
                OnPropertyChanged();
                UpdateProductShelfList();
            }
        }
        
        // Vælger produkt som så kan tilføjes til indkøbskurven
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }


        // Lister
        
        // Holder listen over transaktioner
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set
            {
                _transactions = value;
                OnPropertyChanged();
            }
        }

        // Holder listen over reoler som bruges til en combobox, hvor man kan vælge en shelf
        public ObservableCollection<Shelf> Shelves
        {
            get => _shelves;
            set
            {
                _shelves = value;
                OnPropertyChanged();
            }
        }

        // Holder på listen over produkter, som sættes til at loade efter den valgte reol (SelectedShelf)
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }
        // Listen til at holde på selve de produkter der hører under SelecedShelf's combobox
        public ObservableCollection<Product> ShelfProducts
        {
            get => _shelfProducts;
            set
            {
                _shelfProducts = value;
                OnPropertyChanged();
            }
        }

        // Konstruktør
        public TransactionViewModel(ITransactionRepository<Transaction> tRepository, IShelfRepository sRepository,
                                    IProductRepository pRepository,
                                    ITransactionProductRepository tPrepository, 
                                    TransactionProductViewModel transactionProductViewModel)
        {
            this._transactionRepository = tRepository;
            this._shelfRepository = sRepository;
            this._productRepository = pRepository;
            this._transactionProductRepository = tPrepository;

            this._transactionProductViewModel = transactionProductViewModel;

            Transactions = new ObservableCollection<Transaction>(_transactionRepository.GetAllTransactions());         
            Shelves = new ObservableCollection<Shelf>(_shelfRepository.GetAllShelves());
            Products = new ObservableCollection<Product>(_productRepository.GetAllProducts());
            ShelfProducts = new ObservableCollection<Product>();

        }

        // Metoder

        // Metode som tilføjer en transaktion samt transkationproducter til DB og lister
        private void AddTransaction()
        {
            var transaction = new Transaction
            {
                TransactionDate = DateOnly.FromDateTime(DateTime.Today)
            };

            // Transaktionen tilføjes til DB og returnere et ID
            int newId = _transactionRepository.AddTransaction(transaction);
            
            // Det oprettede ID fra DB sættes på objektet
            transaction.TransactionID = newId;
            
            // Tilføjer transkationen til listen
            Transactions.Add(transaction);

            // Bruger nu listen "OrderDetails" (Som holder alle TransaktionProdukter) for at sætte ID på alle tingene der er sat ind i indkøbslisten
            foreach (var product in TpVm.OrderDetails)
            {
                product.TransactionID = newId;
                _transactionProductRepository.AddTransactionProduct(product);
            }

            TpVm.OrderDetails.Clear();

            TransactionDate = default;

        }

        
        // Metode som sletter transaktion fra DB & memory ---- BRUGER CASCADE PÅ TRANSAKTIONPRODUCT OG SLETTER ALT FORBUNDET
        private void DeleteTransaction()
        {
            // Null-conditional operator (?.) sikrer at Hvis SelectedTransaction ikke er null, så prøv at hente TransactionID
            // Hvis den er null (SelectedTransaction) - så returnes null i stedet for at kaste en NullReferenceException
            var transactionToDelete = Transactions
                                            .FirstOrDefault(t => t.TransactionID == SelectedTransaction?.TransactionID);

            if (transactionToDelete != null)
            {
                if (MessageBox.Show($"Er du sikker på, at du vil slette transaktionen med ID - " +
                    $"DETTE ER PERMANENT og sletter alle forbindelser: {SelectedTransaction.TransactionID}?",
                "Bekræft sletning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _transactionRepository.DeleteTransaction(transactionToDelete);
                        Transactions.Remove(transactionToDelete);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // Metode til at redigere i en transkation (Mangler implemenation af redigering af produkter)
        private void EditTransaction()
        {
            // Null-conditional operator (?.) sikrer at Hvis SelectedTransaction ikke er null, så prøv at hente TransactionID
            // Hvis den er null (SelectedTransaction) - så returnes null i stedet for at kaste en NullReferenceException
            var transactionToEdit = Transactions
                                           .FirstOrDefault(t => t.TransactionID == SelectedTransaction?.TransactionID);

            if (transactionToEdit != null)
            {
                transactionToEdit.TransactionDate = this.TransactionDate;

                try
                {
                    _transactionRepository.UpdateTransaction(transactionToEdit);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
        }

        // Denne metode bruges til at tilføje produkter til en liste (Man tilføjer en af gangen
        // Tjekker også om et produkt allerede findes, hvis det gør får den +1 amount
        private void AddProductToTransaction()
        {
            // Henter det valgte produkt fra listen "ShelfProducts"
            var productToAdd = ShelfProducts
                                   .FirstOrDefault(p => p.ProductID == SelectedProduct?.ProductID);

            // Tjekker at det valgte objekt ikke er null, selvom det ikke burde være muligt og at den har mere end 0 i stock
            if (productToAdd != null && productToAdd.Amount > 0)
            {
                // Trækker 1 fra det valgte produkt
                productToAdd.Amount--;

                // Tjekker om produktet allerede er i kurven
                var existingItem = TpVm.OrderDetails
                                        .FirstOrDefault(oD => oD.ProductID == productToAdd.ProductID);

                if (existingItem != null)
                {
                    // Sætter antallet på produktet i kurven til +1
                    existingItem.Amount++;
                }
                else
                {
                    // Tilføjer nyt produkt til orderdetails
                    var tP = new TransactionProduct
                    {
                        ProductID = productToAdd.ProductID,
                        UnitPrice = productToAdd.UnitPrice,
                        Amount = 1
                    };
                    TpVm.OrderDetails.Add(tP);
                }
            }
        }
        // Denne metode fjerner et produkt fra listen (indkøbskurven)
        private void RemoveProductFromList()
        {
            // Tjekker at det valgte TransaktionProdukt i orderdetail ikke er tomt.
            if (TpVm.SelectedOrderDetail != null)
            {
                // Finder det matchene produkt i HELE listen af produkter
                var matchingProduct = Products
                                         .FirstOrDefault(p => p.ProductID == TpVm.SelectedOrderDetail?.ProductID);

                // Tilføjer den antal af produktet tilbage, på det originale produkt som man nu sletter fra indkøbslisten
                if (matchingProduct != null)
                {
                    matchingProduct.Amount += TpVm.SelectedOrderDetail.Amount;
                }

                // Fjerner det fra listen
                TpVm.OrderDetails.Remove(TpVm.SelectedOrderDetail);

                // Sætter SelecetedOrderDetail til at være null, indtil et nyt produkt vælges
                TpVm.SelectedOrderDetail = null;
            }
        }

        // Metode til at hente alle produkter ind i en specifik reol, når den vælges
        private void UpdateProductShelfList()
        {
            if (SelectedShelf != null)
            {
                var filteredProducts = Products
                                           .Where(p => p.ShelfNumber == SelectedShelf.ShelfNumber)
                                           .ToList();

                // Clear den nuværende liste af ShelfProdukter
                ShelfProducts.Clear();
                
                // Tilføjer alle produkterne til listen
                foreach (var product in filteredProducts)
                {
                    ShelfProducts.Add(product);
                }

            }
            // Fjerner alt fra listen, selvom der ikke er en valgt shelf
            else
            {
                ShelfProducts.Clear();
            }
        }


        // Knapper 

        public RelayCommand AddTransactionCommand => new RelayCommand(execute => AddTransaction(), canExecute => CanAddTransaction());
        public RelayCommand DeleteTransactionCommand => new RelayCommand(execute => DeleteTransaction(), canExecute => CanDeleteTransaction());
        public RelayCommand EditTrasnactionCommand => new RelayCommand(exeute => EditTransaction(), canExecute => CanEditTransaction());
        
        // Knap til at tilføje ting til "orderdetails" (indkøbskurven)
        public RelayCommand AddToOrderDetailsCommand => new RelayCommand(execute => AddProductToTransaction(), canExecute => CanAddToOrderDetails());
        // Knap til at fjerne ting fra "orderdetails" (indkøbskurven)
        public RelayCommand RemoveFromOrderDetailsCommand => new RelayCommand(execute => RemoveProductFromList(), canExecute => CanRemoveFromOrderDetails());

        // Conditions til knapper

        private bool CanAddTransaction() => TpVm.OrderDetails.Count > 0;
        private bool CanDeleteTransaction() => SelectedTransaction != null;
        private bool CanEditTransaction() => SelectedTransaction != null && 
                                             TransactionDate != default;
        private bool CanAddToOrderDetails() => SelectedProduct != null;
        private bool CanRemoveFromOrderDetails() => TpVm.SelectedOrderDetail != null;
    }
}
