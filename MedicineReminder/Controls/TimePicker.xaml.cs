using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MedicineReminder
{
    public sealed partial class TimePicker : UserControl
    {
        private enum AMPMIndex
        {
            am = 0,
            pm,
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TimePicker()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Dependency property giving the value of the picked time
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(DateTime), typeof(TimePicker), new PropertyMetadata(DateTime.Now, new PropertyChangedCallback(OnValueChanged)));
        public DateTime Value
        {
            get { return (DateTime)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, (DateTime)value); }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTime time = (DateTime)e.NewValue;

            int hr;
            if (time.Hour == 0)
                hr = 12;
            else if (time.Hour > 12)
                hr = time.Hour - 12;
            else
                hr = time.Hour;

            TimePicker timePicker = (TimePicker)d;

            timePicker.Hours.SelectedIndex = hr - 1;
            timePicker.Minutes.SelectedIndex = time.Minute;
            timePicker.AMPM.SelectedIndex = (time.Hour >= 12) ? (int)AMPMIndex.pm : (int)AMPMIndex.am;
        }


        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime current = this.Value;

            if (AMPM.SelectedIndex < 0)
                return;

            AMPMIndex ampm = (AMPMIndex)AMPM.SelectedIndex;
            int hr = Hours.SelectedIndex + 1;

            //Converting hour to 24hr format
            if (AMPMIndex.am == ampm && 12 == hr) //12am means 00
                hr = 0;
            else if (AMPMIndex.pm == ampm && hr < 12) //1pm to 11pm
                hr += 12;


            DateTime time = new DateTime(current.Year, current.Month, current.Day, hr, Minutes.SelectedIndex, current.Second);

            if (time == this.Value)
                return;

            SetValue(ValueProperty, time);
        }

    }

}
