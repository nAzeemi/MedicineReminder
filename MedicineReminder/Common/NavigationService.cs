using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MedicineReminder.Common
{
    class NavigationService : INavigationService
    {
        private Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void GoBack()
        {
            if (_frame != null && _frame.CanGoBack) 
                _frame.GoBack();
        }

        public void GoHome()
        {
            // Use the navigation frame to return to the topmost page
            if (_frame != null)
            {
                while (_frame.CanGoBack)
                    _frame.GoBack();
            }
        }

        public void Navigate(Type page)
        {
            if (_frame != null) 
                _frame.Navigate(page);
        }

        public void Navigate(Type page, object parameter)
        {
            if (_frame != null)
                _frame.Navigate(page, parameter);
        }

    }
}
