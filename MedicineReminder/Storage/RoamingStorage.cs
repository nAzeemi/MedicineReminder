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
    class RoamingStorage : ContainerStorage
    {
        public RoamingStorage()
            : base(ApplicationData.Current.RoamingSettings.CreateContainer("Reminders", ApplicationDataCreateDisposition.Always))
        {
        }

    }
}
