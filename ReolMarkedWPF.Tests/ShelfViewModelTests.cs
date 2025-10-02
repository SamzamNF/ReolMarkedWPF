// Importer de nødvendige biblioteker.
// 'Microsoft.VisualStudio.TestTools.UnitTesting' er MSTest-frameworket.
// 'Moq' er vores mocking-bibliotek.
// 'ReolMarkedWPF.ViewModel' er for at kunne tilgå den klasse, vi vil teste.
using Moq;
using ReolMarkedWPF.Repositories;
using ReolMarkedWPF.ViewModel;

// Angiver at denne klasse indeholder unit tests.
[TestClass]
public class ShelfViewModelTests
{
    // Vi erklærer vores "mock"-objekt og ViewModel her, så de er tilgængelige i alle testmetoder.
    private Mock<IShelfRepository> _mockShelfRepo;
    private ShelfViewModel _viewModel;

    // Denne metode kører FØR hver eneste test i klassen.
    // Det sikrer, at hver test starter med et "rent" ViewModel-objekt.
    [TestInitialize]
    public void Setup()
    {
        // Opret en ny mock af vores repository-interface.
        // En mock lader os simulere databasen uden rent faktisk at have en databaseforbindelse.
        _mockShelfRepo = new Mock<IShelfRepository>();

        // Opret en ny instans af den ViewModel, vi vil teste.
        // Vi giver den vores mock-objekt i stedet for det rigtige repository.
        _viewModel = new ShelfViewModel(_mockShelfRepo.Object);
    }

    // Angiver at dette er en testmetode.
    // Navnet på metoden følger en god konvention:
    // [MetodeNavn]_[Scenarie]_[ForventetResultat]
    [TestMethod]
    public void AddShelfCommand_CanExecute_WithValidInput_ReturnsTrue()
    {
        // ----- ARRANGE -----
        // I "Arrange"-fasen sætter vi scenariet op. Vi forbereder de data
        // og den tilstand, som vores test kræver.

        // Vi simulerer, at brugeren har udfyldt felterne i UI'en med gyldige værdier.
        _viewModel.ShelfType = "Standard hylde";
        _viewModel.Price = 200;

        // ----- ACT -----
        // I "Act"-fasen udfører vi den handling, vi rent faktisk vil teste.

        // Vi kalder CanExecute på vores AddShelfCommand for at se, hvad den returnerer
        // baseret på de værdier, vi satte i Arrange-fasen.
        bool kanUdføre = _viewModel.AddShelfCommand.CanExecute(null);

        // ----- ASSERT -----
        // I "Assert"-fasen verificerer vi, at resultatet af vores handling er som forventet.

        // Vi forventer, at `CanExecute` returnerer 'true', fordi både ShelfType og Price er gyldige.
        // Assert.IsTrue vil lade testen bestå, hvis `kanUdføre` er true, og fejle hvis den er false.
        Assert.IsTrue(kanUdføre);
    }

    [TestMethod]
    public void AddShelfCommand_CanExecute_WhenShelfTypeIsEmpty_ReturnsFalse()
    {
        // ----- ARRANGE -----
        // Vi sætter et "negativt" scenarie op, hvor input er ugyldigt.
        _viewModel.ShelfType = ""; // Tom streng
        _viewModel.Price = 200;

        // ----- ACT -----
        // Vi udfører den samme handling som før.
        bool kanUdføre = _viewModel.AddShelfCommand.CanExecute(null);

        // ----- ASSERT -----
        // Denne gang forventer vi, at resultatet er 'false', fordi ShelfType er tom.
        Assert.IsFalse(kanUdføre);
    }

    [TestMethod]
    public void DeleteShelfCommand_CanExecute_WhenNoShelfIsSelected_ReturnsFalse()
    {
        // ----- ARRANGE -----
        // Standarden er, at ingen hylde er valgt, så vi behøver ikke at sætte noget op.
        // _viewModel.SelectedShelf er null fra start.

        // ----- ACT -----
        // Vi tjekker, om 'Slet'-knappen ville være aktiv.
        bool kanUdføre = _viewModel.DeleteShelfCommand.CanExecute(null);

        // ----- ASSERT -----
        // Vi forventer, at den er inaktiv, da der ikke er valgt nogen hylde.
        Assert.IsFalse(kanUdføre);
    }
}