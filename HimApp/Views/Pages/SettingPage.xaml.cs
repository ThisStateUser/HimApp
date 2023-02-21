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
            RdStartPage();
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

        private async void AuthCodeSet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage hpm = await hc
                    .GetAsync("https://www.authenticatorApi.com/pair.aspx?AppName=HimApp&AppInfo=" + UserObj.UserAcc + "&SecretCode=230b793597bf40365f18cb98bf9aa8e3");

                if (hpm.IsSuccessStatusCode)
                {
                    MatchCollection matches = new Regex(@"src='.*?'").Matches(await hpm.Content.ReadAsStringAsync());
                    string ShortVal = matches[0].Value.Remove(0, 5);
                    ShortVal = ShortVal.Remove(ShortVal.Length - 1, 1);
                    new DoubleAuth(ShortVal).Show();
                }
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ");
                return;
            }

            //HimAppCours3
            //Uri http = new Uri();
            //http.
            //https://www.authenticatorApi.com/pair.aspx?AppName=HimApp&AppInfo=UserName&SecretCode=230b793597bf40365f18cb98bf9aa8e3
        }
    }
}
