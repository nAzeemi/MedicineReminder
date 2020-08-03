using MedicineReminder.Model;
using MedicineReminder.Notifications;
using MedicineReminder.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace MedicineReminder.Tasks
{
    class BackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var storage = new LocalStorage();
            var reminders = storage.All;

            UpdateLiveTiles(reminders);
            UpdateToastNotifications(reminders); //Modifies next due time for overdue reminders. This should be the last call with reminders.
        }

        /// <summary>
        /// Ensures that toast notifcations are set for future reminders
        /// Advances next due time for overdue reminders
        /// </summary>
        /// <param name="reminders"></param>
        private static void UpdateToastNotifications(IEnumerable<Reminder> reminders)
        {
            foreach (var reminder in reminders)
            {
                //Advancing next due time for overdue reminders
                //so that toast can be set at original interval even if medicine was skipped
                NotificationManager.SetNotification(reminder.IsOverdue() ? reminder.AdvanceNext() : reminder);
            }
        }

        private void UpdateLiveTiles(IEnumerable<Reminder> reminders)
        {
            //TODO
        }

    }
}
