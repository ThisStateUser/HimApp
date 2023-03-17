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
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();
            PreLoad();
        }

        private void PreLoad()
        {
            RBService.IsChecked = true;
            firstloadcheck.IsChecked = true;
            DG_Category.ItemsSource = HimBDEntities.GetContext().ServiceCategory.ToList();
            Category_service.ItemsSource = HimBDEntities.GetContext().ServiceCategory.ToList();
            UpdLVSource();
        }

        private void UpdLVSource()
        {
            if (secondloadcheck.IsChecked == true)
                LV_services.ItemsSource = HimBDEntities.GetContext().Services.ToList();
            else
                LV_services.ItemsSource = HimBDEntities.GetContext().Services.ToList();
        }

        private void firstloadcheck_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void secondloadcheck_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void NewService_Click(object sender, RoutedEventArgs e)
        {
            if (Title_service.Text.Length < 2
                || Description_service.Text.Length < 2
                || Category_service.SelectedItem == null
                || Cost_service.Text.Length < 2)
            {
                MainVoid.ErrorMessage("Заполните все поля добавления услуги");
                return;
            }
            try
            {
                HimBDEntities.GetContext().Services.Add(new Services()
                {
                    title = Title_service.Text.Trim(),
                    description = Description_service.Text.Trim(),
                    category_id = ((ServiceCategory)Category_service.SelectedItem).id,
                    cost = double.Parse(Cost_service.Text.Trim()),
                });
                HimBDEntities.GetContext().SaveChanges();
                MainVoid.InformationMessage($"Услуга \"{Title_service.Text.Trim()}\" добавлена.");
                UpdLVSource();
                Title_service.Text = "";
                Description_service.Text = "";
                Cost_service.Text = "";
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.ToString());
            }
        }

        private void Cost_service_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            MainVoid.OnlyNumber((TextBox)sender, e);
        }

        private void AddAllHide()
        {
            RBS_Service .Visibility = Visibility.Collapsed;
            RBS_Preset  .Visibility = Visibility.Collapsed;
            RBS_Category.Visibility = Visibility.Collapsed;
        }

        private void RBS_Checked(object sender, RoutedEventArgs e)
        {
            switch (((RadioButton)sender).Name)
            {
                case "RBService":
                    AddAllHide();
                    RBS_Service.Visibility = Visibility.Visible;
                    break;
                case "RBPreset":
                    AddAllHide();
                    RBS_Preset.Visibility = Visibility.Visible;
                    break;
                case "RBCategory":
                    AddAllHide();
                    RBS_Category.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
