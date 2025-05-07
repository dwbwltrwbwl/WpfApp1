using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing;
using System.Drawing;
using System.Drawing.Imaging;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageQRCode.xaml
    /// </summary>
    public partial class PageQRCode : Page
    {
        public PageQRCode()
        {
            InitializeComponent();
            LoadQR();
        }
        private void LoadQR()
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 300
                }
            };
            var result = writer.Write(@"https://learn.microsoft.com/ru-ru/dotnet/api/system.timespan?view=net-8.0");
            var bitmap = new BitmapImage();

            using (var memoryStream = new MemoryStream())
            {
                result.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Position = 0;
                bitmap.BeginInit();
                bitmap.StreamSource = memoryStream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
            }
            imgQr.Source = bitmap;
        }
    }
}
