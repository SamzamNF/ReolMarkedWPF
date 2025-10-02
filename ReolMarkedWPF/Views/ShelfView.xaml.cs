using System.Windows.Controls;
using ReolMarkedWPF.ViewModel;

namespace ReolMarkedWPF.Views
{
    /// <summary>
    /// Interaction logic for ShelfView.xaml
    /// </summary>
    public partial class ShelfView : Page
    {
        public ShelfView(ShelfViewModel viewmodel)
        {
            // Datacontext kommer fra DIcontainer.cs
            // Det sendes med i parameter fra DIcontainer, da ShelfViewModel er oprettet der samt view
            // Så DIcontainer ved, at når der bliver spurgt om "ShelfViewModel" her, så skal det sendes herover.
            InitializeComponent();
            DataContext = viewmodel;
        }
    }
}
