using Moq;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.ViewModels;

namespace ReolMarkedWPF.Tests
{
    [TestClass]
    public class ShelfVendorViewModelTests
    {
        // Erklær mocks for BEGGE afhængigheder.
        private Mock<IShelfVendorRepository> _mockVendorRepo;
        private Mock<IPaymentMethodRepository> _mockPaymentMethodRepo;
        private ShelfVendorViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            // Opret en ny mock for hver afhængighed.
            _mockVendorRepo = new Mock<IShelfVendorRepository>();
            _mockPaymentMethodRepo = new Mock<IPaymentMethodRepository>();

            // Giv begge mock-objekter til ViewModel'ens konstruktør.
            _viewModel = new ShelfVendorViewModel(_mockVendorRepo.Object, _mockPaymentMethodRepo.Object);
        }

        [TestMethod]
        public void AddShelfVendorCommand_Execute_CallsAddShelfVendorOnRepository()
        {
            // ----- ARRANGE -----
            // Sæt gyldige data på ViewModel'en, som om brugeren har udfyldt formularen.
            _viewModel.FirstName = "Test";
            _viewModel.LastName = "Person";
            _viewModel.Email = "test@person.dk";
            _viewModel.PhoneNumber = "12345678";
            _viewModel.SelectedPaymentOption = "MobilePay";
            _viewModel.PaymentInfo = "87654321";

            // Vi kan opsætte en mock til at "opføre sig" på en bestemt måde.
            // Her siger vi, at når AddShelfVendor-metoden kaldes på vores mock-repository,
            // skal den returnere '1' (som om det var et nyt ID fra databasen).
            _mockVendorRepo.Setup(repo => repo.AddShelfVendor(It.IsAny<ShelfVendor>())).Returns(1);

            // ----- ACT -----
            // Udfør kommandoen. Dette vil kalde AddShelfVendor-metoden inde i ViewModel'en.
            _viewModel.AddShelfVendorCommand.Execute(null);

            // ----- ASSERT -----
            // I stedet for at tjekke en returværdi, verificerer vi nu, at en bestemt metode
            // på vores mock-objekt blev kaldt.

            // Verify, at AddShelfVendor-metoden på vores mock-repository blev kaldt præcis én gang.
            // Dette bekræfter, at vores logik i ViewModel'en virker som forventet.
            _mockVendorRepo.Verify(repo => repo.AddShelfVendor(It.IsAny<ShelfVendor>()), Times.Once());

            // Vi kan også verificere, at betalingsmetoden blev gemt.
            _mockPaymentMethodRepo.Verify(repo => repo.AddPaymentMethod(It.IsAny<PaymentMethod>()), Times.Once());
        }

        [TestMethod]
        public void CanAdd_IsFalse_WhenPaymentInfoIsEmpty()
        {
            // ----- ARRANGE -----
            // Udfyld alt undtagen den information, vi vil teste for.
            _viewModel.FirstName = "Test";
            _viewModel.Email = "test@person.dk";
            _viewModel.SelectedPaymentOption = "MobilePay";
            _viewModel.PaymentInfo = ""; // Ugyldigt input

            // ----- ACT -----
            // CanExecute er privat i denne ViewModel, så vi tester den indirekte
            // via kommandoens CanExecute.
            bool kanTilføje = _viewModel.AddShelfVendorCommand.CanExecute(null);

            // ----- ASSERT -----
            // Forvent at knappen er deaktiveret.
            Assert.IsFalse(kanTilføje);
        }
    }
}