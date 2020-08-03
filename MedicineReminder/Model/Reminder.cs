using System;

namespace MedicineReminder.Model
{
    /// <summary>
    /// Reminder interval
    /// </summary>
    enum Interval
    {
        EveryHour = 1,
        Every2Hours = 2,
        Every3Hours = 3,
        Every4Hours = 4,
        Every6Hours = 6,
        Every8Hours = 8,
        TwiceADay = 12,
        OnceADay = 24,
    }

    /// <summary>
    /// This class represents a medicine reminder
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    [System.Runtime.Serialization.DataContract]
    internal class Reminder : MedicineReminder.Common.BindableBase
    {
        public Reminder()
        {
            this._id = Guid.NewGuid();
        }

        public Reminder(
            Guid id,
            String medicine,
            String patient,
            Interval interval,
            DateTime next,
            String note)
        {
            this._id = id;
            this._medicine = medicine;
            this._patient = patient;
            this._interval = interval;
            this._next = next;
            this._note = note;
        }


        private Guid _id;
        [System.Runtime.Serialization.DataMember]
        public Guid Id
        {
            get { return this._id; }
            set { this.SetProperty(ref this._id, value); }
        }

        private String _medicine = string.Empty;
        [System.Runtime.Serialization.DataMember]
        public String Medicine
        {
            get { return this._medicine; }
            set { this.SetProperty(ref this._medicine, value); }
        }

        private String _patient = string.Empty;
        [System.Runtime.Serialization.DataMember]
        public String Patient
        {
            get { return this._patient; }
            set { this.SetProperty(ref this._patient, value); }
        }

        private Interval _interval = Interval.OnceADay;
        [System.Runtime.Serialization.DataMember]
        public Interval Interval
        {
            get { return this._interval; }
            set { this.SetProperty(ref this._interval, value); }
        }

        private DateTime _next = DateTime.Now;
        [System.Runtime.Serialization.DataMember]
        public DateTime Next
        {
            get { return this._next; }
            set { this.SetProperty(ref this._next, value); }
        }

        private String _note = string.Empty;
        [System.Runtime.Serialization.DataMember]
        public String Note
        {
            get { return this._note; }
            set { this.SetProperty(ref this._note, value); }
        }

        /// <summary>
        /// Returns true if a given reminder is overdue
        /// </summary>
        public bool IsOverdue()
        {
            return _next <= DateTime.Now;
        }


        /// <summary>
        /// Advances the reminder to when the medicine is due next in future
        /// </summary>
        /// <returns>self</returns>
        public Reminder AdvanceNext()
        {
            return AdvanceNext(this._next);
        }

        /// <summary>
        /// Advances the reminder to when the medicine is due next in future
        /// shifting the schedule based on when the medicine was last taken
        /// </summary>
        /// <param name="given"></param>
        /// <returns>self</returns>
        public Reminder AdvanceNext(DateTime given)
        {
            //Calculate the next due time
            DateTime now = DateTime.Now;
            TimeSpan span = new TimeSpan((int)_interval, 0, 0);

            DateTime advanced = given;
            while (advanced <= now)
                advanced += span;

            this.Next = advanced;
            return this;
        }

    }
}
