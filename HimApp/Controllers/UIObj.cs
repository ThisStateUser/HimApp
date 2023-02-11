using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        public static void SwitchThemeVoid()
        {
            bool theme = Properties.Settings.Default.Theme;

            switch (theme)
            {
                case true:
                    Properties.Settings.Default.Theme = false;
                    SwitchThemeCheck();
                    break;
                case false:
                    Properties.Settings.Default.Theme = true;
                    SwitchThemeCheck();
                    break;
                default:
                    break;
            }
            Properties.Settings.Default.Save();
        }

        public static void SwitchThemeCheck()
        {
            if (Properties.Settings.Default.Theme)
            {
                UpdateColor("Styles/Themes/BrightBackground.xaml");
                
            }
            else
                UpdateColor("Styles/Themes/DarkBackground.xaml");
        }

        public static void SwitchColor()
        {
            UpdateColor("Styles/Colors/" + Properties.Settings.Default.Color + ".xaml");
        }

        public static void SweepColor(string colorname)
        {
            Properties.Settings.Default.Color = colorname;
            Properties.Settings.Default.Save();
            SwitchColor();
        }
    }
}
