using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
