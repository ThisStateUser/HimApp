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

namespace HimApp.Views.Pages.FnPages
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Page
    {
        public AddOrder()
        {
            InitializeComponent();
            WConnect.MainWindowMethod.PageTitle.Text = "Новый заказ";
            PreLoad();
        }

        private void PreLoad()
        {
            executor.ItemsSource = HimBDEntities.GetContext().Users.ToList();
            condition.ItemsSource = HimBDEntities.GetContext().Conditions.ToList();
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