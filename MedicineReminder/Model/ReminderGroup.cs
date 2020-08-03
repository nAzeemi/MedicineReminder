using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineReminder.Model
{
    /// <summary>
    /// Represents a certain group of reminders
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    class ReminderGroup : MedicineReminder.Common.BindableBase
    {
        public String Title { get; private set; }
        public String Description { get; private set; }
        public ObservableCollection<Reminder> Reminders { get; private set; }
        public Int32 Total { get { return Reminders.Count(); } }
        public Int32 Overdue { get { return Reminders.Where(r => r.IsOverdue()).Count(); } }

        public ReminderGroup(String title, String description, ObservableCollection<Reminder> reminders)
        {
            this.Title = title;
            this.Description = description;
            this.Reminders = reminders;
        }
    }
}
