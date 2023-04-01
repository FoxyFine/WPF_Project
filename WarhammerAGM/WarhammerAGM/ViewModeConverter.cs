using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WarhammerAGM
{
    public class ViewModeConverter : IValueConverter
    {
        private bool not;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mode = (ViewMode)value;
            var view = mode == ViewMode.View;
            view ^= not;
            if (targetType == typeof(Visibility))
                return view ? Visibility.Visible : Visibility.Collapsed;
            return view;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private ViewModeConverter() { }

        public static ViewModeConverter Instance { get; } = new ViewModeConverter();
        public static ViewModeConverter NotInstance { get; } = new ViewModeConverter() { not = true };
    }

    public class ViewModeExtension : MarkupExtension
    {
        public bool Not { get; set; }
        public ViewModeExtension() { }
        public ViewModeExtension(bool not) => Not = not;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Not ? ViewModeConverter.NotInstance : ViewModeConverter.Instance;
        }
    }
}
