using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineReminder.Common
{
    interface INavigationService
    {
        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="pageType">The page type to navigate to</param>
        void Navigate(Type page);

        /// <summary>
        /// Navigate to the specified page passing the given parameter
        /// </summary>
        /// <param name="page">The page type to navigate to</param>
        /// <param name="parameter">The parameter to pass to the page</param>
        void Navigate(Type page, Object parameter);

        /// <summary>
        /// Navigates to the previous page
        /// </summary>
        void GoBack();

        /// <summary>
        /// Navigates to the home page
        /// </summary>
        void GoHome();

    }
}
