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
            List<Order> orders = HimBDEntities.GetContext().Order.Where(x => x.arrival_date >= DateTime.Today && x.arrival_date <= overdate).OrderBy(x => x.arrival_date).ToList();
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
            status.ItemsSource = HimBDEntities.GetContext().Status.ToList();
            DG_bestActivClient.ItemsSource = HimBDEntities.GetContext().ClientStat.OrderByDescending(x => x.orders).Take(10).ToList();
        }

        private void updLV()
        {
            DateTime overdate = DateTime.Today.AddDays(Properties.Settings.Default.AddDays);
            List<Order> orders = HimBDEntities.GetContext().Order.Where(x => x.arrival_date >= DateTime.Today && x.arrival_date <= overdate).OrderBy(x => x.arrival_date).ToList();
            orderFirst.ItemsSource = orders;
        }

        private void FOrder_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                FOrder.ScrollToHorizontalOffset(FOrder.HorizontalOffset + (e.Delta - 10) * -1);
            else if (e.Delta < 0)
                FOrder.ScrollToHorizontalOffset(FOrder.HorizontalOffset + (e.Delta + 10) * -1);
            e.Handled = true;
        }

        Order openorder;
        private void moreinfo_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(((Button)sender).Tag.ToString());
            Order order = HimBDEntities.GetContext().Order.FirstOrDefault(x => x.id == id);
            if (order == null)
            {
                MainVoid.ErrorMessage("Заказ не найден");
                return;
            }
            openorder = order;
            Order_num.Text = $"Заказ № {order.id}";
            executer.Text = $"Исполнитель: {order.Users.UserInfo.last_name} {order.Users.UserInfo.first_name}";
            client.Text = $"Клиент: {order.ClientCar.Client.last_name} {order.ClientCar.Client.first_name} {order.ClientCar.Client.phone}";
            client_car.Text = $"Автомобиль клиента: {order.ClientCar.Cars.car_brand} {order.ClientCar.Cars.car_model} {order.ClientCar.car_number}";
            condition.Text = $"Состояние автомобиля: {order.Conditions.condition_rate}";
            cost.Text = $"Стоимость работ: {order.custom_cost}";
            prepay.Text = $"Предоплата: {order.prepayment}";
            arrival.Text = $"Дата принятия: {order.arrival_date.Value.ToShortDateString()}";
            departure.Text = $"Дата выдачи: {order.departure_date.Value.ToShortDateString()}";
            List<OrderSet> orderSet = HimBDEntities.GetContext().OrderSet.Where(x => x.group_id == order.order_group_id).ToList();
            var list = from t in orderSet select new {title = (t.PresetGroup==null? t.Services.title : t.PresetGroup.title)};
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
                updLV();
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }
    }
}
