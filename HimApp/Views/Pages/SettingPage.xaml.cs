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
            RdStartPage();
            initDouble();
        }

        public void initDouble()
        {
            if (HimBDEntities.GetContext().AuthDouble.Where(x => x.user_id == UserObj.UserAcc.id).FirstOrDefault() != null)
            {
                SetCode.Visibility = Visibility.Collapsed;
                DelCode.Visibility = Visibility.Visible;
            }
            else
            {
                SetCode.Visibility = Visibility.Visible;
                DelCode.Visibility = Visibility.Collapsed;
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
                case "AdminPage":
                    AdminPageIcon.SetResourceReference(PackIconMaterial.ForegroundProperty, "Basic");
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
            AdminPageIcon.SetResourceReference(PackIconMaterial.ForegroundProperty, "Text");
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
        private void AdminPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.StartUp("AdminPage");
            RdStartPage();
        }
        private string news_code()
        {
            Random random = new Random();
            char[] tokenMas = new char[]
            {
                'A','b','c','D','e','F',
                'f','J','j','w','W','g',
                '0','1','2','C','V','G',
                '9','5','3','7','N','q',
                'Q','L','s','S','Z','z',
                'o','O','a','P','p','n'
            };

            string s_code = "";

            for (int i = 0; i <= 32; i++)
            {
                int ti = random.Next(0, tokenMas.Length - 1);
                s_code += tokenMas[ti];
            }
            return s_code;
        }

        private async void AuthCodeSet_Click(object sender, RoutedEventArgs e)
        {
            string code = news_code();
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hpm = await hc
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
            catch (HttpRequestException)
            {
                return;
            }
        }

        private void DelCode_Click(object sender, RoutedEventArgs e)
        {
            AuthDouble DAuth = HimBDEntities.GetContext().AuthDouble.Where(x => x.user_id == UserObj.UserAcc.id).FirstOrDefault();
            if (DAuth == null)
                MessageBox.Show("Отключено");
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите отключить двухфакторную аутентификацию?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (new DoubleAuth(true).ShowDialog() == true)
                    {
                        HimBDEntities.GetContext().AuthDouble.Remove(DAuth);
                    }
                }
            }
            HimBDEntities.GetContext().SaveChanges();
            initDouble();
        }
    }
}
