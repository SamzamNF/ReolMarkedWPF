using Moq;
using ReolMarkedWPF.Models;
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.ViewModel;

namespace ReolMarkedWPF.Tests
{
    [TestClass]
    public class ShelfViewModelTests
    {
        private Mock<IShelfRepository> _mockShelfRepo;
        private ShelfViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _mockShelfRepo = new Mock<IShelfRepository>();

            // Returnerer en tom liste for at undgå null reference-fejl i ViewModel'ens konstruktør.
            _mockShelfRepo.Setup(repo => repo.GetAllShelves()).Returns(new List<Shelf>());

            _viewModel = new ShelfViewModel(_mockShelfRepo.Object);
        }

        [TestMethod]
        public void AddShelfCommand_CanExecute_WithValidInput_ReturnsTrue()
        {
            // Arrange: Simuler gyldigt brugerinput.
            _viewModel.ShelfType = "Standard hylde";
            _viewModel.Price = 200;

            // Act: Tjek om kommandoen kan udføres.
            bool canExecute = _viewModel.AddShelfCommand.CanExecute(null);

            // Assert: Forvent 'true', da input er gyldigt.
            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void AddShelfCommand_CanExecute_WhenShelfTypeIsEmpty_ReturnsFalse()
        {
            // Arrange: Simuler ugyldigt input.
            _viewModel.ShelfType = "";
            _viewModel.Price = 200;

            // Act: Tjek om kommandoen kan udføres.
            bool canExecute = _viewModel.AddShelfCommand.CanExecute(null);

            // Assert: Forvent 'false', da ShelfType mangler.
            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void DeleteShelfCommand_CanExecute_WhenNoShelfIsSelected_ReturnsFalse()
        {
            // Arrange: Sørg for at ingen reol er valgt.
            _viewModel.SelectedShelf = null;

            // Act: Tjek om 'Slet'-kommandoen kan udføres.
            bool canExecute = _viewModel.DeleteShelfCommand.CanExecute(null);

            // Assert: Forvent 'false', da intet er valgt.
            Assert.IsFalse(canExecute);
        }
    }
}