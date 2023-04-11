using HimApp.BD;
using HimApp.Controllers;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
            WConnect.AuthWindowMethod = this;
            UIObj.SwitchThemeCheck();
            UIObj.SwitchColor();
            Task check = new Task(CheckApp);
            check.Start();
        }

        private bool RememberAuth()
        {
            if (Properties.Settings.Default.IsRemember)
            {
                LoaderShow("Вход в аккаунт");
                Users user = HimBDEntities.GetContext().Users.Where
                                (x => x.login == Properties.Settings.Default.Login.Trim()).FirstOrDefault();

                void auth()
                {
                    UserObj.UserAcc = user;
                    new MainWindow().Show();
                    this.Close();
                }

                if (user != null)
                {
                    if (user.UserInfo.role_id != 1)
                    {
                        auth();
                        return true;
                    }
                    if (HimBDEntities.GetContext().AuthDouble.Where(x => x.user_id == user.id).FirstOrDefault() == null)
                    { 
                        auth(); 
                        return true;
                    }
                    if (new DoubleAuth(true).ShowDialog() == true)
                    {
                        auth(); 
                        return true; 
                    }
                    else
                    {
                        Properties.Settings.Default.IsRemember = false;
                        Properties.Settings.Default.Login = "";
                        Properties.Settings.Default.Save();
                        return false;
                    }
                }
                else
                {
                    Loading(false);
                    Dispatcher.Invoke(() => { ErrorShow("Ошибка входа"); });
                    return false;
                }
            }
            return false;
        }

        private void ReturnFormAuth()
        {
            Dispatcher.Invoke(() =>
            {
                if (RememberAuth())
                    return;
                Loading(false);
                ReturnForm();
                login.Focus();
            });
        }

        public void Loading(bool condition)
        {
            Dispatcher.Invoke(() =>
            {
                if (condition == false)
                {
                    RotateLoad.BeginAnimation(RotateTransform.AngleProperty, null);
                    return;
                }
                DoubleAnimation animation = new DoubleAnimation()
                {
                    From = 0,
                    To = 360,
                    Duration = TimeSpan.FromSeconds(1),
                    RepeatBehavior = RepeatBehavior.Forever,
                };
                RotateLoad.BeginAnimation(RotateTransform.AngleProperty, animation);
            });
        }


        private void CheckApp()
        {
            if (Properties.Settings.Default.Color == null)
                UIObj.SweepColor("TealColor");
            LoaderShow("Соединение с базой данных");
            try
            {
                HimBDEntities.GetContext().Roles.ToList();
            }
            catch (EntityException)
            {
                Dispatcher.Invoke(() =>
                {
                    ErrorShow("Соединение отсутствует");
                });
                return;
            }
            ReturnFormAuth();
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
                this.DragMove();
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
            }
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            if (!UserAuth())
                return;
            LoaderShow("Вход в аккаунт");
            Users user = UserObj.FindUser(login.Text, password_visible.Text);
            if (user.UserInfo.role_id == 1
                && HimBDEntities.GetContext().AuthDouble.Where(x => x.user_id == user.id).FirstOrDefault() != null)
            {
                if (new DoubleAuth(true, login:login.Text).ShowDialog() == true)
                {
                    UserObj.UserAcc = user;
                    new MainWindow().Show();
                    this.Close();
                }
                ReturnForm();
                return;
            }
            UserObj.UserAcc = user;
            new MainWindow().Show();
            this.Close();
            
        }

        private void login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if(TbOrPb_pass)
                    password_visible.Focus();
                else
                    password.Focus();
            }
        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Auth_Click(null, null);
        }

        private void password_visible_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Auth_Click(null, null);
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

            Users user = UserObj.FindUser(login.Text, password_visible.Text);

            if (user == null)
            {
                MainVoid.ErrorMessage("Неверный логин или пароль");
                return false;
            }

            if (Remember.IsChecked.Value)
            {
                Properties.Settings.Default.IsRemember = true;
                Properties.Settings.Default.Login = user.login;
                Properties.Settings.Default.Save();
            }

            return true;
        }

        private void ErrorShow(string errormsg)
        {
            ReturnLoader();
            Loading(false);
            Dispatcher.Invoke(() =>
            {
                Loader.Visibility = Visibility.Collapsed;
                Error.Visibility = Visibility.Visible;
                msgload.Text = errormsg;
            });
        }

        private void LoaderShow(string msg)
        {
            ReturnLoader();
            Loading(true);
            Dispatcher.Invoke(() =>
            {
                Loader.Visibility = Visibility.Visible;
                Error.Visibility = Visibility.Collapsed;
                msgload.Text = msg;
            });
        }

        private void ReturnLoader()
        {
            Dispatcher.Invoke(() =>
            {
                LoadPage.Visibility = Visibility.Visible;
                AuthForm.Visibility = Visibility.Collapsed;
            });
        }

        private void ReturnForm()
        {
            Dispatcher.Invoke(() =>
            {
                LoadPage.Visibility = Visibility.Collapsed;
                AuthForm.Visibility = Visibility.Visible;
            });
        }
    }
}
