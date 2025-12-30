using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace StudentPerformanceDashboard
{
    public partial class StudentDashboardPage : Window
    {
        public StudentDashboardPage()
        {
            InitializeComponent();
            DataContext = new StudentPerformanceViewModel();
            Loaded += Page_Loaded;
        }

        private void SfComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }

    // Converters
    public class LabelToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var label = value.ToString() ?? string.Empty;
            var colorMap = new Dictionary<string, Color>
            {
                { "Male", Color.FromArgb(255, 64, 224, 230) },
                { "Female", Color.FromArgb(255, 233, 30, 99) },
                { "Others", Color.FromArgb(255, 38, 50, 56) },
                { "Pass", Color.FromArgb(255, 64, 224, 230) },
                { "Fail", Color.FromArgb(255, 233, 30, 99) },
                { "Attended", Color.FromArgb(255, 38, 50, 56) },
                { "English", Color.FromArgb(255, 64, 224, 230) },
                { "Arts", Color.FromArgb(255, 3, 218, 198) },
                { "Maths", Color.FromArgb(255, 33, 150, 243) },
                { "PhysEd", Color.FromArgb(255, 25, 118, 210) },
                { "Science", Color.FromArgb(255, 13, 71, 161) }
            };

            if (colorMap.TryGetValue(label, out var color))
            {
                return new SolidColorBrush(color);
            }

            return new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class LabelToGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() switch
            {
                "Male" => "♂",
                "Female" => "♀",
                "Others" => "⚧",
                _ => string.Empty
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class OneItemVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count) return count == 1 ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class MultipleItemsVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count) return count == 1 ? Visibility.Collapsed : Visibility.Visible;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class SubjectTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }
        public DataTemplate? SelectedTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Subject subject && subject.IsSelected)
                return SelectedTemplate ?? new DataTemplate();

            return DefaultTemplate ?? new DataTemplate();
        }
    }
}
