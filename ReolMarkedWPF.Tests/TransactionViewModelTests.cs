using Moq;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.ViewModels;

[TestClass]
public class TransactionViewModelTests
{
    // Mocks for all dependencies
    private Mock<ITransactionRepository<Transaction>> _mockTransactionRepo;
    private Mock<IShelfRepository> _mockShelfRepo;
    private Mock<IProductRepository> _mockProductRepo;
    private Mock<ITransactionProductRepository> _mockTransactionProductRepo;
    private TransactionProductViewModel _transactionProductViewModel; // A real instance, not a mock
    private TransactionViewModel _viewModel;

    [TestInitialize]
    public void Setup()
    {
        // Initialize all mocks
        _mockTransactionRepo = new Mock<ITransactionRepository<Transaction>>();
        _mockShelfRepo = new Mock<IShelfRepository>();
        _mockProductRepo = new Mock<IProductRepository>();
        _mockTransactionProductRepo = new Mock<ITransactionProductRepository>();

        // We create a real instance of TransactionProductViewModel because it primarily holds state (collections)
        // that our TransactionViewModel needs to interact with directly.
        _transactionProductViewModel = new TransactionProductViewModel(_mockTransactionProductRepo.Object);

        // Create the ViewModel to be tested
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
        // Test Goal: Verify that the 'Complete Sale' button is enabled when the cart has items.

        // Arrange: Add a product to the "shopping cart" (OrderDetails collection).
        _viewModel.TpVm.OrderDetails.Add(new TransactionProduct { ProductID = 1, Amount = 1, UnitPrice = 100 });

        // Act: Check if the AddTransactionCommand can be executed.
        bool canAdd = _viewModel.AddTransactionCommand.CanExecute(null);

        // Assert: The command should be executable.
        Assert.IsTrue(canAdd);
    }

    [TestMethod]
    public void AddProductToTransaction_WhenProductHasStock_ReducesStockAndAddsToOrder()
    {
        // Test Goal: Verify that adding a product reduces its stock and adds it to the cart.

        // Arrange: Set up a product with available stock.
        var product = new Product { ProductID = 10, ProductName = "Test Product", Amount = 5, ShelfNumber = 1 };
        _viewModel.Products.Add(product);
        _viewModel.ShelfProducts.Add(product);
        _viewModel.SelectedProduct = product;

        // Act: Execute the command to add the product to the order.
        _viewModel.AddToOrderDetailsCommand.Execute(null);

        // Assert: Verify that the product's stock is reduced and it's now in the cart.
        Assert.AreEqual(4, product.Amount); // Stock should be one less
        Assert.AreEqual(1, _viewModel.TpVm.OrderDetails.Count); // Cart should have one item
        Assert.AreEqual(10, _viewModel.TpVm.OrderDetails.First().ProductID); // The correct product should be in the cart
    }

    [TestMethod]
    public void AddProductToTransaction_WhenProductIsOutOfStock_DoesNothing()
    {
        // Test Goal: Ensure that a product with zero stock cannot be added to the cart.

        // Arrange: Set up a product with zero stock.
        var product = new Product { ProductID = 20, ProductName = "Sold Out Product", Amount = 0, ShelfNumber = 2 };
        _viewModel.Products.Add(product);
        _viewModel.ShelfProducts.Add(product);
        _viewModel.SelectedProduct = product;

        // Act: Attempt to execute the command.
        _viewModel.AddToOrderDetailsCommand.Execute(null);

        // Assert: Verify that the stock is still zero and the cart is empty.
        Assert.AreEqual(0, product.Amount);
        Assert.AreEqual(0, _viewModel.TpVm.OrderDetails.Count);
    }
}