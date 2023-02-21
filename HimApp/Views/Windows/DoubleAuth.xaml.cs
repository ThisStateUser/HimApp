using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace HimApp.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для DoubleAuth.xaml
    /// </summary>
    public partial class DoubleAuth : Window
    {
        public DoubleAuth(string PathImg)
        {
            InitializeComponent();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = new Uri(PathImg);
            bitmapImage.EndInit();
            imgAuth.Source = bitmapImage;
        }
    }
}
