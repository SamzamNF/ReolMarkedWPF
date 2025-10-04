using Moq;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.ViewModels;
using System.Collections.Generic;

namespace ReolMarkedWPF.Tests
{

    [TestClass]
    public class TransactionViewModelTests
    {
        // Mocks for alle afhængigheder
        private Mock<ITransactionRepository<Transaction>> _mockTransactionRepo;
        private Mock<IShelfRepository> _mockShelfRepo;
        private Mock<IProductRepository> _mockProductRepo;
        private Mock<ITransactionProductRepository> _mockTransactionProductRepo;
        private TransactionProductViewModel _transactionProductViewModel; // En rigtig instans, ikke en mock
        private TransactionViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            // Initialiserer alle mocks
            _mockTransactionRepo = new Mock<ITransactionRepository<Transaction>>();
            _mockShelfRepo = new Mock<IShelfRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockTransactionProductRepo = new Mock<ITransactionProductRepository>();

            // Sørger for, at kald til repository-metoder returnerer en tom liste i stedet for 'null'.
            // Dette forhindrer 'ArgumentNullException' under oprettelsen af ViewModels.
            _mockTransactionRepo.Setup(repo => repo.GetAllTransactions()).Returns(new List<Transaction>());
            _mockShelfRepo.Setup(repo => repo.GetAllShelves()).Returns(new List<Shelf>());
            _mockProductRepo.Setup(repo => repo.GetAllProducts()).Returns(new List<Product>());
            _mockTransactionProductRepo.Setup(repo => repo.GetAllTransactionProducts()).Returns(new List<TransactionProduct>());

            // Opretter en rigtig instans af TransactionProductViewModel, fordi den primært
            // indeholder state (collections), som vores TransactionViewModel skal interagere
            // direkte med.
            _transactionProductViewModel = new TransactionProductViewModel(_mockTransactionProductRepo.Object);

            // Opretter den ViewModel, der skal testes
            _viewModel = new TransactionViewModel(
                _mockTransactionRepo.Object,
                _mockShelfRepo.Object,
                _mockProductRepo.Object,
                _mockTransactionProductRepo.Object,
                _transactionProductViewModel
            );
        }

        [TestMethod]
        public void CanAddTransaction_WhenOrderDetailsHasItems_ReturnsTrue()
        {
            // Formål: Verificerer at 'Gennemfør salg'-knappen er aktiv, når kurven indeholder varer.
            // Arrange: Tilføj et produkt til "indkøbskurven" (OrderDetails collection).
            _viewModel.TpVm.OrderDetails.Add(new TransactionProduct { ProductID = 1, Amount = 1, UnitPrice = 100 });

            // Act: Tjek om AddTransactionCommand kan eksekveres.
            bool canAdd = _viewModel.AddTransactionCommand.CanExecute(null);

            // Assert: Kommandoen bør kunne eksekveres.
            Assert.IsTrue(canAdd);
        }

        [TestMethod]
        public void AddProductToTransaction_WhenProductHasStock_ReducesStockAndAddsToOrder()
        {
            // Formål: Verificerer at tilføjelse af et produkt reducerer lagerbeholdningen og tilføjer det til kurven.
            // Arrange: Opsæt et produkt med tilgængelig lagerbeholdning.
            var product = new Product { ProductID = 10, ProductName = "Test produkt", Amount = 5, ShelfNumber = 1 };
            _viewModel.Products.Add(product);
            _viewModel.ShelfProducts.Add(product);
            _viewModel.SelectedProduct = product;

            // Act: Eksekver kommandoen for at tilføje produktet til ordren.
            _viewModel.AddToOrderDetailsCommand.Execute(null);

            // Assert: Verificer at produktets lagerbeholdning er reduceret, og det nu er i kurven.
            Assert.AreEqual(4, product.Amount); // Lagerbeholdningen skal være én mindre
            Assert.AreEqual(1, _viewModel.TpVm.OrderDetails.Count); // Kurven skal have ét item
            Assert.AreEqual(10, _viewModel.TpVm.OrderDetails.First().ProductID); // Det korrekte produkt skal være i kurven
        }

        [TestMethod]
        public void AddProductToTransaction_WhenProductIsOutOfStock_DoesNothing()
        {
            // Formål: Sikrer at et produkt med nul på lager ikke kan tilføjes til kurven.
            // Arrange: Opsæt et produkt med nul på lager.
            var product = new Product { ProductID = 20, ProductName = "Udsolgt produkt", Amount = 0, ShelfNumber = 2 };
            _viewModel.Products.Add(product);
            _viewModel.ShelfProducts.Add(product);
            _viewModel.SelectedProduct = product;

            // Act: Forsøg at eksekvere kommandoen.
            _viewModel.AddToOrderDetailsCommand.Execute(null);

            // Assert: Verificer at lagerbeholdningen stadig er nul, og kurven er tom.
            Assert.AreEqual(0, product.Amount);
            Assert.AreEqual(0, _viewModel.TpVm.OrderDetails.Count);
        }
    }
}