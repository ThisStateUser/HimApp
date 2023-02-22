using HimApp.BD;
using HimApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
        string _s_code = "";
        public DoubleAuth(string PathImg, string s_code)
        {
            InitializeComponent();
            ShowQR(PathImg);
            _s_code = s_code;
            code.Focus();
        }

        private void ShowQR(string PathImg)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = new Uri(PathImg);
            bitmapImage.EndInit();
            imgAuth.Source = bitmapImage;
        }

        private void CloseWin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CollapseWin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void SendCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebClient client = new WebClient();
                string responce = client.DownloadString($"https://www.authenticatorApi.com/Validate.aspx?Pin={code.Text.Trim()}&SecretCode={_s_code}");
                if (responce == "True")
                {
                    MessageBox.Show("Двухфакторная аутентификация установлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (HimBDEntities.GetContext().AuthDouble.Where(x => x.user_id == UserObj.UserAcc.id).FirstOrDefault() == null)
                        HimBDEntities.GetContext().AuthDouble.Add(new AuthDouble { s_code = _s_code, user_id = UserObj.UserAcc.id });
                    else
                        HimBDEntities.GetContext().AuthDouble.Where(x => x.user_id == UserObj.UserAcc.id).FirstOrDefault().s_code = _s_code;
                    HimBDEntities.GetContext().SaveChanges();
                }
                else if (responce == "False")
                    MessageBox.Show("Неверный код", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                else return;
            }
            catch (Exception)
            {
                MessageBox.Show("Нет интернет соединения");
                return;
            }
        }
    }
}
