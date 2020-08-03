using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineReminder.Storage
{
    /// <summary>
    /// Reresents applicaiton settings 
    /// </summary>
    interface ISettings
    {
        /// <summary>
        /// Display items by Patient Name
        /// </summary>
        bool DisplayByPatients { get; set; }
    }
}
