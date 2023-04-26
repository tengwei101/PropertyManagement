using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace PropertyManagement
{
    public class StatusToBorderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string status = value as string;
            
            switch (status)
            {
                case "In Progress":
                    return new SolidColorBrush(Color.FromArgb(255, 244, 205, 185)); // #7FBAD0

                case "Pending":
                    return new SolidColorBrush(Color.FromArgb(255, 127, 186, 208)); // #C0EEAB

                case "Completed":
                    return new SolidColorBrush(Color.FromArgb(255, 192, 238, 171)); // #F4CDB9

                default:
                    return new SolidColorBrush(Windows.UI.Colors.Teal);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
