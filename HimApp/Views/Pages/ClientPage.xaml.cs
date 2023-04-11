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
using System.Xml.Linq;
using HimApp.BD;
using HimApp.Controllers;

namespace HimApp.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        private Client client { get; set; }

        public ClientPage(Client clientObj)
        {
            InitializeComponent();
            client = clientObj;
            PreLoad();
        }

        private void PreLoad()
        {
            ClientFirstName.Text = client.first_name;
            ClientLastName.Text = client.last_name;
            ClientPhone.Text = client.phone;
            status.ItemsSource = HimBDEntities.GetContext().Status.ToList();
            upgDG();

            if (UserObj.UserAcc.UserInfo.role_id == 2)
                edit.Visibility = Visibility.Collapsed;
            else
                edit.Visibility = Visibility.Visible;
        }
        
        private void upgDG()
        {
            DG_Order.ItemsSource = HimBDEntities.GetContext().Order.Where(x => x.ClientCar.client_id == client.id).ToList();
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            f_name.Text = client.first_name;
            l_name.Text = client.last_name;
            e_phone.Text = client.phone;
            edit.Visibility = Visibility.Collapsed;
            editzone.Visibility = Visibility.Visible;
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            editzone.Visibility = Visibility.Collapsed;
            edit.Visibility = Visibility.Visible;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.first_name = f_name.Text.Trim();
                client.last_name = l_name.Text.Trim();
                client.phone = e_phone.Text.Trim();
                HimBDEntities.GetContext().SaveChanges();
                MainVoid.InformationMessage("Данные обновлены");

            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }

        Order openorder;
        private void DG_Order_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int id = ((Order)DG_Order.SelectedItem).id;
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
                upgDG();
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }
    }
}
