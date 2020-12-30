using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TreeVisualization
{
    [ValueConversion(sourceType: typeof(bool), targetType: typeof(Visibility), ParameterType = typeof(bool))]
    public class Bool2VisibilityHiddenConverter : BaseConverter<Bool2VisibilityHiddenConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool v = false;
            if (parameter != null)
                bool.TryParse(parameter.ToString(), out v);

            // if parameter is true, swap the visibility.
#pragma warning disable IDE0046 // Convertir en expression conditionnelle
            if (v)
#pragma warning restore IDE0046 // Convertir en expression conditionnelle
                return (bool)value ? Visibility.Hidden : Visibility.Visible;
            else
                return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool v = false;
            if (parameter != null)
                bool.TryParse(parameter.ToString(), out v);

            switch ((Visibility)value)
            {
                case Visibility.Visible:
                    return true ^ v;
                case Visibility.Hidden:
                case Visibility.Collapsed:
                    return false ^ v;
                default:
                    return null;
            }
        }
    }
}
