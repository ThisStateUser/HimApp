using HimApp.Controllers;
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
        }

        private void BlueColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("BlueColor");
        }
        private void IndigoColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("IndigoColor");
        }
        private void OrangeColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("OrangeColor");
        }
        private void PinkColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("PinkColor");
        }
        private void RedColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("RedColor");
        }        
        private void TealColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("TealColor");
        }
        private void YellowColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIObj.SweepColor("YellowColor");
        }
        private void WhiteDarkColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Settings.Default.Theme)
                UIObj.SweepColor("DarkColor");
            else
                UIObj.SweepColor("WhiteColor");
        }
    }
}
