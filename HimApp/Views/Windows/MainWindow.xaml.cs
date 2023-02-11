using HimApp.Controllers;
using HimApp.Views.Pages;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HimApp.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool MinMaxWin = false;

        public MainWindow()
        {
            InitializeComponent();
            UpdIconTheme();
            //UserName.Text = UserObj.UserAcc.UserInfo.last_name + " " + UserObj.UserAcc.UserInfo.first_name;
            FrameM.Navigate(new HomePage());
        }

        //ToolBar
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

        private void ResizeWin_Click(object sender, RoutedEventArgs e)
        {
            switch (MinMaxWin)
            {
                case false:
                    WindowState = WindowState.Maximized;
                    ResizeIcon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.Square;
                    MinMaxWin = true;
                    break;

                case true:
                    WindowState = WindowState.Normal;
                    ResizeIcon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.SquareOutline;
                    MinMaxWin = false;
                    break;
                default:
                    break;
            }
        }



        private void ThemeSwitch_Click(object sender, RoutedEventArgs e)
        {
            UIObj.SwitchThemeVoid();
            UpdIconTheme();
            if (Properties.Settings.Default.Theme)
                if (Properties.Settings.Default.Color == "WhiteColor")
                    UIObj.SweepColor("DarkColor");
                else return;
            else if (Properties.Settings.Default.Color == "DarkColor")
                UIObj.SweepColor("WhiteColor");
            else return;
        }
        private void UpdIconTheme()
        {
            if (Properties.Settings.Default.Theme == true)
                ThemeSwitchIcon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.MoonWaningCrescent;
            else
                ThemeSwitchIcon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.Lightbulb;
        }

        private void GoSettingPage_Click(object sender, RoutedEventArgs e)
        {
            FrameM.Navigate(new SettingPage());
        }

        private void GoClientPage_Click(object sender, RoutedEventArgs e)
        {
            FrameM.Navigate(new ClientPage());
        }
    }
}
