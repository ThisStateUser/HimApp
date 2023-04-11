using HimApp.BD;
using HimApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
        UserInfo user = null;

        public EmployeesPage()
        {
            InitializeComponent();
            WConnect.MainWindowMethod.PageTitle.Text = "Сотрудники";
            updDG();
        }

        private void updDG()
        {
            EmployUser.ItemsSource = HimBDEntities.GetContext().Users.Where(x => x.UserInfo.role_id == 2).ToList();
        }

        private void showprofile()
        {
            profile_info.Visibility = Visibility.Visible;
            profile_btn.Visibility = Visibility.Visible;
        }


        private void addEmployees_Click(object sender, RoutedEventArgs e)
        {
            titleaddedit.Text = "Добавление сотрудника";
            addEmployeesBD.Visibility = Visibility.Visible;
            editEmployeesBD.Visibility = Visibility.Collapsed;
            EmployUser.Visibility = Visibility.Collapsed;
            addemp.Visibility = Visibility.Visible;
            addEmployees.Visibility = Visibility.Collapsed;
            cancelEmployees.Visibility= Visibility.Visible;
            profile_info.Visibility = Visibility.Visible;
            clearinfo();
        }

        private void clearinfo()
        {
            FirstName.Text = "";
            LastName.Text = "";
            role.Text = "";
            personal_account.Text = "";
            location.Text = "";
            schedule.Text = "";
            Phone.Text = "";
        }
        private void cancelEmployees_Click(object sender, RoutedEventArgs e)
        {
            EmployUser.Visibility = Visibility.Visible;
            addemp.Visibility = Visibility.Collapsed;
            addEmployees.Visibility = Visibility.Visible;
            cancelEmployees.Visibility = Visibility.Collapsed;
            profile_btn.Visibility = Visibility.Collapsed;
            profile_info.Visibility = Visibility.Collapsed;
            clearinfo();
        }

        private string Valid(bool s = true)
        {
            List<string> list = new List<string>();
            if (login_user.Text.Length < 1)
                list.Add("Вы не указали логин сотрудника");
            if(s)
            {
                if (HimBDEntities.GetContext().Users.Any(x => x.login.Trim().ToLower() == login_user.Text.Trim().ToLower()))
                    list.Add("Сотрудник с таким логином уже существует");
                if (password_user.Text.Length < 1)
                    list.Add("Вы не указали пароль сотрудника");
            }
            if (first_name.Text.Length < 1)
                list.Add("Вы не указали имя сотрудника");
            if (last_name.Text.Length < 1)
                list.Add("Вы не указали фамилия сотрудника");
            if (phone.Text.Length < 1)
                list.Add("Вы не указали телефон сотрудника");
            if (personal_acc.Text.Length < 1)
                list.Add("Вы не указали лицевой счет сотрудника");
            if (location_user.Text.Length < 1)
                list.Add("Вы не указали место жительства сотрудника");
            if (schedule_user.Text.Length < 1)
                list.Add("Вы не указали график работы сотрудника");
            if (list.Count > 0)
            {
                string result = "Вы допустили данные ошибки: \n";
                foreach (var item in list)
                    result += item + "\n";
                return result;
            }
            return "";
        }

        private void addEmployeesBD_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Valid()))
            {
                MainVoid.ErrorMessage(Valid());
                return;
            }
            try
            {
                UserInfo userInfo = HimBDEntities.GetContext().UserInfo.Add(new UserInfo()
                {
                    first_name = first_name.Text.Trim(),
                    last_name = last_name.Text.Trim(),
                    phone = phone.Text.Trim(),
                    personal_account = personal_acc.Text.Trim(),
                    location = location_user.Text.Trim(),
                    schedule = schedule.Text.Trim(),
                    role_id = 2,
                    created_at = DateTime.Now,
                });
                HimBDEntities.GetContext().SaveChanges();
                HimBDEntities.GetContext().Users.Add(new Users()
                {
                    login = login_user.Text.Trim(),
                    password = password_user.Text.Trim(),
                    userinfo_id = userInfo.id,
                });
                HimBDEntities.GetContext().SaveChanges();
                MainVoid.InformationMessage($"Сотрудник {last_name.Text.Trim()} {first_name.Text.Trim()} добавлен");
                updDG();
                cancelEmployees_Click(null, null);
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
            
        }

        private void edituser_Click(object sender, RoutedEventArgs e)
        {
            titleaddedit.Text = "Редактирование сотрудника";
            addEmployeesBD.Visibility = Visibility.Collapsed;
            editEmployeesBD.Visibility = Visibility.Visible;
            EmployUser.Visibility = Visibility.Collapsed;
            addemp.Visibility = Visibility.Visible;
            addEmployees.Visibility = Visibility.Collapsed;
            cancelEmployees.Visibility = Visibility.Visible;
            profile_info.Visibility = Visibility.Visible;

            login_user.Text = HimBDEntities.GetContext().Users.FirstOrDefault(x => x.userinfo_id == user.id).login;
            password_user.Text = HimBDEntities.GetContext().Users.FirstOrDefault(x => x.userinfo_id == user.id).password;
            first_name.Text = user.first_name;
            last_name.Text = user.last_name;
            role.Text = user.Roles.role_name;
            personal_acc.Text = user.personal_account;
            location_user.Text = user.location;
            schedule_user.Text = user.schedule;
            phone.Text = user.phone;
        }

        private void deleteuser_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить сотрудника {user.last_name} {user.first_name}?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;
            try
            {
                Users userobj = HimBDEntities.GetContext().Users.FirstOrDefault(x => x.userinfo_id == user.id);
                void removeuser()
                {
                    HimBDEntities.GetContext().Users.Remove(userobj);
                    HimBDEntities.GetContext().UserInfo.Remove(user);
                    HimBDEntities.GetContext().SaveChanges();
                    MainVoid.InformationMessage("Сотрудник удален.");
                }

                if (HimBDEntities.GetContext().Order.Any(x => x.executor_id == userobj.id))
                {
                    MessageBoxResult userresult = MessageBox.Show("Пользователь является исполнителем в нескольких заказах, заменить исполнителя на администратора?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (userresult == MessageBoxResult.No)
                        return;
                    foreach (var item in HimBDEntities.GetContext().Order.Where(x => x.executor_id == userobj.id))
                    {
                        item.executor_id = UserObj.UserAcc.id;
                    }
                    HimBDEntities.GetContext().SaveChanges();
                    updDG();
                    removeuser();
                    return;
                }
                removeuser();
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }

        }

        private void editEmployeesBD_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Valid(false)))
            {
                MainVoid.ErrorMessage(Valid());
                return;
            }
            try
            {
                HimBDEntities.GetContext().Users.FirstOrDefault(x => x.userinfo_id == user.id).login = login_user.Text.Trim();
                HimBDEntities.GetContext().Users.FirstOrDefault(x => x.userinfo_id == user.id).password = password_user.Text.Trim();
                user.first_name = first_name.Text.Trim();
                user.last_name = last_name.Text.Trim();
                user.phone = phone.Text.Trim();
                user.personal_account = personal_acc.Text.Trim();
                user.location = location_user.Text.Trim();
                user.schedule = schedule.Text.Trim();
                user.updated_at = DateTime.Now;

                HimBDEntities.GetContext().SaveChanges();
                MainVoid.InformationMessage($"Данные обновлены");
                updDG();
                cancelEmployees_Click(null, null);
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }

        private void preview_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (((TextBox)sender).Name)
            {
                case "first_name":
                    FirstName.Text = ((TextBox)sender).Text.Trim();
                    break;
                case "last_name":
                    LastName.Text = ((TextBox)sender).Text.Trim();
                    break;
                case "phone":
                    Phone.Text = ((TextBox)sender).Text.Trim();
                    break;
                case "personal_acc":
                    personal_account.Text = ((TextBox)sender).Text.Trim();
                    break;
                case "location_user":
                    location.Text = ((TextBox)sender).Text.Trim();
                    break;
                case "schedule_user":
                    schedule.Text = ((TextBox)sender).Text.Trim();
                    break;
            }
        }

        private void preview_KeyDown(object sender, KeyEventArgs e)
        {
            switch (((TextBox)sender).Name)
            {
                case "login_user":
                    if (e.Key == Key.Enter)
                        password_user.Focus();
                    break;
                case "password_user":
                    if (e.Key == Key.Enter)
                        first_name.Focus();
                    break;
                case "first_name":
                    if (e.Key == Key.Enter)
                        last_name.Focus();
                    break;
                case "last_name":
                    if (e.Key == Key.Enter)
                        phone.Focus();
                    break;
                case "phone":
                    if (e.Key == Key.Enter)
                        personal_acc.Focus();
                    break;
                case "personal_acc":
                    if (e.Key == Key.Enter)
                        location_user.Focus();
                    break;
                case "location_user":
                    if (e.Key == Key.Enter)
                        schedule_user.Focus();
                    break;
            }
        }

        private void EmployUser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int id = ((Users)EmployUser.SelectedItem).id;

            user = HimBDEntities.GetContext().UserInfo.FirstOrDefault(x => x.id == id);
            if (user == null)
            {
                MainVoid.ErrorMessage("Пользователь не найден");
                return;
            }

            addEmployees.Visibility = Visibility.Collapsed;
            cancelEmployees.Visibility = Visibility.Visible;

            FirstName.Text = user.first_name;
            LastName.Text = user.last_name;
            role.Text = user.Roles.role_name;
            personal_account.Text = user.personal_account;
            location.Text = user.location;
            schedule.Text = user.schedule;
            Phone.Text = user.phone;

            profile_btn.Visibility = Visibility.Visible;
            profile_info.Visibility = Visibility.Visible;
        }
    }
}
