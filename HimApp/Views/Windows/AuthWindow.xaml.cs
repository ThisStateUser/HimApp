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
using HimApp.Controllers;
using MahApps.Metro.IconPacks;

namespace HimApp.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        bool TbOrPb_pass = false;

        public AuthWindow()
        {
            InitializeComponent();
            login.Focus();
            UIObj.SwithThemeCheck();
        }

        private void CloseWin_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CollapseWin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void ShowPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            shb_help(TbOrPb_pass, password, password_visible, SHicon);
        }

        private void shb_help(bool param, PasswordBox pass, TextBox text, PackIconMaterial icon)
        {
            switch (param)
            {
                case false:
                    text.Text = pass.Password;
                    text.Visibility = Visibility.Visible;
                    pass.Visibility = Visibility.Collapsed;
                    icon.Kind = PackIconMaterialKind.EyeOff;
                    TbOrPb_pass = true;
                    break;

                case true:
                    pass.Password = text.Text;
                    text.Visibility = Visibility.Collapsed;
                    pass.Visibility = Visibility.Visible;
                    icon.Kind = PackIconMaterialKind.Eye;
                    TbOrPb_pass = false;
                    break;
                default:
                    break;
            }
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            if(login.Text != "1234")
            {
                login.BorderThickness = new Thickness(1);
                login.BorderBrush = (SolidColorBrush)FindResource("redcolor");
                return;
            }
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                switch (TbOrPb_pass)
                {
                    case true:
                        password_visible.Focus();
                        break;
                    case false:
                        password.Focus();
                        break;
                    default:
                        break;
                }
            }
        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            Auth_Click(null, null);
        }

        private void password_visible_KeyDown(object sender, KeyEventArgs e)
        {
            Auth_Click(null, null);
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    UIObj.UpdateColor("Styles/Themes/DarkBackground.xaml");
        //}
    }
}
