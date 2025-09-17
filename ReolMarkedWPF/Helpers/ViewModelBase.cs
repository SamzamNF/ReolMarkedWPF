using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
