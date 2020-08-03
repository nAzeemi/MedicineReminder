using MedicineReminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MedicineReminder.Storage
{
    class ContainerStorage : IStorage
    {
        ApplicationDataContainer _reminders;
        DataContractJsonSerializer _serializer;


        public ContainerStorage(ApplicationDataContainer reminders)
        {
            _reminders = reminders;

            //Initialize the serializer
            _serializer = new DataContractJsonSerializer(typeof(Reminder));
        }

        public IEnumerable<Reminder> All
        {
            get 
            {
                List<Reminder> reminders = new List<Reminder>();
                var values = _reminders.Values.Values;
                foreach (var value in values)
                {
                    reminders.Add(Deserialize(value));
                }
                return reminders;
            }
        }

        public Reminder Get(Guid id)
        {
            return Deserialize(_reminders.Values[id.ToString()]);
        }

        public void Remove(Guid id)
        {
            _reminders.Values.Remove(id.ToString());
        }

        public void Add(Reminder reminder)
        {
            _reminders.Values.Add(reminder.Id.ToString(), Serialize(reminder));
        }

        public void Update(Reminder reminder)
        {
            _reminders.Values[reminder.Id.ToString()] = Serialize(reminder);
        }

        private string Serialize(Reminder reminder)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                _serializer.WriteObject(stream, reminder);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                using (var reader = new System.IO.StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private Reminder Deserialize(Object data)
        {
            if (data is String)
            {
                using (var stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes((String)data)))
                {
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return (Reminder)_serializer.ReadObject(stream);
                }
            }
            else
                throw new ArgumentException();
        }

    }
}
