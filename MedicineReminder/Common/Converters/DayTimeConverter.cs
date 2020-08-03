using System;
using Windows.Globalization.DateTimeFormatting;
using Windows.UI.Xaml.Data;

namespace MedicineReminder.Common
{
    public sealed class DayTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime)
            {
                DateTime given = (DateTime)value;

                // Converting day
                String converted = String.Empty;
                if (given.Date == DateTime.Now.Date)
                    converted = String.Empty;
                else if (given.Date == DateTime.Now.AddDays(1).Date)
                    converted = "Tomorrow";
                else if (given.Date == DateTime.Now.AddDays(-1).Date)
                    converted = "Yesterday";
                else
                    converted = DateTimeFormatter.ShortDate.Format(given);

                // Adding space after day
                if (!String.IsNullOrEmpty(converted))
                    converted += " ";

                // Converting time
                converted += DateTimeFormatter.ShortTime.Format(given);

                return converted;
            }
            else
                return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
