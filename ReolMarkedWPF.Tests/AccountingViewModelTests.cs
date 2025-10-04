using Moq;
using ReolMarkedWPF.Models.AccountingModels;
using ReolMarkedWPF.Repositories.AccountingRepository;
using ReolMarkedWPF.ViewModels.AccountingViewModels;

namespace ReolMarkedWPF.Tests
{
    [TestClass]
    public class AccountingViewModelTests
{
    // Felter til mocks og ViewModel
    private Mock<IAccountingRepository> _mockAccountingRepo;
    private AccountingViewModel _viewModel;

    // Initialiserer mocks og ViewModel før hver test. Hver test kører med en frisk instans.
    [TestInitialize]
    public void Setup()
    {
        _mockAccountingRepo = new Mock<IAccountingRepository>();
        _viewModel = new AccountingViewModel(_mockAccountingRepo.Object);
    }

    // SKABELON til testmetoder:
    // [UnitOfWork]_[Scenario]_[ExpectedBehavior]

    [TestMethod]
    public void CanCallAccountData_WhenDatesAreValid_ReturnsTrue()
    {
        // Tester om Søg-knappen er aktiv, når bruger vælger gyldigt datointerval.
        // Arrange
        _viewModel.StartDate = new DateTime(2025, 1, 1);
        _viewModel.EndDate = new DateTime(2025, 1, 31);

        // Act
        bool canCall = _viewModel.CallAccountingData.CanExecute(null);

        // Assert
        Assert.IsTrue(canCall);
    }

    [TestMethod]
    public void CanCallAccountData_WhenEndDateIsBeforeStartDate_ReturnsFalse()
    {
        // Tester om Søg-knappen er inaktiv, når bruger vælger ugyldigt datointerval.
        // Arrange
        _viewModel.StartDate = new DateTime(2025, 2, 1);
        _viewModel.EndDate = new DateTime(2025, 1, 31);

        // Act
        bool canCall = _viewModel.CallAccountingData.CanExecute(null);

        // Assert
        Assert.IsFalse(canCall);
    }

    [TestMethod]
    public async Task GetResults_WhenCalled_PopulatesResultsCollection()
    {
        // Tester om GetResults korrekt henter og fylder Results collection med data fra repository.
        // Arrange
        // 1. Opretter fake data som repository skal returnere.
        var fakeResults = new List<AccountingResult>
        {
            new AccountingResult { ShelfVendorID = 1, TotalSale = 1000 },
            new AccountingResult { ShelfVendorID = 2, TotalSale = 2000 }
        };
        _viewModel.StartDate = new DateTime(2025, 1, 1);
        _viewModel.EndDate = new DateTime(2025, 1, 31);
        // 2. Konfigurerer mock repository til at returnere fake data.
        _mockAccountingRepo
            .Setup(repo => repo.GetAccountingData(It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(fakeResults);

        // Act
        // Kalder asynkron metode for at hente data.
        await _viewModel.GetResults();

        // Assert
        // Verificerer at Results collection er fyldt korrekt.
        Assert.AreEqual(2, _viewModel.Results.Count);
        Assert.AreEqual(1000, _viewModel.Results[0].TotalSale);
    }
}
}