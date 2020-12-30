using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TreeVisualization
{
    [ValueConversion(sourceType: typeof(bool), targetType: typeof(Brush), ParameterType = typeof(bool))]
    public class Bool2BlackRedConverter : BaseConverter<Bool2BlackRedConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool v = false;
            if (parameter != null)
                bool.TryParse(parameter.ToString(), out v);

            Color color;
            if (!(v ^ (bool)value)) // Need both 'v' and 'value' true or both false
                color = Color.FromRgb(0, 0, 0);
            else
                color = Color.FromRgb(255, 0, 0);

            return new SolidColorBrush(color);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
