using MedicineReminder.Common;
using MedicineReminder.Model;
using MedicineReminder.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MedicineReminder.ViewModel
{
    /// <summary>
    /// Reminders View Model
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    class RemindersViewModel : MedicineReminder.Common.BindableBase
    {
        private ISettings _settings;
        private IStorage _storage;
        private INavigationService _navigationService;
        private IEnumerable<Reminder> _reminders = new List<Reminder>();

        private static Object _instanceLock = new Object();
        private static RemindersViewModel _instance;

        #region Properties

        public bool DisplayByPatients
        {
            get { return _settings.DisplayByPatients; }
        }

        /// <summary>
        /// Reminders which are past due time
        /// </summary>
        public ObservableCollection<Reminder> Overdue 
        { 
            get 
            {
                ObservableCollection<Reminder> overdue = new ObservableCollection<Reminder>();
                
                lock (_reminders)
                {
                    var reminders = from reminder in _reminders
                                    where reminder.IsOverdue()
                                    select reminder;

                    foreach (var reminder in reminders.OrderBy(r => r.Next))
                        overdue.Add(reminder);

                }
                
                return overdue;
            } 
        }

        /// <summary>
        /// Next Reminders
        /// </summary>
        public ObservableCollection<Reminder> Next
        {
            get
            {
                ObservableCollection<Reminder> next = new ObservableCollection<Reminder>();
             
                lock (this._reminders)
                {
                    var reminders = from reminder in _reminders
                                    where !reminder.IsOverdue()
                                    select reminder;

                    foreach (var reminder in reminders.OrderBy(r => r.Next))
                        next.Add(reminder);
                }

                return next;
            }
        }

        public ObservableCollection<ReminderGroup> ReminderGroups
        {
            get { return SearchReminderGroups(x => true); }
        }

        public ObservableCollection<ReminderGroup> PatientGroups
        {
            get
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

                var groups = new ObservableCollection<ReminderGroup>();

                lock (_reminders)
                {
                    var reminderGroups = from reminder in _reminders 
                                         group reminder by reminder.Patient into g
                                         orderby g.Key
                                         select g;

                    foreach (IGrouping<string, Reminder> group in reminderGroups)
                    {
                        ObservableCollection<Reminder> reminders = new ObservableCollection<Reminder>();

                        foreach (var reminder in group.OrderBy(r => r.Next))
                        {
                            reminders.Add(reminder);
                        }

                        groups.Add(new ReminderGroup(
                                            group.Key,
                                            String.Format(loader.GetString("PatientGroupDescription"), group.Count()),
                                            reminders));
                                                    
                    }
                }

                return groups;
            }
        }

        #endregion //Properties

        #region Commands
        
        public ICommand RefreshCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand GoHomeCommand { get; private set; }

        #endregion //Commands

        public static RemindersViewModel Create(IStorage storage, INavigationService navigationService, ISettings settings)
        {
            lock (_instanceLock)
            {
                if (null == _instance)
                    _instance = new RemindersViewModel(storage, navigationService, settings);

                return _instance;
            }
        }

        // Constructor for Mock data for design time
        public RemindersViewModel()
            : this(new MockStorage(), null, new MockSettings())
        {
        }

        private RemindersViewModel(IStorage storage, INavigationService navigationService, ISettings settings)
        {
            _settings = settings;
            _storage = storage;
            _navigationService = navigationService;

            RefreshCommand = new RelayCommand(x => this.Reload());
            AddCommand = new RelayCommand(x => this.Add());
            DeleteCommand = new RelayCommand(x => this.Delete());
            GoHomeCommand = new RelayCommand(x => _navigationService.GoHome());

            Reload();
        }

        internal ObservableCollection<ReminderGroup> SearchReminderGroups(Func<Reminder, bool> predicate)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            var groups = new ObservableCollection<ReminderGroup>();

            if (null == predicate)
                return groups;

            var overdue = new ObservableCollection<Reminder>(this.Overdue.Where(predicate));
            if (overdue.Count() > 0)
                groups.Add(new ReminderGroup(
                    loader.GetString("Overdue"),
                    loader.GetString("OverdueDescription"),
                    overdue));

            var next = new ObservableCollection<Reminder>(this.Next.Where(predicate));
            if (next.Count() > 0)
                groups.Add(new ReminderGroup(
                    loader.GetString("Next"),
                    loader.GetString("NextDescription"),
                    next));

            return groups;
        }

        /// <summary>
        /// Refreshes reminders
        /// </summary>
        private void Refresh()
        {
            //Notifying view to refresh reminders
            this.OnPropertyChanged("Now");
            this.OnPropertyChanged("Overdue");
            this.OnPropertyChanged("Next");
            this.OnPropertyChanged("DisplayRemindersBy");
            this.OnPropertyChanged("ReminderGroups");
            this.OnPropertyChanged("PatientGroups");
        }

        /// <summary>
        /// Reloads reminders
        /// </summary>
        public void Reload()
        {
            List<Reminder> reminders = new List<Reminder>();
            
            reminders.AddRange(_storage.All);
            //reminders.AddRange(GetSampleData());

            lock (this._reminders)
            {
                _reminders = reminders;
            }

            //Refreshing reminders in the view
            Refresh();
        }

        /// <summary>
        /// Adds a new reminder
        /// </summary>
        private void Add()
        {
            //Clear the selected reminder
            TheApp.Instance.SelectedReminder = null;

            //Navigate to the Add page
            _navigationService.Navigate(typeof(AddReminder));
        }

        private void Delete()
        {
            //GetId the selected reminder
            Reminder reminder = TheApp.Instance.SelectedReminder;
            if (null == reminder)
                return;

            _storage.Remove(reminder.Id);

            Reload();
        }

    }
}
