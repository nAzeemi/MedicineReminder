using MedicineReminder.Common;
using MedicineReminder.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Search Contract item template is documented at http://go.microsoft.com/fwlink/?LinkId=234240

namespace MedicineReminder.View
{
    /// <summary>
    /// This page displays search results when a global search is directed to this application.
    /// </summary>
    public sealed partial class SearchResults : MedicineReminder.Common.LayoutAwarePage
    {
        private ViewModel.RemindersViewModel _vm;

        public SearchResults()
        {
            this.InitializeComponent();

            _vm = ViewModel.RemindersViewModel.Create(TheApp.Instance.Storage, TheApp.Instance.NavigationService, TheApp.Instance.Settings);
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
            var queryText = navigationParameter as String;

            // TODO: Application-specific searching logic.  The search process is responsible for
            //       creating a list of user-selectable result categories:
            //
            //       filterList.Add(new Filter("<filter name>", <result count>));
            //
            //       Only the first filter, typically "All", should pass true as a third argument in
            //       order to start in an active state.  Results for the active filter are provided
            //       in Filter_SelectionChanged below.

            //showing search pane on keyboard input
            SearchPane.GetForCurrentView().ShowOnKeyboardInput = true;
            
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            var filterList = new List<SearchResultFilter>();
            filterList.Add(new SearchResultFilter(loader.GetString("All"), _vm.SearchReminderGroups(GetFilterPredicate(loader.GetString("All"), queryText)), true));
            filterList.Add(new SearchResultFilter(loader.GetString("Medicine"), _vm.SearchReminderGroups(GetFilterPredicate(loader.GetString("Medicine"), queryText)), false));
            filterList.Add(new SearchResultFilter(loader.GetString("Patient"), _vm.SearchReminderGroups(GetFilterPredicate(loader.GetString("Patient"), queryText)), false));

            // Communicate results through the view model
            this.DefaultViewModel["QueryText"] = '\u201c' + queryText + '\u201d';
            this.DefaultViewModel["Query"] = queryText;
            this.DefaultViewModel["Filters"] = filterList;
            this.DefaultViewModel["ShowFilters"] = filterList.Count > 1;
        }

        Func<Reminder, bool> GetFilterPredicate(String filter, String search)
        {
            if (null == search)
                return null;

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            Func<Reminder, bool> predicate;

            if (filter == loader.GetString("Medicine"))
            {
                predicate = new Func<Reminder,bool>(reminder => reminder.Medicine.Contains(search));
            }
            else if (filter == loader.GetString("Patient"))
            {
                predicate = new Func<Reminder, bool>(reminder => reminder.Patient.Contains(search));
            }
            else
            {
                predicate = new Func<Reminder, bool>(reminder => reminder.Patient.Contains(search) || reminder.Medicine.Contains(search));
            }

            return predicate;
        }

        /// <summary>
        /// Invoked when a filter is selected using the ComboBox in snapped view state.
        /// </summary>
        /// <param name="sender">The ComboBox instance.</param>
        /// <param name="e">Event data describing how the selected filter was changed.</param>
        void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Determine what filter was selected
            var selectedFilter = e.AddedItems.FirstOrDefault() as SearchResultFilter;
            if (selectedFilter != null)
            {
                // Mirror the results into the corresponding Filter object to allow the
                // RadioButton representation used when not snapped to reflect the change
                selectedFilter.Active = true;

                this.DefaultViewModel["Results"] = selectedFilter.Results;

                // Ensure results are found
                object results;
                ICollection resultsCollection;
                if (this.DefaultViewModel.TryGetValue("Results", out results) &&
                    (resultsCollection = results as ICollection) != null &&
                    resultsCollection.Count != 0)
                {
                    VisualStateManager.GoToState(this, "ResultsFound", true);
                    return;
                }
            }

            // Display informational text when there are no search results.
            VisualStateManager.GoToState(this, "NoResultsFound", true);
        }

        /// <summary>
        /// Invoked when a filter is selected using a RadioButton when not snapped.
        /// </summary>
        /// <param name="sender">The selected RadioButton instance.</param>
        /// <param name="e">Event data describing how the RadioButton was selected.</param>
        void Filter_Checked(object sender, RoutedEventArgs e)
        {
            // Mirror the change into the CollectionViewSource used by the corresponding ComboBox
            // to ensure that the change is reflected when snapped
            if (filtersViewSource.View != null)
            {
                var filter = (sender as FrameworkElement).DataContext;
                filtersViewSource.View.MoveCurrentTo(filter);
            }
        }

        private void ResultsView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            Reminder reminder = (Reminder)e.ClickedItem;
            TheApp.Instance.SelectedReminder = reminder;

            TheApp.Instance.NavigationService.Navigate(typeof(TakeMedicine), reminder);
        }


        /// <summary>
        /// View model describing one of the filters available for viewing search results.
        /// </summary>
        private sealed class SearchResultFilter : MedicineReminder.Common.BindableBase
        {
            private String _name;
            private bool _active;
            private ObservableCollection<ReminderGroup> _results;

            public SearchResultFilter(String name, ObservableCollection<ReminderGroup> results, bool active = false)
            {
                this.Name = name;
                this.Results = results;
                this.Active = active;
            }

            public override String ToString()
            {
                return Description;
            }

            public String Name
            {
                get { return _name; }
                set { if (this.SetProperty(ref _name, value)) this.OnPropertyChanged("Description"); }
            }

            public int Count
            {
                get 
                {
                    int count = 0;
                    foreach (var group in _results)
                        count += group.Reminders.Count;

                    return count;
                }
            }

            public ObservableCollection<ReminderGroup> Results 
            {
                get { return _results; }
                set 
                {
                    if (this.SetProperty(ref _results, value))
                    {
                        this.OnPropertyChanged("Results");
                        this.OnPropertyChanged("Description");
                    }
                }
            }

            public bool Active
            {
                get { return _active; }
                set { this.SetProperty(ref _active, value); }
            }

            public String Description
            {
                get { return String.Format("{0} ({1})", _name, this.Count); }
            }
        }

    }
}
