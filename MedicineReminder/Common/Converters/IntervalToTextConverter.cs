using MedicineReminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace MedicineReminder.Common
{
    class IntervalToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Interval)
            {
                Interval given = (Interval)value;
                
                //Loading strings from Resources.resw file
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                string text;

                switch (given)
                {
                    case Interval.OnceADay: text = loader.GetString("OnceADay"); break;
                    case Interval.TwiceADay: text = loader.GetString("TwiceADay"); break;
                    case Interval.Every8Hours: text = loader.GetString("Every8Hours"); break;
                    case Interval.Every6Hours: text = loader.GetString("Every6Hours"); break;
                    case Interval.Every4Hours: text = loader.GetString("Every4Hours"); break;
                    case Interval.Every3Hours: text = loader.GetString("Every3Hours"); break;
                    case Interval.Every2Hours: text = loader.GetString("Every2Hours"); break;
                    case Interval.EveryHour: text = loader.GetString("EveryHour"); break;
                    default: throw new ArgumentException();
                }

                return text;
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
