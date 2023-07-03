using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace HotelManagement.ViewModel.AdminVM.TroubleManagementVM
{
    public class EditConverter : IValueConverter
    {
        // This converts the result object to the foreground.
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo language)
        {
            // Retrieve the format string and use it to format the value.
            string text = value as string;

            if ( text == STATUS.CANCLE)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StatusBrushConverter : IValueConverter
    {
        // This converts the result object to the foreground.
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo language)
        {
            // Retrieve the format string and use it to format the value.
            string text = value as string;

            if (text == STATUS.WAITING || text == "Waiting" || text == LEVEL.CRITICAL)
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#DF0404");
            else if (text == STATUS.DONE || text == "Solved" || text == LEVEL.NORMAL)
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#00B087");
            else if (text == STATUS.IN_PROGRESS || text == "Solving" || text == STATUS.PREDIT)
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#2233C5");
            else
                return new SolidColorBrush(Colors.Gray);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StatusBGBrushConverter : IValueConverter
    {
        // This converts the result object to the foreground.
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo language)
        {
            // Retrieve the format string and use it to format the value.
            string text = value as string;

            if (text == STATUS.WAITING || text == "Waiting" || text == LEVEL.CRITICAL)
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#FFC5C5");
            else if (text == STATUS.DONE || text == "Solved" || text == LEVEL.NORMAL)
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#B0EEE3");
            else if (text == STATUS.IN_PROGRESS || text == "Solving" || text == STATUS.PREDIT)
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#C0DAF1");
            else
                return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
