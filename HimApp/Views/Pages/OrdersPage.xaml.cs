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
                orderinfo.Visibility = Visibility.Collapsed;
            }
            else
            {
                updDGOrder();
                emptyborder.Visibility = Visibility.Collapsed;
                DG_orders.Visibility = Visibility.Visible;
                status.ItemsSource = HimBDEntities.GetContext().Status.ToList();
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

        Order openorder;
        private void DG_orders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int id = ((Order)DG_orders.SelectedItem).id;
            Order order = HimBDEntities.GetContext().Order.FirstOrDefault(x => x.id == id);
            if (order == null)
            {
                MainVoid.ErrorMessage("Заказ не найден");
                return;
            }

            openorder = order;
            Order_num.Text = $"Заказ № {order.id}";
            executer.Text = $"Исполнитель: {order.Users.UserInfo.last_name} {order.Users.UserInfo.first_name}";
            client_zone.Text = $"Клиент: {order.ClientCar.Client.last_name} {order.ClientCar.Client.first_name} {order.ClientCar.Client.phone}";
            client_car.Text = $"Автомобиль клиента: {order.ClientCar.Cars.car_brand} {order.ClientCar.Cars.car_model} {order.ClientCar.car_number}";
            condition.Text = $"Состояние автомобиля: {order.Conditions.condition_rate}";
            cost.Text = $"Стоимость работ: {order.custom_cost}";
            prepay.Text = $"Предоплата: {order.prepayment}";
            arrival.Text = $"Дата принятия: {order.arrival_date.Value.ToShortDateString()}";
            departure.Text = $"Дата выдачи: {order.departure_date.Value.ToShortDateString()}";
            List<OrderSet> orderSet = HimBDEntities.GetContext().OrderSet.Where(x => x.group_id == order.order_group_id).ToList();
            var list = from t in orderSet select new { title = (t.PresetGroup == null ? t.Services.title : t.PresetGroup.title) };
            DG_Service.ItemsSource = list;
            status.SelectedItem = order.Status;
            comment.Text = $"Комментарий к заказу: {order.comments}";
            orderinfo.Visibility = Visibility.Visible;
        }

        private void status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                openorder.status_id = ((Status)status.SelectedItem).id;
                HimBDEntities.GetContext().SaveChanges();
                updDGOrder(search.Text.Trim().ToLower());
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }

        private void GoClient_Click(object sender, RoutedEventArgs e)
        {
            Client client = HimBDEntities.GetContext().Order.FirstOrDefault(x => x.id == openorder.id).ClientCar.Client;
            NavigationService.Navigate(new ClientPage(client));
            orderinfo.Visibility = Visibility.Visible;
        }

        private void CheckOut_Click(object sender, RoutedEventArgs e)
        {
            var application = new Microsoft.Office.Interop.Word.Application();
            var document = application.Documents.Add();
        }
    }
}
