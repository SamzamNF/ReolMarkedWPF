using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ReolMarkedWPF.Helpers
{

    public class FrameNavigationService : INavigationService
    {
        private readonly Frame _frame;

        // Constructoren tager et Frame som parameter, ellers crasher den
        public FrameNavigationService(Frame frame)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        public void Navigate(object view)
        {
            _frame.Navigate(view);
        }
    }
}
