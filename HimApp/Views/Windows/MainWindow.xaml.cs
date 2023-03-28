using HimApp.Controllers;
using HimApp.Views.Pages;
using HimApp.Views.Pages.FnPages;
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
            WConnect.MainWindowMethod = this;
            UpdIconTheme();
            UserName.Text = ($"{UserObj.UserAcc.UserInfo.last_name} {UserObj.UserAcc.UserInfo.first_name} ({UserObj.UserAcc.UserInfo.Roles.role_name})");
            RdStartPage();
        }

        private void RdStartPage()
        {
            if (UserObj.UserAcc.UserInfo.Roles.id == 2)
            {
                AdminPageBtn.Visibility = Visibility.Collapsed;
                if (Properties.Settings.Default.StartupPage == "AdminPage")
                    Properties.Settings.Default.StartupPage = "HomePage";
            }
            else
                AdminPageBtn.Visibility = Visibility.Visible;

            switch (Properties.Settings.Default.StartupPage)
            {
                case "HomePage":
                    FrameM.Navigate(new HomePage());
                    HomePageBtn.IsChecked = true;
                    break;
                case "OrdersPage":
                    FrameM.Navigate(new OrdersPage());
                    OrdersPageBtn.IsChecked = true;
                    break;
                case "ProductPage":
                    FrameM.Navigate(new ProductPage());
                    ProductPageBtn.IsChecked = true;
                    break;
                case "EmployessPage":
                    FrameM.Navigate(new EmployeesPage());
                    EmployessPageBtn.IsChecked = true;
                    break;
                case "ServicePage":
                    FrameM.Navigate(new ServicePage());
                    ServicePageBtn.IsChecked = true;
                    break;
                case "AdminPage":
                    FrameM.Navigate(new AdminPage());
                    AdminPageBtn.IsChecked = true;
                    break;
            }
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
                this.DragMove();
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

            if (FrameM.Content.ToString().Contains("SettingPage"))
            {
                SettingPage curPage = FrameM.Content as SettingPage;
                curPage.RdStartPage();
            };
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
        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            FrameM.Navigate(new AddOrder());
        }

        private void HomePage_Click(object sender, RoutedEventArgs e)
        {
            FrameM.Navigate(new HomePage());
        }
        private void OrdersPage_Click(object sender, RoutedEventArgs e)
        {
            FrameM.Navigate(new OrdersPage());
        }
        private void ProductPage_Click(object sender, RoutedEventArgs e)
        {
            FrameM.Navigate(new ProductPage());
        }
        private void EmployeesPage_Click(object sender, RoutedEventArgs e)
        {
            FrameM.Navigate(new EmployeesPage());
        }
        private void ServicePage_Click(object sender, RoutedEventArgs e)
        {
            FrameM.Navigate(new ServicePage());
        }
        private void AdminPage_Click(object sender, RoutedEventArgs e)
        {
            FrameM.Navigate(new AdminPage());
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Login = "";
            Properties.Settings.Default.IsRemember = false;
            Properties.Settings.Default.Save();
            UserObj.UserAcc = null;
            new AuthWindow().Show();
            this.Close();
        }
    }
}
