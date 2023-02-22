using HimApp.BD;
using HimApp.Controllers;
using HimApp.Views.Pages;
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
        string _login = "";
        public DoubleAuth(bool OpenStatus, string PathImg = null, string s_code = null, string login = null)
        {
            InitializeComponent();
            WConnect.DoubleAuthMethod = this;
            if (OpenStatus)
            {
                _login = login;
                CheckCodeQwest.Visibility = Visibility.Visible;
                this.Height = 140;
                CheckCode.Focus();
            }
            else
            {
                SetCodeQwest.Visibility = Visibility.Visible;
                this.Height = 550;
                ShowQR(PathImg);
                _s_code = s_code;
                code.Focus();
            }
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
            this.DialogResult = false;
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
            AuthDouble DAuth = HimBDEntities.GetContext().AuthDouble.Where(x => x.user_id == UserObj.UserAcc.id).FirstOrDefault();
            try
            {
                WebClient client = new WebClient();
                string responce = client.DownloadString($"https://www.authenticatorApi.com/Validate.aspx?Pin={code.Text.Trim()}&SecretCode={_s_code}");
                if (responce == "True")
                {
                    if (DAuth == null)
                        HimBDEntities.GetContext().AuthDouble.Add(new AuthDouble { s_code = _s_code, user_id = UserObj.UserAcc.id });
                    else
                        DAuth.s_code = _s_code;
                    HimBDEntities.GetContext().SaveChanges();
                    MessageBox.Show("Двухфакторная аутентификация установлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
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
        private void SendCheckCode_Click(object sender, RoutedEventArgs e)
        {
            Users user;
            if (Properties.Settings.Default.Login.Length != 0)
                user = HimBDEntities.GetContext().Users.Where(z => z.login == Properties.Settings.Default.Login.Trim()).FirstOrDefault();
            else
                user = HimBDEntities.GetContext().Users.Where(z => z.login == _login.Trim()).FirstOrDefault();
            if (user == null)
                return;
            AuthDouble DAuth = HimBDEntities.GetContext().AuthDouble.Where(x => x.user_id == user.id).FirstOrDefault();
            try
            {
                WebClient client = new WebClient();
                string responce = client.DownloadString($"https://www.authenticatorApi.com/Validate.aspx?Pin={CheckCode.Text.Trim()}&SecretCode={DAuth.s_code}");
                if (responce == "True")
                {
                    if (DAuth == null)
                        return;
                    MessageBox.Show("Проверка пройдена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
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

        private void CheckCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendCheckCode_Click(null, null);
        }

        private void code_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendCode_Click(null, null);
        }
    }
}
