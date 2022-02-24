using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TrainingManager.Data;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

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

            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday: return "Mon";
                case DayOfWeek.Tuesday: return "Tue";
                case DayOfWeek.Wednesday: return "Wed";
                case DayOfWeek.Thursday: return "Thu";
                case DayOfWeek.Friday: return "Fri";
                case DayOfWeek.Saturday: return "Sat";
                case DayOfWeek.Sunday: return "Sun";
                default: return string.Empty;
            }
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

    public class BoolNegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value != null ? !(bool)value : false;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException("The converter is not implemeted.");
    }

    public class ColorToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var colors = (IEnumerable<MaterialColors>)value;

            return colors.Select(x =>
            {
                switch (x)
                {
                    case MaterialColors.Default:
                        return "Default";
                    case MaterialColors.Red:
                        return "Red";
                    case MaterialColors.Purple:
                        return "Purple";
                    case MaterialColors.DeepPurple:
                        return "Deep Purple";
                    case MaterialColors.Blue:
                        return "Blue";
                    case MaterialColors.Cyan:
                        return "Cyan";
                    case MaterialColors.LightGreen:
                        return "Light Green";
                    case MaterialColors.Lime:
                        return "Lime";
                    case MaterialColors.Amber:
                        return "Amber";
                    case MaterialColors.DeepOrange:
                        return "Deep Orange";
                    case MaterialColors.Brown:
                        return "Brown";
                    default:
                        throw new NotImplementedException();
                }
            });
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException("The converter is not implemeted.");
    }
}
