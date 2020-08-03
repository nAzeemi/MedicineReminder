using MedicineReminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineReminder.Storage
{
    interface IStorage
    {
        /// <summary>
        /// Returns all teh reminders
        /// </summary>
        IEnumerable<Reminder> All { get; }

        /// <summary>
        /// Returns a particular reminder
        /// </summary>
        /// <param name="id">Identifier of the reminder to get</param>
        /// <returns>Reminder. Returns null if not found</returns>
        Reminder Get(Guid id);

        /// <summary>
        /// Removes a particular reminder from the storage if found.
        /// </summary>
        /// <param name="id">Identifier of the reminder to delete</param>
        void Remove(Guid id);

        /// <summary>
        /// Adds a new reminder in the storage
        /// </summary>
        /// <param name="reminder">Reminder to insert in the storage</param>
        void Add(Reminder reminder);

        /// <summary>
        /// Updates a reminder
        /// </summary>
        /// <param name="reminder">The updated reminder</param>
        void Update(Reminder reminder);
    }
}
