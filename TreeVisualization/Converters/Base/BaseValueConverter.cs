using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace TreeVisualization
{
    /// <summary>
    /// A base value converter that allows direct XAML usage.
    /// </summary>
    /// <typeparam name="T">The type of this value converter.</typeparam>
    public abstract class BaseConverter<T> : MarkupExtension, IValueConverter where T : class, new()
    {
        #region Private
        private static T _converter = null;
        #endregion Private

        #region Markup Extension Methods
        /// <summary>
        /// Provides a static instance of the value converter.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // If null, instanciate a new objet and return it.
            return _converter ??= new T();
        }
        #endregion Markup Extension Methods

        #region Converter Methods
        /// <summary>
        /// The method to convert one type to another.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// The method to convert a value back to it's source type.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
        #endregion Converter Methods
    }
}
