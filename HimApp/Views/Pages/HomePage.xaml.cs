using HimApp.BD;
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
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            WConnect.MainWindowMethod.PageTitle.Text = "Главная";
            PreLoad();
        }

        private void PreLoad()
        {
            DateTime overdate = DateTime.Today.AddDays(Properties.Settings.Default.AddDays);
            List<Order> orders = HimBDEntities.GetContext().Order.Where(x => x.arrival_date >= DateTime.Today && x.status_id == 3 && x.arrival_date <= overdate).OrderBy(x => x.arrival_date).ToList();
            if (orders.Count == 0)
            {
                emptyborder.Visibility = Visibility.Visible;
                FOrder.Visibility = Visibility.Collapsed;
            
            } else
            {
                emptyborder.Visibility = Visibility.Collapsed;
                FOrder.Visibility = Visibility.Visible;
                orderFirst.ItemsSource = orders;
            }
        }

        private void FOrder_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                FOrder.ScrollToHorizontalOffset(FOrder.HorizontalOffset + (e.Delta - 10) * -1);
            else if (e.Delta < 0)
                FOrder.ScrollToHorizontalOffset(FOrder.HorizontalOffset + (e.Delta + 10) * -1);
            e.Handled = true;
        }
    }
}
