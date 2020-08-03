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
    class SampleDataSource : MedicineReminder.Common.BindableBase
    {
        private static IEnumerable<Reminder> _reminders = new List<Reminder>();
        private static SampleDataSource _sample = new SampleDataSource();


        #region Properties

        public static bool DisplayByPatients
        {
            get { return true; }
        }


        /// <summary>
        /// Reminders which are past due time
        /// </summary>
        public static ObservableCollection<Reminder> Overdue 
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
        public static ObservableCollection<Reminder> Next
        {
            get
            {
                ObservableCollection<Reminder> next = new ObservableCollection<Reminder>();
             
                lock (_reminders)
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

        public static ObservableCollection<ReminderGroup> ReminderGroups
        {
            get { return SearchReminderGroups(x => true); }
        }

        public static ObservableCollection<ReminderGroup> PatientGroups
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

        public SampleDataSource()
        {
            Reload();
        }

        internal static ObservableCollection<ReminderGroup> SearchReminderGroups(Func<Reminder, bool> predicate)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            var groups = new ObservableCollection<ReminderGroup>();

            if (null == predicate)
                return groups;

            var overdue = new ObservableCollection<Reminder>(Overdue.Where(predicate));
            if (overdue.Count() > 0)
                groups.Add(new ReminderGroup(
                    loader.GetString("Overdue"),
                    loader.GetString("OverdueDescription"),
                    overdue));

            var next = new ObservableCollection<Reminder>(Next.Where(predicate));
            if (next.Count() > 0)
                groups.Add(new ReminderGroup(
                    loader.GetString("Next"),
                    loader.GetString("NextDescription"),
                    next));

            return groups;
        }

        private IEnumerable<Reminder> GetSampleData()
        {
            List<Reminder> reminders = new List<Reminder>();
            reminders.Add(new Reminder(Guid.NewGuid(), "Sulfasalazine", "Naveed", Interval.TwiceADay, DateTime.Now, "Take with a full glass of water"));
            reminders.Add(new Reminder(Guid.NewGuid(), "Vitamin C", "Saima", Interval.TwiceADay, DateTime.Now + new TimeSpan(1, 1, 1), "Take with a full glass of water"));
            reminders.Add(new Reminder(Guid.NewGuid(), "Vitamin E", "Saima", Interval.TwiceADay, DateTime.Now.AddDays(-1), "Take with a full glass of water"));
            reminders.Add(new Reminder(Guid.NewGuid(), "Vitamin B", "Saima", Interval.TwiceADay, DateTime.Now.AddDays(-3), "Take with a full glass of water"));
            reminders.Add(new Reminder(Guid.NewGuid(), "Vitamin D", "Naveed", Interval.OnceADay, DateTime.Now + new TimeSpan(1, 1, 1), "Take with milk"));
            reminders.Add(new Reminder(Guid.NewGuid(), "Vitamin A", "Naveed", Interval.OnceADay, DateTime.Now.AddDays(4), "Take with milk"));
            reminders.Add(new Reminder(Guid.NewGuid(), "Vitamin B", "Naveed", Interval.OnceADay, DateTime.Now.AddDays(1), "Take with milk"));
            reminders.Add(new Reminder(Guid.NewGuid(), "Vitamin DASFSASFDASadf afafdsdfssfsfsdfsdfASDAFDAasdadadasdas asdaad", "Naveed", Interval.OnceADay, DateTime.Now + new TimeSpan(1, 1, 1), "Take with milk"));
            return reminders;
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
        private void Reload()
        {
            List<Reminder> reminders = new List<Reminder>();
            
            reminders.AddRange(GetSampleData());

            lock (_reminders)
            {
                _reminders = reminders;
            }

            //Refreshing reminders in the view
            Refresh();
        }

    }
}
