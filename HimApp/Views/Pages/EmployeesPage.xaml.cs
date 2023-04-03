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
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            InitializeComponent();
            WConnect.MainWindowMethod.PageTitle.Text = "Сотрудники";
            EmployUser.ItemsSource = HimBDEntities.GetContext().Users.Where(x => x.UserInfo.role_id == 2).ToList();
        }

        private void showprofile()
        {
            profile_info.Visibility = Visibility.Visible;
            profile_btn.Visibility = Visibility.Visible;
        }

        private void MoreInfo_Click(object sender, RoutedEventArgs e)
        {
            WConnect.MainWindowMethod.FrameM.Navigate(new ClientPage());
        }
    }
}
