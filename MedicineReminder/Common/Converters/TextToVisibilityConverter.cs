using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MedicineReminder.Common
{
    class TextToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is string && !string.IsNullOrEmpty((string)value) && !string.IsNullOrWhiteSpace((string)value)) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
