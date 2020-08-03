using MedicineReminder.Model;
using MedicineReminder.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace MedicineReminder
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class TakeMedicine : MedicineReminder.Common.LayoutAwarePage
    {
        public TakeMedicine()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            Reminder reminder = (Reminder)navigationParameter;
            this.DataContext = new ReminderViewModel(reminder);

            //Disabling auto showing search pane on keyboard input
            SearchPane.GetForCurrentView().ShowOnKeyboardInput = false;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Reminder reminder = TheApp.Instance.SelectedReminder;
            Debug.Assert(null != reminder, "Selected reminder cannot be null");
            
            TheApp.Instance.NavigationService.Navigate(typeof(EditReminder), reminder);

            

        }
    }
}
