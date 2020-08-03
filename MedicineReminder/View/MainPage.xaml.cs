using MedicineReminder.Common;
using MedicineReminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace MedicineReminder
{

    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class MainPage : MedicineReminder.Common.LayoutAwarePage
    {
        private ViewModel.RemindersViewModel _vm;

        public MainPage()
        {
            this.InitializeComponent();

            _vm = ViewModel.RemindersViewModel.Create(TheApp.Instance.Storage, TheApp.Instance.NavigationService, TheApp.Instance.Settings);
            this.DataContext = _vm;
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
            _vm.Reload();

            if (_vm.ReminderGroups.Count() < 1)
            {
                bottomAppBar.IsOpen = true;
                bottomAppBar.IsSticky = true;
            }

            //Enabling auto showing search pane on keyboard input
            SearchPane.GetForCurrentView().ShowOnKeyboardInput = true;

            //Linking the semantic zoomed out views
            this.duetimeZoomedOutGridView.ItemsSource = this.duetimeZoomedOutListView.ItemsSource = this.RemindersViewSource.View.CollectionGroups;
            this.patientsZoomeOutdGridView.ItemsSource = this.patientsZoomedOutListView.ItemsSource = this.PatientsViewSource.View.CollectionGroups;

            //Disabling the auto-selection of the first item in the Grid Views
            this.duetimeZoomedInGridView.SelectedIndex = -1;
            this.patientsZoomedInGridView.SelectedIndex = -1;
        }

        /// <summary>
        /// Invoked when a group header is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a group header for the selected group.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var group = (sender as FrameworkElement).DataContext;
        }

        /// <summary>
        /// Invoked when an item within a group is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            Reminder reminder = (Reminder)e.ClickedItem;
            TheApp.Instance.SelectedReminder = reminder;

            TheApp.Instance.NavigationService.Navigate(typeof(TakeMedicine), reminder);
        }

        private void itemGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reminder reminder = GetSelectedReminder();
            if (reminder != null)
            {
                TheApp.Instance.SelectedReminder = reminder;
                DeleteButton.Visibility = EditButton.Visibility = Visibility.Visible;
                bottomAppBar.IsSticky = true;
                bottomAppBar.IsOpen = true;
            }
            else
            {
                TheApp.Instance.SelectedReminder = null;
                DeleteButton.Visibility = EditButton.Visibility = Visibility.Collapsed;
                bottomAppBar.IsSticky = false;
                bottomAppBar.IsOpen = false;
            }

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Reminder reminder = GetSelectedReminder();
            if (null == reminder)
                return;

            this.Frame.Navigate(typeof(EditReminder), reminder);
        }

        private void DisplayByButtonClicked(object sender, RoutedEventArgs e)
        {
            if ((bool)PatientsView.IsChecked)
                TheApp.Instance.Settings.DisplayByPatients = true;
            else
                TheApp.Instance.Settings.DisplayByPatients = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _vm.Reload();

            //Register for share
            DataTransferManager.GetForCurrentView().DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnSharedDataRequested);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            //Unregister for share();
            DataTransferManager.GetForCurrentView().DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnSharedDataRequested);
        }


        private void OnSharedDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            Reminder reminder = TheApp.Instance.SelectedReminder;
            if (null == reminder)
            {
                e.Request.FailWithDisplayText(loader.GetString("SelectAReminderToShare"));
                return;
            }

            e.Request.Data.Properties.Title = String.Format(loader.GetString("TakeMedicineAt"), reminder.Medicine, reminder.Next.ToString());
            e.Request.Data.Properties.Description = String.Format(loader.GetString("SharingReminderForPatient"), reminder.Patient);
            e.Request.Data.SetText(String.Format(loader.GetString("SharingReminderText"), reminder.Medicine, reminder.Patient, reminder.Next.ToString(), reminder.Note));

            //Setting some extra properties
            //e.Request.Data.Properties.Add(new KeyValuePair<string, object>("Patient", reminder.Patient));
        }

        /// <summary>
        /// Returns the selected reminder from the active view
        /// </summary>
        /// <returns>The selected reminder</returns>
        private Reminder GetSelectedReminder()
        {
            Reminder reminder;

            if (TheApp.Instance.Settings.DisplayByPatients)
                if (Windows.UI.Xaml.Visibility.Visible == fullView.Visibility)
                    reminder = (Reminder)patientsZoomedInGridView.SelectedItem;
                else //snappedView
                    reminder = (Reminder)patientsZoomedInListView.SelectedItem;
            else
                if (Windows.UI.Xaml.Visibility.Visible == fullView.Visibility)
                    reminder = (Reminder)duetimeZoomedInGridView.SelectedItem;
                else //snappedView
                    reminder = (Reminder)duetimeZoomedInListView.SelectedItem;

            return reminder;
        }

    }
}
