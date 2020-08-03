using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MedicineReminder.Storage
{
    /// <summary>
    /// This class provides a toast id derived from the reminder guid id
    /// </summary>
    class ToastId
    {
        private static ApplicationDataContainer _ids = ApplicationData.Current.LocalSettings.CreateContainer("ToastId", ApplicationDataCreateDisposition.Always);

        private const String NEXT = "NEXT";

        /// <summary>
        /// Returns next id
        /// </summary>
        /// <returns></returns>
        private static UInt32 GetNext()
        {
            UInt32 next;
            Object value = _ids.Values[NEXT];
            if (null == value) //If this is the first time
                next = 1;
            else
                next = (UInt32)value;

            _ids.Values[NEXT] = ++next; //TODO: Take care of int overflow

            return next;
        }

        /// <summary>
        /// Gets a toast id derived from the guid
        /// </summary>
        public static String GetId(Guid guid)
        {
            String key = guid.ToString();
            String value;

            if (_ids.Values.ContainsKey(key))
                value = _ids.Values[key].ToString();
            else
            {
                var next = GetNext();
                _ids.Values[key] = next;
                value = next.ToString();
            }

            return value;
        }

        /// <summary>
        /// Removes an id derived from the guid
        /// </summary>
        public static void RemoveId(Guid guid)
        {
            _ids.Values.Remove(guid.ToString());
        }
    }
}
