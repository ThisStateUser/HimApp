using HimApp.BD;
using HimApp.Controllers;
using HimApp.Views.Windows;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;

namespace HimApp.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
            WConnect.SettingPageMethod = this;
            WConnect.MainWindowMethod.PageTitle.Text = "Настройки";
            RdStartPage();
            initDouble();
            PreLoad();
        }

        public void initDouble()
        {
            if (HimBDEntities.GetContext().AuthDouble.Where(x => x.user_id == UserObj.UserAcc.id).FirstOrDefault() != null)
            {
                SetFactorCode.Visibility = Visibility.Collapsed;
                DelFactorCode.Visibility = Visibility.Visible;
            }
            else
            {
                SetFactorCode.Visibility = Visibility.Visible;
                DelFactorCode.Visibility = Visibility.Collapsed;
            }
        }

        private void PreLoad()
        {
            DayOfFast.Tag = Properties.Settings.Default.AddDays.ToString();
            if (Properties.Settings.Default.IsRemember)
                RememberUser.IsChecked = true;
            else
                RememberUser.IsChecked = false;

            if (UserObj.UserAcc.UserInfo.role_id == 2)
            {
                DayOfFast.IsEnabled = false;
                SetFactorCode.IsEnabled = false;
            }
            else
            {
                DayOfFast.IsEnabled = true;
                SetFactorCode.IsEnabled = true;
            }
        }

        public void RdStartPage()
        {
            RestColor();
            switch (Properties.Settings.Default.StartupPage)
            {
                case "HomePage":
                    HomePageIcon.SetResourceReference(PackIconMaterial.ForegroundProperty, "Basic");
                    break;
                case "OrdersPage":
                    OrdersPageIcon.SetResourceReference(PackIconMaterial.ForegroundProperty, "Basic");
                    break;
                case "ProductPage":
                    ProductPageIcon.SetResourceReference(PackIconMaterial.ForegroundProperty, "Basic");
                    break;
                case "EmployessPage":
                    EmployeesPageIcon.SetResourceReference(PackIconMaterial.ForegroundProperty, "Basic");
                    break;
                case "ServicePage":
                    ServicePageIcon.SetResourceReference(PackIconMaterial.ForegroundProperty, "Basic");
                    break;
            }
        }

        private void RestColor()
        {
            HomePageIcon.SetResourceReference(PackIconMaterial.ForegroundProperty, "Text");
            OrdersPageIcon.SetResourceReference(PackIconMaterial.ForegroundProperty, "Text");
            ProductPageIcon.SetResourceReference(PackIconMaterial.ForegroundProperty, "Text");
            EmployeesPageIcon.SetResourceReference(PackIconMaterial.ForegroundProperty, "Text");
            ServicePageIcon.SetResourceReference(PackIconMaterial.ForegroundProperty, "Text");
        }

        private void BlueColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("BlueColor");
            RdStartPage();
        }
        private void IndigoColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("IndigoColor");
            RdStartPage();
        }
        private void OrangeColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("OrangeColor");
            RdStartPage();
        }
        private void PinkColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("PinkColor");
            RdStartPage();
        }
        private void RedColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("RedColor");
            RdStartPage();
        }        
        private void TealColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("TealColor");
            RdStartPage();
        }
        private void YellowColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("YellowColor");
            RdStartPage();
        }
        private void WhiteDarkColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Settings.Default.Theme)
                UIObj.SweepColor("DarkColor");
            else
                UIObj.SweepColor("WhiteColor");
            RdStartPage();
        }

        private void HomePage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.StartUp("HomePage");
            RdStartPage();
        }
        private void OrdersPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.StartUp("OrdersPage");
            RdStartPage();
        }
        private void ProductPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.StartUp("ProductPage");
            RdStartPage();
        }
        private void EmployeesPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.StartUp("EmployessPage");
            RdStartPage();
        }
        private void ServicePage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.StartUp("ServicePage");
            RdStartPage();
        }

        private async void SetFactorCode_Click(object sender, RoutedEventArgs e)
        {
            string code = MainVoid.GenCode();
            try
            {
                HttpResponseMessage hpm = await new HttpClient()
                    .GetAsync($"https://www.authenticatorApi.com/pair.aspx?AppName=HimApp&AppInfo={UserObj.UserAcc.login}&SecretCode={code}");
                if (hpm.IsSuccessStatusCode)
                {
                    MatchCollection ImgPath = new Regex(@"src='.*?'").Matches(await hpm.Content.ReadAsStringAsync());
                    string ShortVal = ImgPath[0].Value.Remove(0, 5);
                    ShortVal = ShortVal.Remove(ShortVal.Length - 1, 1);
                    if (new DoubleAuth(false, ShortVal, code).ShowDialog() == true)
                        initDouble();
                }
            }
            catch (HttpRequestException ex)
            {
                MainVoid.FatalErrorMessage(ex.ToString());
                return;
            }
        }

        private void DelFactorCode_Click(object sender, RoutedEventArgs e)
        {
            AuthDouble DAuth = HimBDEntities.GetContext().AuthDouble.Where(x => x.user_id == UserObj.UserAcc.id).FirstOrDefault();
            if (DAuth == null)
                MessageBox.Show("Отключено");
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите отключить двухфакторную аутентификацию?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (new DoubleAuth(true, login:UserObj.UserAcc.login).ShowDialog() == true)
                    {
                        HimBDEntities.GetContext().AuthDouble.Remove(DAuth);
                        HimBDEntities.GetContext().SaveChanges();
                        initDouble();
                    }
                }
            }
        }

        private void OnlyNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            MainVoid.OnlyNumber((TextBox)sender, e);
        }

        private void DayOfFast_TextChanged(object sender, TextChangedEventArgs e)
        {
            int day = 0;
            int.TryParse(DayOfFast.Text.Trim(), out day);
            Properties.Settings.Default.AddDays = day;
            Properties.Settings.Default.Save();
        }

        private void RememberUser_Click(object sender, RoutedEventArgs e)
        {
            if (RememberUser.IsChecked == false)
            {
                Properties.Settings.Default.Login = "";
                Properties.Settings.Default.IsRemember = false;
            }
            else
            {
                Properties.Settings.Default.IsRemember = true;
                Properties.Settings.Default.Login = UserObj.UserAcc.login;
            }
            Properties.Settings.Default.Save();
        }
    }
}
