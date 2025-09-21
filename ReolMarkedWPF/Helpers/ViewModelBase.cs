using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReolMarkedWPF.Helpers
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        // Interface fra PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        //Metode til at aktivere det event fra interfaced
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
