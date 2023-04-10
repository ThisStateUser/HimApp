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
    }
}
