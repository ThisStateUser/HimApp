using HimApp.Controllers;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
    }
}
