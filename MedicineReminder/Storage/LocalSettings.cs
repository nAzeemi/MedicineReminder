using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MedicineReminder.Storage
{
    class LocalSettings : ISettings
    {
        ApplicationDataContainer _settings;

        public LocalSettings()
        {
            _settings = ApplicationData.Current.LocalSettings;
        }

        public bool DisplayByPatients
        {
            get
            {
                object value = _settings.Values["DisplayByPatients"];
                if (null == value)
                    return false;
                else
                    return (bool)value;
            }
            set
            {
                _settings.Values["DisplayByPatients"] = value;
            }
        }
    }
}
