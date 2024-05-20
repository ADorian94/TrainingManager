using System;
using System.Globalization;
using TrainingManager.Data;
using Xamarin.Forms;

namespace TrainingManager.View.Controls
{
    /// <summary>
    /// base value converter
    /// </summary>
    /// <typeparam name="T">The type of this value converter</typeparam>
    public abstract class BaseValueConverter<T> : IValueConverter
        where T : class, new()
    {
        #region Value Converter Methods
        /// <summary>
        /// The method to convert one type to another
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// The method to convert a value back to itself
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion
    }

    /// <summary>
    /// Converts double to user friendly number
    /// </summary>
    public class DoubleValueConverter : BaseValueConverter<DoubleValueConverter>
    {
        /// <summary>
        /// Convert anything to double
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="targetType">not supported</param>
        /// <param name="parameter">not supported</param>
        /// <param name="culture">not supported</param>
        /// <returns>double</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                return /*TypeConverter.*/ForceDoubleUniversal(
                    (value.ToString().EndsWith(".") || value.ToString().EndsWith(","))
                    ? value.ToString() + "0"
                    : value.ToString());
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Convert(value, targetType, parameter, culture);

        /// <summary>
        /// Forces conversion from object to double
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static double ForceDoubleUniversal(object o)
        {
            if (o == null)
            {
                return 0;
            }
            var culture = o.ToString().Contains(",") ? CultureInfo.CreateSpecificCulture("de-DE") : CultureInfo.CreateSpecificCulture("en-US");

            return !Double.TryParse(o.ToString(), NumberStyles.Any, culture, out double result) ? 0 : result;
        }
    }

    public class BoolToNegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class MaterialColorToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var color = (MaterialColors)value;

            switch (color)
            {
                case MaterialColors.Red:
                    return Color.FromHex("#f05545");
                case MaterialColors.Purple:
                    return Color.FromHex("#d05ce3");
                case MaterialColors.DeepPurple:
                    return Color.FromHex("#8559da");
                case MaterialColors.Blue:
                    return Color.FromHex("#005CB2");
                case MaterialColors.Cyan:
                    return Color.FromHex("#007C91");
                case MaterialColors.LightGreen:
                    return Color.FromHex("#4B830D");
                case MaterialColors.Lime:
                    return Color.FromHex("#8C9900");
                case MaterialColors.Amber:
                    return Color.FromHex("#c56000");
                case MaterialColors.DeepOrange:
                    return Color.FromHex("#c63f17");
                case MaterialColors.Brown:
                    return Color.FromHex("#5d4037");
                case MaterialColors.Default:
                default:
                    return Color.FromHex("#666666");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException("The converter is not implemeted.");
    }

    public class DateToSTring : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "N/A";

            return ((DateTime)value).ToString("yyyy.mm.dd");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

}
