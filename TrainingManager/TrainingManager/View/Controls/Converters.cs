using System;
using System.Globalization;
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


}
