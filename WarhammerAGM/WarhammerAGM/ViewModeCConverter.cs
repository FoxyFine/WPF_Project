using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WarhammerAGM
{
    public class ViewModeCConverter : IValueConverter
    {
        private bool not;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mode = (ApplicationViewModel.ViewModeC)value;
            var view = mode == ApplicationViewModel.ViewModeC.ViewC;
            view ^= not; //view будет true если только одна из переменных true, остальное даст false
            if (targetType == typeof(Visibility))
                return view ? Visibility.Visible : Visibility.Collapsed; //Collapsed - элемент не виден и не участвует в компоновке.
            return view;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private ViewModeCConverter() { }

        public static ViewModeCConverter Instance { get; } = new ViewModeCConverter();
        public static ViewModeCConverter NotInstance { get; } = new ViewModeCConverter() { not = true };
    }

    public class ViewModeCExtension : MarkupExtension
    {
        public bool Not { get; set; }
        public ViewModeCExtension() { }
        public ViewModeCExtension(bool not) => Not = not;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Not ? ViewModeCConverter.NotInstance : ViewModeCConverter.Instance;
        }
    }
}
