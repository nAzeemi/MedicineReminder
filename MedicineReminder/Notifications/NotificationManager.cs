using MedicineReminder.Model;
using MedicineReminder.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Notifications;

namespace MedicineReminder.Notifications
{
    class NotificationManager
    {

        public static void SetNotification(Reminder reminder)
        {
            var toast = new ReminderToast(reminder.Medicine, reminder.Patient, reminder.Id.ToString());

            //Setting the toast reminder when medicine is due next
            //Setting a snooze time of 5 minutes
            var recurringToast = new Windows.UI.Notifications.ScheduledToastNotification(toast.GetContent(), reminder.Next, new TimeSpan(0,5,0), 1);

            recurringToast.Id = ToastId.GetId(reminder.Id);

            var notifier = Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier();

            //Remove previously scheduled toasts for this reminder
            RemoveToastFromScedule(recurringToast.Id, notifier);

            notifier.AddToSchedule(recurringToast);

            //TODO: May be add multiple toast notifications for the reminder
        }

        /// <summary>
        /// Removes notifications for the given reminder
        /// </summary>
        /// <param name="reminderId">Id of the reminder whose notifications have to be removed</param>
        public static void RemoveNotification(Guid reminderId)
        {
            RemoveToastFromScedule(ToastId.GetId(reminderId), Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier());
            ToastId.RemoveId(reminderId);
        }

        /// <summary>
        /// Removes previously scheduled toasts matching the id
        /// </summary>
        private static void RemoveToastFromScedule(string toastId, ToastNotifier notifier)
        {
            var oldNotifications = from notification in notifier.GetScheduledToastNotifications()
                                   where notification.Id.Equals(toastId, StringComparison.OrdinalIgnoreCase)
                                   select notification;

            foreach (var old in oldNotifications)
                notifier.RemoveFromSchedule(old);
        }

    }
}
