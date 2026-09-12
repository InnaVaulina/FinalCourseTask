using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Configuration;

namespace WpfBLazorHybridClient.Service
{
    public static class PictureLoader
    {
        public static BitmapImage LoadIllustration(string? fileName)
        {
            if (fileName == null || string.IsNullOrEmpty(fileName))
            {
                return new BitmapImage();
            }
            else
            {
                string imageSavePath = ConfigurationManager.AppSettings["ImageSavePath"];
                string savePath = @$"{imageSavePath}{fileName}";
                FileInfo fileInfo = new FileInfo(savePath);

                using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = fileStream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                    return bitmapImage;
                }
            }

        }
    }
}
