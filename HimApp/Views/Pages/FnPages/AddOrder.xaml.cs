using HimApp.BD;
using HimApp.Controllers;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Policy;
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
using static System.Net.WebRequestMethods;

namespace HimApp.Views.Pages.FnPages
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>

    public class OrderComplit
    {
        //client
        public static Client client;
        public static Cars car;
        public static ClientCar client_car;
        public static Order order = new Order();
        public static OrderGroup orderGroup;
        public static List<OrderSet> orderSet = new List<OrderSet>();
    }

    public class SorP
    {
        public string tag { get; set; }
        public string title { get; set; }
        public double cost { get; set; }
    }

    public partial class AddOrder : Page
    {
        List<object> listService = new List<object>();
        int PageSetIndex = 0;
        public AddOrder()
        {
            InitializeComponent();
            WConnect.MainWindowMethod.PageTitle.Text = "Новый заказ";
            PreLoad();
        }

        private void PreLoad()
        {
            ShowPage();
            executor.ItemsSource = HimBDEntities.GetContext().Users.ToList();
            category_car.ItemsSource = HimBDEntities.GetContext().CarBody.ToList();
            ClientList.ItemsSource = HimBDEntities.GetContext().Client.ToList();
            condition.ItemsSource = HimBDEntities.GetContext().Conditions.ToList();
            additionalVis(true);
            updLVservice();
        }

        private void updLVservice()
        {
            foreach (var item in HimBDEntities.GetContext().Services.ToList())
                listService.Add(new SorP() { tag = $"S{item.id}", title = item.title, cost = item.cost });
            foreach (var item in HimBDEntities.GetContext().PresetGroup.ToList())
                listService.Add(new SorP() { tag = $"P{item.id}", title = item.title, cost = item.cost });
            searchService();
        }
        private void searchService(string search = "")
        {
            if (search == "")
            {
                LV_services.ItemsSource = listService;
                return;
            }
            LV_services.ItemsSource = listService.Where(x => ((SorP)x).title.Trim().ToLower().Contains(search) || 
                                                             ((SorP)x).cost.ToString().Trim().ToLower().StartsWith(search)
                                                       ).ToList();
        }
        private void updDGcar(string search = "")
        {
            if (search == "")
            {
                DG_car.ItemsSource = HimBDEntities.GetContext().Cars.ToList();
                DG_client_car.ItemsSource = HimBDEntities.GetContext().ClientCar.Where(x => x.client_id == OrderComplit.client.id).ToList();
                return;
            }

            DG_car.ItemsSource = HimBDEntities.GetContext().Cars
                .Where(x => x.car_model.Trim().ToLower().Contains(search) || 
                            x.car_brand.Trim().ToLower().Contains(search)
                ).ToList();
            
            DG_client_car.ItemsSource = HimBDEntities.GetContext().ClientCar
                .Where(x => x.client_id == OrderComplit.client.id)
                .Where(z => z.Cars.car_model.Trim().ToLower().Contains(search) || 
                            z.car_number    .Trim().ToLower().Contains(search) ||
                            z.Cars.car_brand.Trim().ToLower().Contains(search)
                      ).ToList();
        }

        private void updDGservice()
        {
            List<object> list = new List<object>();
            foreach (var item in OrderComplit.orderSet)
            {
                if (item.preset_id != null)
                    list.Add(HimBDEntities.GetContext().PresetGroup.FirstOrDefault(x => x.id == item.preset_id));
                if (item.service_id != null)
                    list.Add(HimBDEntities.GetContext().Services.FirstOrDefault(x => x.id == item.service_id));
            }
            DG_SelectService.ItemsSource = list;
        }

        private void additionalVis(bool shide)
        {
            if (shide)
            {
                MainAddCar.Visibility = Visibility.Visible;
                AddCar.Visibility = Visibility.Visible;
                AddNumCar.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainAddCar.Visibility = Visibility.Collapsed;
                AddCar.Visibility = Visibility.Collapsed;
                AddNumCar.Visibility = Visibility.Visible;
            }
        }


        private void PageBack_Click(object sender, RoutedEventArgs e)
        {
            if (PageSetIndex == 1)
            {
                PageNextIcon.Kind = PackIconMaterialKind.ArrowRight;
                PageNextTxt.Text = "Далее";
            }
            if (PageSetIndex > 0)
                PageSetIndex = PageSetIndex - 1;
            ShowPage();
        }
        private void PageNext_Click(object sender, RoutedEventArgs e)
        {
            if (PageSetIndex == 0)
            {
                List<string> list = new List<string>();
                if (OrderComplit.client == null)
                    list.Add("Вы не выбрали клиента");
                if (OrderComplit.car == null)
                    list.Add("Вы не выбрали автомобиль клиента");
                if (condition.SelectedItem == null)
                    list.Add("Вы не выбрали состояние автомобиля");
                if (OrderComplit.orderSet.Count == 0)
                    list.Add("Вы не выбрали состав заказа");
                if (list.Count > 0)
                {
                    string result = "Вы допустили данные ошибки: \n";
                    foreach (var item in list)
                        result += item + "\n";
                    MainVoid.ErrorMessage(result);
                    return;
                }

                OrderComplit.order.client_car_id = OrderComplit.client_car.id;
                OrderComplit.order.condition_id = ((Conditions)condition.SelectedItem).id;
                OrderComplit.order.comments = comments.Text;
                PageNextIcon.Kind = PackIconMaterialKind.Check;
                PageNextTxt.Text = "Готово";
            }

            if (PageSetIndex == 1)
            {
                List<string> list = new List<string>();
                if (executor.SelectedItem == null)
                    list.Add("Вы не выбрали ответственного за заказ");
                if (cost.Text.Length < 1)
                    list.Add("Вы не указали стоимость работ");
                if (arrival.SelectedDate == null)
                    list.Add("Вы не выбрали дату приема");
                if (departure.SelectedDate == null)
                    list.Add("Вы не выбрали дату выдачи");
                if (departure.SelectedDate < arrival.SelectedDate)
                    list.Add("Дата приема не может быть позднее даты выдачи");
                                    
                if (list.Count > 0)
                {
                    string result = "Вы допустили данные ошибки: \n";
                    foreach (var item in list)
                        result += item + "\n";
                    MainVoid.ErrorMessage(result);
                    return;
                }

                OrderComplit.order.executor_id = ((Users)executor.SelectedItem).id;
                OrderComplit.order.custom_cost = double.Parse(cost.Text);
                OrderComplit.order.prepayment = double.Parse(prepay.Text);
                OrderComplit.order.arrival_date = arrival.SelectedDate;
                OrderComplit.order.departure_date = departure.SelectedDate;
                OrderComplit.order.status_id = 3;
                OrderComplit.order.order_group_id = OrderComplit.orderGroup.id;
                try
                {
                    HimBDEntities.GetContext().OrderSet.AddRange(OrderComplit.orderSet);
                    HimBDEntities.GetContext().Order.Add(OrderComplit.order);
                    HimBDEntities.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MainVoid.FatalErrorMessage(ex.Message);
                }
                MainVoid.InformationMessage("Заказ добавлен");
                WConnect.MainWindowMethod.FrameM.Navigate(new HomePage());
            }


            if (PageSetIndex < 1)
                PageSetIndex = PageSetIndex + 1;
            ShowPage();
        }

        private void ShowPage()
        {
            OrderSetPageOne.Visibility = Visibility.Collapsed;
            OrderSetPageTwo.Visibility = Visibility.Collapsed;

            switch (PageSetIndex)
            {
                case 1:
                    PageBack.Visibility = Visibility.Visible;
                    OrderSetPageTwo.Visibility = Visibility.Visible;
                    break;
                default:
                    PageBack.Visibility = Visibility.Collapsed;
                    OrderSetPageOne.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void addClientForm_Click(object sender, RoutedEventArgs e)
        {
            HideOtherPage();
            TitleOther.Text = "Выбор клиента";
            ClientPageOther.Visibility = Visibility.Visible;
            ClientList.ItemsSource = HimBDEntities.GetContext().Client.ToList();
        }

        private void addAutoForm_Click(object sender, RoutedEventArgs e)
        {
            if (OrderComplit.client == null)
            {
                MainVoid.ErrorMessage("Выберите клиента.");
                return;
            }
            additionalVis(true);
            HideOtherPage();
            updDGcar();
            if (HimBDEntities.GetContext().ClientCar.FirstOrDefault(x => x.client_id == OrderComplit.client.id) == null)
                firstloadcheck.IsChecked = true;
            else
                secondloadcheck.IsChecked = true;
            TitleOther.Text = "Выбор автомобиля";
            AutoPageOther.Visibility = Visibility.Visible;
        }

        private void HideOtherPage()
        {
            ServicePageOther.Visibility = Visibility.Collapsed;
            DopPageOther.Visibility = Visibility.Collapsed;
            ClientPageOther.Visibility = Visibility.Collapsed;
            AutoPageOther.Visibility = Visibility.Collapsed;
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            if (phone_client.Text.Length < 11)
            {
                MainVoid.ErrorMessage("Заполните номер телефона");
                return;
            }
            try
            {
                if (HimBDEntities.GetContext().Client.FirstOrDefault(x => x.phone.Trim() == phone_client.Text.Trim()) != null)
                {
                    MainVoid.InformationMessage("Клиент существует");
                    return;
                }
                Client client = HimBDEntities.GetContext().Client.Add(new Client()
                {
                    first_name = first_name_client.Text.Trim(),
                    last_name = last_name_client.Text.Trim(),
                    phone = phone_client.Text.Trim(),
                });
                HimBDEntities.GetContext().ClientStat.Add(new ClientStat()
                {
                    client_id = client.id,
                });
                HimBDEntities.GetContext().SaveChanges();
                MainVoid.InformationMessage($"Клиент \"{phone_client.Text.Trim()}\" добавлен.");
                ClientList.ItemsSource = HimBDEntities.GetContext().Client.ToList();
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }

        }

        private void phone_client_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            MainVoid.OnlyNumber((TextBox)sender, e);
        }

        private void SelectClient_Click(object sender, RoutedEventArgs e)
        {
            Button thisbt = (Button)sender;
            int ltp = Convert.ToInt32(thisbt.Tag.ToString());
            OrderComplit.client = HimBDEntities.GetContext().Client.FirstOrDefault(x => x.id == ltp);
            if (OrderComplit.client.first_name.Length < 1 || OrderComplit.client.last_name.Length < 1)
                info_Client.Text = "Неизвестно";
            else
                info_Client.Text = ($"{OrderComplit.client.first_name} {OrderComplit.client.last_name}");
            info_ClientNumber.Text = OrderComplit.client.phone;
            HideOtherPage();
            OrderComplit.car = null;
            OrderComplit.client_car = null;
            info_ClientCar.Text = "";
            info_ClientCarNumber.Text = "";
            orderbox_car.Text = "";
            DopPageOther.Visibility = Visibility.Visible;
        }
        private void SelectCarClient_Click(object sender, RoutedEventArgs e)
        {
            Button thisbt = (Button)sender;
            int ltp = Convert.ToInt32(thisbt.Tag.ToString());
            OrderComplit.car = HimBDEntities.GetContext().Cars.FirstOrDefault(x => x.id == ltp);
            MessageBoxResult result = MessageBox.Show("Добавить новый автомобиль клиенту?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                additionalVis(false);
        }

        private void SelectCar_Click(object sender, RoutedEventArgs e)
        {
            Button thisbt = (Button)sender;
            string ltp = thisbt.Tag.ToString();
            ClientCar car = HimBDEntities.GetContext().ClientCar.FirstOrDefault(z => z.car_number == ltp);
            OrderComplit.car = HimBDEntities.GetContext().Cars.FirstOrDefault(x => x.id == car.car_id);
            OrderComplit.client_car = HimBDEntities.GetContext().ClientCar.FirstOrDefault(x => x.car_number == ltp && x.client_id == OrderComplit.client.id);
            info_ClientCar.Text = ($"{OrderComplit.car.car_brand} {OrderComplit.car.car_model}");
            info_ClientCarNumber.Text = OrderComplit.client_car.car_number;
            HideOtherPage();
            orderbox_car.Text = ($"{OrderComplit.car.car_brand} {OrderComplit.car.car_model} {OrderComplit.client_car.car_number}");
            DopPageOther.Visibility = Visibility.Visible;
        }

        private void AddNumCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (num_car.Text.Length <= 2)
                {
                    MainVoid.ErrorMessage("Заполните номер автомобиля");
                    return;
                }
                HimBDEntities.GetContext().ClientCar.Add(new ClientCar()
                {
                    client_id = OrderComplit.client.id,
                    car_id = OrderComplit.car.id,
                    car_number = num_car.Text.Trim().ToUpper(),
                });
                HimBDEntities.GetContext().SaveChanges();
                OrderComplit.client_car = HimBDEntities.GetContext().ClientCar.FirstOrDefault(x => x.car_id == OrderComplit.car.id && x.client_id == OrderComplit.client.id);
                MainVoid.InformationMessage($"Автомобиль \"{OrderComplit.car.car_brand} {OrderComplit.car.car_model}\" добавлен клиенту \"{OrderComplit.client.first_name} {OrderComplit.client.last_name}\".");
                info_ClientCar.Text = ($"{OrderComplit.car.car_brand} {OrderComplit.car.car_model}");
                info_ClientCarNumber.Text = OrderComplit.client_car.car_number;
                HideOtherPage();
                DopPageOther.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {

            if (brand_car.Text.Length <= 2 || num_car.Text.Length <= 2 || model_car.Text.Length < 2 || category_car.SelectedItem == null)
            {
                MainVoid.ErrorMessage("Заполните все обязательные поля");
                return;
            }
            try
            {
                void add()
                {
                    HimBDEntities.GetContext().ClientCar.Add(new ClientCar()
                    {
                        client_id = OrderComplit.client.id,
                        car_id = OrderComplit.car.id,
                        car_number = num_car.Text.Trim().ToUpper(),
                    });
                    HimBDEntities.GetContext().SaveChanges();
                }

                OrderComplit.car = HimBDEntities.GetContext().Cars.FirstOrDefault(x =>
                                        x.car_brand.ToLower().Trim() == brand_car.Text.ToLower().Trim() &&
                                        x.car_model.ToLower().Trim() == model_car.Text.ToLower().Trim());
                if (OrderComplit.car == null)
                {
                    OrderComplit.car = HimBDEntities.GetContext().Cars.Add(new Cars()
                    {
                        car_brand = brand_car.Text.Trim(),
                        car_model = model_car.Text.Trim(),
                        car_body_id = ((CarBody)category_car.SelectedItem).id,
                    });
                }
                add();
                MainVoid.InformationMessage($"Автомобиль \"{brand_car.Text} {model_car.Text}\" добавлен клиенту \"{OrderComplit.client.first_name} {OrderComplit.client.last_name}\".");
                secondloadcheck.IsChecked = true;
                
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }

        private void carout_Checked(object sender, RoutedEventArgs e)
        {
            DG_car.Visibility = Visibility.Visible;
            DG_client_car.Visibility = Visibility.Collapsed;
        }

        private void carout_Checked_1(object sender, RoutedEventArgs e)
        {
            DG_car.Visibility = Visibility.Collapsed;
            DG_client_car.Visibility = Visibility.Visible;
        }

        private void executor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            executer_order.Text = ($"{((Users)executor.SelectedItem).UserInfo.first_name} {((Users)executor.SelectedItem).UserInfo.last_name}");
        }

        private void cost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            MainVoid.OnlyNumber((TextBox)sender, e);
        }

        private void prepay_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            MainVoid.OnlyNumber((TextBox)sender, e);
        }

        private void condition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            orderbox_condition.Text = ($"{((Conditions)condition.SelectedItem).condition_rate}");
        }

        private void arrival_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            OrderComplit.order.arrival_date = arrival.SelectedDate.Value;
            orderbox_arrival.Text = ($"{OrderComplit.order.arrival_date.Value.ToString("dd.MM.yyyy")}");
        }

        private void departure_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            OrderComplit.order.departure_date = departure.SelectedDate.Value;
            orderbox_departure.Text = ($"{OrderComplit.order.departure_date.Value.ToString("dd.MM.yyyy")}");
        }

        private void cost_TextChanged(object sender, TextChangedEventArgs e)
        {
            orderbox_cost.Text = cost.Text;
        }

        private void prepay_TextChanged(object sender, TextChangedEventArgs e)
        {
            orderbox_prepay.Text = prepay.Text;
        }

        bool isCreated = false;
        private void newGroupOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderComplit.car == null)
            {
                MainVoid.ErrorMessage("Выберите автомобиль.");
                return;
            }
            HideOtherPage();
            if (!isCreated)
            {
                try
                {
                    OrderGroup ordergadd = new OrderGroup()
                    {
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now,
                    };
                    HimBDEntities.GetContext().OrderGroup.Add(ordergadd);
                    HimBDEntities.GetContext().SaveChanges();
                    OrderComplit.orderGroup = ordergadd;
                    isCreated = true;
                    ((TextBlock)newGroupOrder.Content).Text = "Изменить";
                }
                catch (Exception ex)
                {
                    MainVoid.FatalErrorMessage(ex.Message);
                }
            }
            ServicePageOther.Visibility = Visibility.Visible;
        }

        private void Service_Checked(object sender, RoutedEventArgs e)
        {
            int OpenID = int.Parse(((CheckBox)sender).Tag.ToString().Remove(0, 1));
            Services idserv = null;
            PresetGroup idpres = null;

            if (((CheckBox)sender).Tag.ToString()[0] == 'S')
                idserv = HimBDEntities.GetContext().Services.FirstOrDefault(x => x.id == OpenID);
            else
                idpres = HimBDEntities.GetContext().PresetGroup.FirstOrDefault(x => x.id == OpenID);

            if (idpres != null)
                OrderComplit.orderSet.Add(new OrderSet()
                {
                    preset_id = OpenID,
                    group_id = OrderComplit.orderGroup.id,
                });
            if (idserv != null)
                OrderComplit.orderSet.Add(new OrderSet()
                {
                    service_id = OpenID,
                    group_id = OrderComplit.orderGroup.id,
                });
            updDGservice();
        }

        private void Service_Unchecked(object sender, RoutedEventArgs e)
        {
            int OpenID = int.Parse(((CheckBox)sender).Tag.ToString().Remove(0, 1));

            if (((CheckBox)sender).Tag.ToString()[0] == 'S')
                OrderComplit.orderSet.Remove(OrderComplit.orderSet.FirstOrDefault(x => x.service_id == OpenID));
            else
                OrderComplit.orderSet.Remove(OrderComplit.orderSet.FirstOrDefault(x => x.preset_id == OpenID));
            updDGservice();
        }

        private void SearchService_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchService(((TextBox)sender).Text.Trim().ToLower());
        }

        private void SearchCar_TextChanged(object sender, TextChangedEventArgs e)
        {
            updDGcar(((TextBox)sender).Text.Trim().ToLower());
        }
    }
}