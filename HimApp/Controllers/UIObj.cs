using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HimApp.Controllers
{
    internal class UIObj
    {
        public static void UpdateColor(String path)
        {
            var uri = new Uri(@path, UriKind.Relative);
            ResourceDictionary Dictionary = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(Dictionary);
        }

        public static void SwithThemeVoid()
        {
            bool theme = Properties.Settings.Default.Theme;

            switch (theme)
            {
                case true:
                    Properties.Settings.Default.Theme = false;
                    SwithThemeCheck();
                    break;
                case false:
                    Properties.Settings.Default.Theme = true;
                    SwithThemeCheck();
                    break;
                default:
                    break;
            }
            Properties.Settings.Default.Save();
        }

        public static void SwithThemeCheck()
        {
            if (Properties.Settings.Default.Theme == false)
                UpdateColor("Styles/Themes/DarkBackground.xaml");
            else
                UpdateColor("Styles/Themes/BrightBackground.xaml");

        }
    }
}
