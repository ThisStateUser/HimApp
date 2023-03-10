using HimApp.BD;
using HimApp.Controllers;
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
    }

    public partial class AddOrder : Page
    {
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
            additionalVis(true);
            //condition.ItemsSource = HimBDEntities.GetContext().Conditions.ToList();
        }

        private void additionalVis(bool shide)
        {
            if (shide)
            {
                MainAddCar.Visibility = Visibility.Visible;
                AddCar.Visibility = Visibility.Visible;
                AddNumCar.Visibility = Visibility.Collapsed;
            } else
            {
                MainAddCar.Visibility = Visibility.Collapsed;
                AddCar.Visibility = Visibility.Collapsed;
                AddNumCar.Visibility = Visibility.Visible;
            }
        }


        private void PageBack_Click(object sender, RoutedEventArgs e)
        {
            if (PageSetIndex > 0)
                PageSetIndex = PageSetIndex - 1;
            ShowPage();
        }
        private void PageNext_Click(object sender, RoutedEventArgs e)
        {
            if (PageSetIndex < 2)
                PageSetIndex = PageSetIndex + 1;
            ShowPage();
        }

        private void ShowPage()
        {
            OrderSetPageOne.Visibility = Visibility.Collapsed;
            OrderSetPageTwo.Visibility = Visibility.Collapsed;
            OrderSetPageThree.Visibility = Visibility.Collapsed;

            switch (PageSetIndex)
            {
                case 1:
                    PageBack.Visibility = Visibility.Visible;
                    OrderSetPageTwo.Visibility = Visibility.Visible;
                    break;
                case 2:
                    PageBack.Visibility = Visibility.Visible;
                    OrderSetPageThree.Visibility = Visibility.Visible;
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
        }

        private void addAutoForm_Click(object sender, RoutedEventArgs e)
        {
            if(OrderComplit.client == null)
            {
                MainVoid.ErrorMessage("Выберите клиента.");
                return;
            }
            additionalVis(true);
            HideOtherPage();
            firstloadcheck.IsChecked = true;
            TitleOther.Text = "Выбор автомобиля";
            AutoPageOther.Visibility = Visibility.Visible;
        }

        private void HideOtherPage()
        {
            DopPageOther.Visibility = Visibility.Collapsed;
            ClientPageOther.Visibility = Visibility.Collapsed;
            AutoPageOther.Visibility = Visibility.Collapsed;
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            if(phone_client.Text.Length < 11)
            {
                MainVoid.ErrorMessage("Заполните номер телефона");
                return;
            }
            try
            {
                if (HimBDEntities.GetContext().Client.Where(x => x.phone.Trim() == phone_client.Text.Trim()).FirstOrDefault() != null)
                {
                    MainVoid.InformationMessage("Клиент существует");
                    return;
                }
                HimBDEntities.GetContext().Client.Add(new Client()
                {
                    first_name = first_name_client.Text.Trim(),
                    last_name = last_name_client.Text.Trim(),
                    phone = phone_client.Text.Trim(),
                });
                HimBDEntities.GetContext().SaveChanges();
                MainVoid.InformationMessage($"Клиент {phone_client.Text.Trim()} добавлен.");
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.ToString());
            }
        }

        private void phone_client_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".")
            && !phone_client.Text.Contains(".")
            && phone_client.Text.Length != 0))
            {
                e.Handled = true;
            }
        }

        private void SelectClient_Click(object sender, RoutedEventArgs e)
        {
            Button thisbt = (Button)sender;
            int ltp = Convert.ToInt32(thisbt.Tag.ToString());
            OrderComplit.client = HimBDEntities.GetContext().Client.Where(x => x.id == ltp).FirstOrDefault();
            info_Client.Text = OrderComplit.client.first_name + " " + OrderComplit.client.last_name;
            info_ClientNumber.Text = OrderComplit.client.phone;
            HideOtherPage();
            OrderComplit.car = null;
            OrderComplit.client_car = null;
            info_ClientCar.Text = "";
            info_ClientCarNumber.Text = "";
            DopPageOther.Visibility = Visibility.Visible;
        }
        private void SelectCarClient_Click(object sender, RoutedEventArgs e)
        {
            Button thisbt = (Button)sender;
            int ltp = Convert.ToInt32(thisbt.Tag.ToString());
            OrderComplit.car = HimBDEntities.GetContext().Cars.Where(x => x.id == ltp).FirstOrDefault();
            if (OrderComplit.client.ClientCar.Where(x => x.car_id == ltp).FirstOrDefault() == null)
            {
                MessageBoxResult result = MessageBox.Show("Добавить новый автомобиль клиенту?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                    additionalVis(false);
                else
                    return;
                return;
            }
            OrderComplit.client_car = HimBDEntities.GetContext().ClientCar.Where(x => x.car_id == ltp && x.client_id == OrderComplit.client.id).FirstOrDefault();
            info_ClientCar.Text = ($"{OrderComplit.car.car_brand} {OrderComplit.car.car_model}");
            info_ClientCarNumber.Text = OrderComplit.client_car.car_number;
            HideOtherPage();
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
                OrderComplit.client_car = HimBDEntities.GetContext().ClientCar.Where(x => x.car_id == OrderComplit.car.id && x.client_id == OrderComplit.client.id).FirstOrDefault();
                MainVoid.InformationMessage($"Автомобиль {OrderComplit.car.car_brand} {OrderComplit.car.car_model} добавлен клиенту {OrderComplit.client.first_name} {OrderComplit.client.last_name}.");
                info_ClientCar.Text = ($"{OrderComplit.car.car_brand} {OrderComplit.car.car_model}");
                info_ClientCarNumber.Text = OrderComplit.client_car.car_number;
                HideOtherPage();
                DopPageOther.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.ToString());
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
                //обработка нескольких номеров на автомобиль у клиента
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

                OrderComplit.car = HimBDEntities.GetContext().Cars.Where(x =>
                                        x.car_brand.ToLower().Trim() == brand_car.Text.ToLower().Trim() &&
                                        x.car_model.ToLower().Trim() == model_car.Text.ToLower().Trim()).FirstOrDefault();
                if (OrderComplit.car != null)
                {
                    add();
                    return;
                }
                OrderComplit.car = HimBDEntities.GetContext().Cars.Add(new Cars()
                {
                    car_brand = brand_car.Text.Trim(),
                    car_model = model_car.Text.Trim(),
                    car_body_id = ((CarBody)category_car.SelectedItem).id,
                });
                add();

                MainVoid.InformationMessage($"Автомобиль {brand_car.Text} {model_car.Text} добавлен клиенту {OrderComplit.client.first_name} {OrderComplit.client.last_name}.");
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HideOtherPage();
            DopPageOther.Visibility = Visibility.Visible;
        }

        private void carout_Checked(object sender, RoutedEventArgs e)
        {
            DG_client_car.ItemsSource = HimBDEntities.GetContext().Cars.ToList();
        }

        private void carout_Checked_1(object sender, RoutedEventArgs e)
        {
            DG_client_car.ItemsSource = HimBDEntities.GetContext().Cars.Where(x => x.id == x.ClientCar.Where(z => z.client_id == OrderComplit.client.id).FirstOrDefault().car_id).ToList();
        }

    }
}


//<Border Grid.RowSpan="2" Grid.Column="1" Style="{DynamicResource BorderPanel}" Width="400" Height="560">
//    <ScrollViewer>
//        <StackPanel Width="350" HorizontalAlignment="Left">
//            <TextBlock Text="Заказ" Style="{DynamicResource TitleText}" Margin="0 0 0 5"/>
//            <TextBlock Text="Ответственный за заказ"/>
//            <ComboBox x:Name="executor" >
//                <ComboBox.ItemTemplate>
//                    <DataTemplate>
//                        <StackPanel Orientation="Horizontal">
//                            <TextBlock Text="{Binding UserInfo.first_name}" Foreground="{DynamicResource Text}" Margin="0 0 5 0"/>
//                            <TextBlock Text="{Binding UserInfo.last_name}" Foreground="{DynamicResource Text}"/>
//                        </StackPanel>
//                    </DataTemplate>
//                </ComboBox.ItemTemplate>
//            </ComboBox>

//            <TextBlock Text="Состав заказа"/>
//            <WrapPanel>
//                <Button>
//                    <Icon:PackIconMaterial Kind="Plus"/>
//                </Button>
//            </WrapPanel>
//            <TextBlock Text="Заметка к заказу"/>
//            <RichTextBox x:Name="comments"/>
//            <TextBlock Text="Стоимость работ"/>
//            <TextBox x:Name="cost"/>
//            <TextBlock Text="Предоплата (при наличии)"/>
//            <TextBox x:Name="prepay"/>
//            <TextBlock Text="Дата записи"/>
//            <DatePicker CalendarStyle="{DynamicResource Calendars}" x:Name="arrival"/>
//            <TextBlock Text="Дата выдачи"/>
//            <DatePicker CalendarStyle="{DynamicResource Calendars}" SelectedDate="10 10" x:Name="departure"/>
//            <Button Margin="0 10 0 0">
//                <StackPanel Orientation="Horizontal">
//                    <TextBlock Text="Добавить" Foreground="{DynamicResource TextBtn}"/>
//                </StackPanel>
//            </Button>
//        </StackPanel>
//    </ScrollViewer>
//</Border>