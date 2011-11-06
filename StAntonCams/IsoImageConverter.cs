/// Converter to allow binding to image stored in Isolated Storage.
/// 
/// Thanks to Ernest Poletaev @csharpblog
/// (http://www.csharpblog.co.cc/2011/10/binding-to-image-path-in-isolated.html)
///
///

namespace StAntonCams
{
    using System.Windows.Data;
    using System.Windows.Media.Imaging;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System;

    public class IsoImageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var bitmap = new BitmapImage();
            try
            {
                var path = (string)value;
                if (!string.IsNullOrEmpty(path))
                {
                    using (var file = LoadFile(path))
                    {
                        bitmap.SetSource(file);
                    }
                }
            }
            catch
            {
            }
            return bitmap;
        }

        private Stream LoadFile(string file)
        {
            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return isoStore.OpenFile(file, FileMode.Open, FileAccess.Read);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}