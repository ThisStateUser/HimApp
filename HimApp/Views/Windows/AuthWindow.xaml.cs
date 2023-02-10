﻿using HimApp.BD;
using HimApp.Controllers;
using MahApps.Metro.IconPacks;
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
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        bool TbOrPb_pass = false;

        public AuthWindow()
        {
            InitializeComponent();
            UIObj.SwithThemeCheck();
            if (Properties.Settings.Default.IsRemember)
            {
                Users user = HimBDEntities.GetContext().Users.Where
                                (x => x.login == Properties.Settings.Default.Login.Trim()).FirstOrDefault();
                if (user != null)
                {
                    UserObj.UserAcc = user;
                    new MainWindow().Show();
                    this.Close();
                }
            }
            login.Focus();
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
            if (!UserAuth())
                return;
            UserObj.UserAcc = FindUser();
            new MainWindow().Show();
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
            if (e.Key == Key.Enter)
            {
                Auth_Click(null, null);
            }
        }

        private void password_visible_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Auth_Click(null, null);
            }
        }

        private bool UserAuth()
        {
            switch (TbOrPb_pass)
            {
                case true:
                    password.Password = password_visible.Text;
                    break;
                default:
                    password_visible.Text = password.Password;
                    break;
            }

            Users user = FindUser();

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }

            if (Remember.IsChecked.Value)
            {
                Properties.Settings.Default.IsRemember = true;
                Properties.Settings.Default.Login = user.login;
            }

            return true;
        }

        private Users FindUser()
        {
            Users user = HimBDEntities.GetContext().Users.Where
                            (x =>
                                x.login == login.Text.Trim() &&
                                x.password == password_visible.Text.Trim()
                            ).FirstOrDefault();
            return user;
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    UIObj.UpdateColor("Styles/Themes/DarkBackground.xaml");
        //}
    }
}
