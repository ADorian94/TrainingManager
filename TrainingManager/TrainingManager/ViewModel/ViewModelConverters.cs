using System;
using System.Globalization;
using Xamarin.Forms;

namespace TrainingManager.ViewModel
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (int)value != 0;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? 1 : 0;
    }

    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var dateTime = (DateTime)value;

            return dateTime.Date.ToString("yyyy.MM.dd");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class DateToShortStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var dateTime = (DateTime)value;

            return dateTime.Date.ToString("ddd");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }


    public class DoubleToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return ((double)value) != 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class IntToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "0:0:0";

            var seconds = (int)value;

            int hour = seconds / 3600;
            seconds = seconds % 3600;

            int minutes = seconds / 60;
            seconds = seconds % 60;

            return $"{hour}:{minutes}:{seconds}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException("The converter is not implemeted.");
    }

    public class DoubleToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return (double)value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0.0;

            string stringValue = value as string;

            if (string.IsNullOrEmpty(stringValue))
                return 0.0;

            double dbl;

            if (double.TryParse(stringValue, out dbl))
            {
                if (dbl == 0)
                    return 0.0;

                return dbl;
            }

            return 0.0;
        }
    }
}
