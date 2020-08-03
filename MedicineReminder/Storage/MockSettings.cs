using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineReminder.Storage
{
    class MockSettings : ISettings
    {
        public bool DisplayByPatients
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
