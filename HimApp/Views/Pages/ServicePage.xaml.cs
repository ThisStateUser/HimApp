﻿using HimApp.BD;
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
        public static PresetGroup presetGroup;
        public static List<ServicePreset> servicePresets = new List<ServicePreset>();

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
                if (HimBDEntities.GetContext().Services.FirstOrDefault(x => x.title == Title_service.Text.ToLower().Trim()) != null)
                {
                    MainVoid.ErrorMessage($"Услуга \"{Title_service.Text}\" уже существует.");
                    return;
                }

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

        private void NewPreset_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }

        private void NewCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HimBDEntities.GetContext().ServiceCategory.Add(new ServiceCategory
                {
                    category_name = category_name_add.Text.Trim(),
                });
                HimBDEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }

        private void ShowService_Click(object sender, RoutedEventArgs e)
        {
            LV_services_preset.ItemsSource = HimBDEntities.GetContext().Services.ToList();
            sel_zone.Visibility = Visibility.Hidden;
            SV_services_preset.Visibility = Visibility.Visible;
            int cost = int.Parse(Cost_preset.Text.Trim());
            if (HimBDEntities.GetContext().PresetGroup.FirstOrDefault(x => x.title == Title_preset.Text.ToLower().Trim()) != null)
            {
                MessageBoxResult result = MessageBox.Show($"Комплекс услуг \"{Title_preset.Text.Trim()}\" уже существует. Изменить состав?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                    return;
                return;
            }

            try
            {
                presetGroup = HimBDEntities.GetContext().PresetGroup.Add(new PresetGroup
                {
                    title = Title_preset.Text.Trim(),
                    cost = cost,
                });
                HimBDEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }

        private void updDGservice()
        {
            DG_SelectService.ItemsSource = servicePresets.ToList();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(((CheckBox)sender).Tag.ToString());
            servicePresets.Add(new ServicePreset
            {
                service_id = id,
                preset_group_id = presetGroup.id,
            });
            updDGservice();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(((CheckBox)sender).Tag.ToString());
            servicePresets.Remove(servicePresets.FirstOrDefault(x => x.service_id == id));
            updDGservice();
        }
    }
}
