using Moq;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.ViewModels;

namespace ReolMarkedWPF.Tests
{
    [TestClass]
    public class ShelfVendorViewModelTests
    {
        private Mock<IShelfVendorRepository> _mockVendorRepo;
        private Mock<IPaymentMethodRepository> _mockPaymentMethodRepo;
        private ShelfVendorViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _mockVendorRepo = new Mock<IShelfVendorRepository>();
            _mockPaymentMethodRepo = new Mock<IPaymentMethodRepository>();

            // Returnerer en tom liste for at undgå null reference-fejl i viewmodel konstruktør.
            _mockVendorRepo.Setup(repo => repo.GetAllShelfVendors()).Returns(new List<ShelfVendor>());

            _viewModel = new ShelfVendorViewModel(_mockVendorRepo.Object, _mockPaymentMethodRepo.Object);
        }

        [TestMethod]
        public void AddShelfVendorCommand_Execute_CallsAddShelfVendorOnRepository()
        {
            // Arrange: Opsæt gyldige data for en ny lejer.
            _viewModel.FirstName = "Test";
            _viewModel.LastName = "Person";
            _viewModel.Email = "test@person.dk";
            _viewModel.PhoneNumber = "12345678";
            _viewModel.SelectedPaymentOption = "MobilePay";
            _viewModel.PaymentInfo = "87654321";

            _mockVendorRepo.Setup(repo => repo.AddShelfVendor(It.IsAny<ShelfVendor>())).Returns(1);

            // Act: Udfør kommandoen.
            _viewModel.AddShelfVendorCommand.Execute(null);

            // Assert: Verificer at både lejer og betalingsmetode blev gemt.
            _mockVendorRepo.Verify(repo => repo.AddShelfVendor(It.IsAny<ShelfVendor>()), Times.Once());
            _mockPaymentMethodRepo.Verify(repo => repo.AddPaymentMethod(It.IsAny<PaymentMethod>()), Times.Once());
        }

        [TestMethod]
        public void CanAdd_IsFalse_WhenPaymentInfoIsEmpty()
        {
            // Arrange: Udfyld alle felter undtagen PaymentInfo.
            _viewModel.FirstName = "Test";
            _viewModel.Email = "test@person.dk";
            _viewModel.SelectedPaymentOption = "MobilePay";
            _viewModel.PaymentInfo = ""; // Ugyldigt input

            // Act: Tjek om kommandoen kan udføres.
            bool kanTilføje = _viewModel.AddShelfVendorCommand.CanExecute(null);

            // Assert: Forvent at kommandoen er deaktiveret.
            Assert.IsFalse(kanTilføje);
        }
    }
}