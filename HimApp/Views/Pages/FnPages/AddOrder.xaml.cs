using HimApp.BD;
using HimApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace HimApp.Views.Pages.FnPages
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    
    public class OrderComplit
    {
        //client
        public static Client client;
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
            //condition.ItemsSource = HimBDEntities.GetContext().Conditions.ToList();
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
            HideOtherPage();
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
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            //if (phone_client.Text.Length < 11)
            //{
            //    MainVoid.ErrorMessage("Заполните номер телефона");
            //    return;
            //}
            //try
            //{
            //    if (HimBDEntities.GetContext().Client.Where(x => x.phone.Trim() == phone_client.Text.Trim()).FirstOrDefault() != null)
            //    {
            //        MainVoid.InformationMessage("Клиент существует");
            //        return;
            //    }
            //    HimBDEntities.GetContext().Client.Add(new Client()
            //    {
            //        first_name = first_name_client.Text.Trim(),
            //        last_name = last_name_client.Text.Trim(),
            //        phone = phone_client.Text.Trim(),
            //    });
            //    HimBDEntities.GetContext().SaveChanges();
            //    MainVoid.InformationMessage($"Клиент {phone_client.Text.Trim()} добавлен.");
            //}
            //catch (Exception ex)
            //{
            //    MainVoid.FatalErrorMessage(ex.ToString());
            //}
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