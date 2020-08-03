using MedicineReminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineReminder.Storage
{
    class MockStorage : IStorage
    {
        IEnumerable<Reminder> _reminders;

        public MockStorage()
        {
            _reminders = GetMockData();
        }

        private IEnumerable<Reminder> GetMockData()
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

        public IEnumerable<Model.Reminder> All
        {
            get { return _reminders; }
        }

        public Model.Reminder Get(Guid id)
        {
            return _reminders.Single((reminder) => id == reminder.Id);
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Add(Model.Reminder reminder)
        {
            throw new NotImplementedException();
        }

        public void Update(Model.Reminder reminder)
        {
            throw new NotImplementedException();
        }
    }
}
