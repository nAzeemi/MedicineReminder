using MedicineReminder.Common;
using MedicineReminder.Model;
using MedicineReminder.Notifications;
using MedicineReminder.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MedicineReminder.ViewModel
{
    class ReminderViewModel : MedicineReminder.Common.BindableBase
    {

        private IStorage _storage;
        private INavigationService _navigationService;

        #region Properties

        private Reminder _reminder;
        public Reminder Reminder
        {
            get { return this._reminder; }
        }

        private string _medicine = string.Empty;
        public string Medicine
        {
            get { return this._reminder.Medicine; }
            set { this.SetProperty(ref this._medicine, value); }
        }

        private string _patient = string.Empty;
        public string Patient
        {
            get { return this._reminder.Patient; }
            set { this.SetProperty(ref this._patient, value); }
        }

        private Interval _interval = Interval.OnceADay;
        public Interval Interval
        {
            get { return this._reminder.Interval; }
            set { this.SetProperty(ref this._interval, value); }
        }

        private DateTime _next = DateTime.Now;
        public DateTime Next
        {
            get { return this._reminder.Next; }
            set { this.SetProperty(ref this._next, value); }
        }

        private String _note = string.Empty;
        public String Note
        {
            get { return this._reminder.Note; }
            set { this.SetProperty(ref this._note, value); }
        }


        private bool _onSchedule = true;
        public bool OnSchedule
        {
            get { return this._onSchedule; }
            set { this.SetProperty(ref this._onSchedule, value); }
        }

        private bool _offSchedule = false;
        public bool OffSchedule
        {
            get { return this._offSchedule; }
            set { this.SetProperty(ref this._offSchedule, value); }
        }

        private DateTime _takeAt = DateTime.Now;
        public DateTime TakeAt
        {
            get { return this._takeAt; }
            set { this.SetProperty(ref this._takeAt, value); }
        }

        private bool _shiftSchedule = true;
        public bool ShiftSchedule
        {
            get { return this._shiftSchedule; }
            set { this.SetProperty(ref this._shiftSchedule, value); }
        }

        #endregion //Properties

        #region Commands

        public ICommand TakeMedicineCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand AddCommand { get; private set; }

        #endregion //Commands

        /// <summary>
        /// Default constructor is used only for design time with mock data
        /// </summary>
        public ReminderViewModel()
            : this(new MockStorage().All.First(), new MockStorage(), null)
        {
        }

        internal ReminderViewModel(Reminder reminder)
            : this(reminder, TheApp.Instance.Storage, TheApp.Instance.NavigationService)
        {
        }

        internal ReminderViewModel(Reminder reminder, IStorage storage, INavigationService navigationService)
        {
            if (null != reminder) //existing reminder
                _reminder = reminder;
            else //new reminder
                _reminder = new Reminder();

            _storage = storage;
            _navigationService = navigationService;

            //Updating properties
            _medicine = _reminder.Medicine;
            _patient = _reminder.Patient;
            _interval = _reminder.Interval;
            _next = _reminder.Next;
            _note = _reminder.Note;

            //Initializing commands
            this.TakeMedicineCommand = new RelayCommand(x => this.TakeMedicine());
            this.DeleteCommand = new RelayCommand(x => this.Delete());
            this.UpdateCommand = new RelayCommand(x => this.Update());
            this.AddCommand = new RelayCommand(x => this.Add());
        }

        private void TakeMedicine()
        {
            DateTime baseTime;
            if (_offSchedule && _shiftSchedule)
                baseTime = _takeAt;
            else
                baseTime = _reminder.Next;

            _reminder.AdvanceNext(baseTime);
            _storage.Update(_reminder);

            NotificationManager.SetNotification(_reminder);

            _navigationService.GoBack();
        }

        private void Delete()
        {
            _storage.Remove(_reminder.Id);

            //Remove any shceduled notification
            NotificationManager.RemoveNotification(_reminder.Id);

            _navigationService.GoBack();
        }

        private void Update()
        {
            //Use the private memebers of the ViewModel for update, as the properties will fetch the values from the original reminder
            _reminder.Medicine = _medicine;
            _reminder.Patient = _patient;
            _reminder.Interval = _interval;
            _reminder.Next = AdvanceDay(_next); //Advance the day to when the medicine is due next
            _reminder.Note = _note;

            //Save updated reminder
            _storage.Update(_reminder);

            NotificationManager.SetNotification(_reminder);

            _navigationService.GoBack();
        }

        private void Add()
        {
            //Ensure that Next due time is in future
            _reminder.Next = AdvanceDay(_reminder.Next);

            _storage.Add(_reminder);

            NotificationManager.SetNotification(_reminder);

            _navigationService.GoBack();
        }

        /// <summary>
        /// Advnaces the date part for next due time
        /// Time is kept but the date is advanced for next reminder
        /// </summary>
        private DateTime AdvanceDay(DateTime given)
        {
            DateTime now = DateTime.Now;
            TimeSpan day = new TimeSpan(1, 0, 0, 0);
            DateTime advanced = given;
            while (advanced <= now)
                advanced += day;

            return advanced;
        }

    }
}
