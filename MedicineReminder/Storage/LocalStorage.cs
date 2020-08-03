using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MedicineReminder.Storage
{
    class LocalStorage: ContainerStorage
    {
        public LocalStorage()
            : base(ApplicationData.Current.LocalSettings.CreateContainer("Reminders", ApplicationDataCreateDisposition.Always))
        {
        }
    }
}
