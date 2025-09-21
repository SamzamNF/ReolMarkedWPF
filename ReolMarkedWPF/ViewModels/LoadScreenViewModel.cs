using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReolMarkedWPF.ViewModel
{
    internal class LoadScreenViewModel
    {
        // Constructor
        public LoadScreenViewModel()
            {
        }
        public ICommand OpenUpCommand { get; set; }

        //Launcher til MainVindow

        public void OpenUp()
        {
            // Open MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }



        }
}
