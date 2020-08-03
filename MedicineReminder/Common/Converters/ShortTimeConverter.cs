using System;
using Windows.Globalization.DateTimeFormatting;
using Windows.UI.Xaml.Data;

namespace MedicineReminder.Common
{
    class ShortTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime)
            {
                DateTime given = (DateTime)value;

                return DateTimeFormatter.ShortTime.Format(given);
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
