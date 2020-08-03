using MedicineReminder.Model;
using System;
using Windows.UI.Xaml.Data;

namespace MedicineReminder.Common
{
    class IntervalToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Interval)
            {
                Interval given = (Interval)value;

                int index = -1;
                switch (given)
                {
                    case Interval.OnceADay: index = 0; break;
                    case Interval.TwiceADay: index = 1; break;
                    case Interval.Every8Hours: index = 2; break;
                    case Interval.Every6Hours: index = 3; break;
                    case Interval.Every4Hours: index = 4; break;
                    case Interval.Every3Hours: index = 5; break;
                    case Interval.Every2Hours: index = 6; break;
                    case Interval.EveryHour: index = 7; break;
                    default: throw new ArgumentException();
                }
                
                return index;
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is int)
            {
                int given = (int)value;

                Interval interval;
                switch (given)
                {
                    case 0: interval = Interval.OnceADay; break;
                    case 1: interval = Interval.TwiceADay; break;
                    case 2: interval = Interval.Every8Hours; break;
                    case 3: interval = Interval.Every6Hours; break;
                    case 4: interval = Interval.Every4Hours; break;
                    case 5: interval = Interval.Every3Hours; break;
                    case 6: interval = Interval.Every2Hours; break;
                    case 7: interval = Interval.EveryHour; break;
                    default: throw new ArgumentException();
                }

                return interval;

            }
            else
                return value;

        }
    }
}
