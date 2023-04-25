using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace PropertyManagement
{
    public class EmptyImageUrlToDefaultImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string imageUrl = value as string;
            if (string.IsNullOrEmpty(imageUrl))
            {
                // Use the default image from the Assets folder
                return "ms-appx:///Assets/maintenance_icon.png";
            }
            else
            {
                return imageUrl;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
