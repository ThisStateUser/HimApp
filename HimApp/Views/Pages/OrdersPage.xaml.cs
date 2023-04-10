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
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {

        
        public OrdersPage()
        {
            InitializeComponent();
            WConnect.MainWindowMethod.PageTitle.Text = "Заказы";
            PreLoad();
        }
        
        private void PreLoad()
        {
            if (HimBDEntities.GetContext().Order.ToList().Count == 0)
            {
                emptyborder.Visibility = Visibility.Visible;
                DG_orders.Visibility = Visibility.Collapsed;
            }
            else
            {
                updDGOrder();
                emptyborder.Visibility = Visibility.Collapsed;
                DG_orders.Visibility = Visibility.Visible;
            }
        }

        private void updDGOrder(string search = "")
        {
            if(search == "")
            {
                DG_orders.ItemsSource = HimBDEntities.GetContext().Order.ToList();
                return;
            }
            DG_orders.ItemsSource = HimBDEntities.GetContext().Order.Where(x => x.ClientCar.Cars.car_brand    .Contains(search.Trim().ToLower()) ||
                                                                                x.ClientCar.Cars.car_model    .Contains(search.Trim().ToLower()) ||
                                                                                x.ClientCar.car_number        .Contains(search.Trim().ToLower()) ||
                                                                                x.ClientCar.Client.first_name .Contains(search.Trim().ToLower()) ||
                                                                                x.ClientCar.Client.last_name  .Contains(search.Trim().ToLower()) ||
                                                                                x.ClientCar.Client.phone      .Contains(search.Trim().ToLower())
                                                                          ).ToList();
        }

        private void SearchOrder_TextChanged(object sender, TextChangedEventArgs e)
        {
            updDGOrder(((TextBox)sender).Text.Trim().ToLower());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(((Button)sender).Tag.ToString());
            Client client = HimBDEntities.GetContext().Order.FirstOrDefault(x => x.id == id).ClientCar.Client;
            NavigationService.Navigate(new ClientPage(client));
        }
    }
}
