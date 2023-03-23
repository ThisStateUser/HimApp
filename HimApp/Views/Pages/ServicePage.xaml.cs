using HimApp.BD;
using HimApp.Controllers;
using HimApp.Models;
using HimApp.Views.Pages.FnPages;
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
            RBLV_service.IsChecked = true;
            updCategory();
            UpdLVSource();
        }

        private void updCategory()
        {
            DG_Category.ItemsSource = HimBDEntities.GetContext().ServiceCategory.ToList();
            Category_service.ItemsSource = HimBDEntities.GetContext().ServiceCategory.ToList();
        }

        private void UpdLVSource()
        {
            if (RBLV_service.IsChecked == true)
                LV_services.ItemsSource = HimBDEntities.GetContext().Services.ToList();
            else
                LV_preset.ItemsSource = HimBDEntities.GetContext().PresetGroup.ToList();
        }

        private void SwapLV_Checked(object sender, RoutedEventArgs e)
        {
            switch (((RadioButton)sender).Name)
            {
                case "RBLV_service":
                    LV_services.Visibility = Visibility.Visible;
                    LV_preset.Visibility = Visibility.Collapsed;
                    UpdLVSource();
                    break;
                case "RBLV_preset":
                    LV_preset.Visibility = Visibility.Visible;
                    LV_services.Visibility = Visibility.Collapsed;
                    UpdLVSource();
                    break;
            }
            LV_services.ItemsSource = HimBDEntities.GetContext().Services.ToList();
        }

        private void NewService_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            if (Title_service.Text.Length < 1)
                list.Add("Вы не указали название услуги");
            if (Category_service.SelectedItem == null)
                list.Add("Вы не выбрали категорию услуги");
            if (Cost_service.Text.Length < 1)
                list.Add("Вы не указали стоимость услуги");
            if (list.Count > 0)
            {
                string result = "Вы допустили данные ошибки: \n";
                foreach (var item in list)
                    result += item + "\n";
                MainVoid.ErrorMessage(result);
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
            RBS_Service.Visibility = Visibility.Collapsed;
            RBS_Preset.Visibility = Visibility.Collapsed;
            RBS_Category.Visibility = Visibility.Collapsed;
            DG_SelectService.Visibility = Visibility.Collapsed;
            DG_Category.Visibility = Visibility.Collapsed;
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
                    DG_SelectService.Visibility = Visibility.Visible;
                    break;
                case "RBCategory":
                    AddAllHide();
                    RBS_Category.Visibility = Visibility.Visible;
                    DG_Category.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ShowHideLV(bool hide)
        {
            if (hide)
            {
                sel_zone.Visibility = Visibility.Hidden;
                LV_services_preset.Visibility = Visibility.Visible;
                LV_services.Visibility = Visibility.Collapsed;
                LV_preset.Visibility = Visibility.Collapsed;
                return;
            }
            sel_zone.Visibility = Visibility.Visible;
            LV_services_preset.Visibility = Visibility.Collapsed;
            LV_services.Visibility = Visibility.Visible;
            LV_preset.Visibility = Visibility.Visible;
        }

        private void NewPreset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int cost = int.Parse(Cost_preset.Text.Trim());
                presetGroup.cost = cost;
                List<ServicePreset> list = HimBDEntities.GetContext().ServicePreset.Where(x => x.preset_group_id == presetGroup.id).ToList();
                if (list.Count != 0)
                {
                    foreach (var item in servicePresets)
                    {
                        if (list.FirstOrDefault(s => s.service_id == item.service_id) != null)
                            list.Remove(item);
                        else
                            HimBDEntities.GetContext().ServicePreset.Add(item);
                    }
                    foreach (var item in list)
                    {
                        if (!servicePresets.Any(x => x.service_id == item.service_id))
                            HimBDEntities.GetContext().ServicePreset.Remove(item);
                    }
                    HimBDEntities.GetContext().SaveChanges();
                    ShowHideLV(false);
                    MainVoid.InformationMessage("Комплекс обновлен");
                    Title_preset.Text = "";
                    Cost_preset.Text = "";
                    NewPresetText.Text = "Создать комплекс";
                    return;
                }
                HimBDEntities.GetContext().ServicePreset.AddRange(servicePresets);
                HimBDEntities.GetContext().SaveChanges();
                Title_preset.Text = "";
                Cost_preset.Text = "";
                ShowHideLV(false);
                MainVoid.InformationMessage($"Комплекс \"{presetGroup.title}\" создан");
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
                MainVoid.InformationMessage($"Категория \"{category_name_add.Text.Trim()}\" создана");
                updCategory();
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }

        private void ShowService_Click(object sender, RoutedEventArgs e)
        {
            List<ServicesModel> list = new List<ServicesModel>();
            ShowHideLV(true);
            if (Title_preset.Text.Length < 1)
            {
                MainVoid.ErrorMessage("Название не может быть пустым");
                return;
            }
            PresetGroup preset = HimBDEntities.GetContext().PresetGroup.FirstOrDefault(x => x.title == Title_preset.Text.ToLower().Trim());
            if (preset != null)
            {
                MessageBoxResult result = MessageBox.Show($"Комплекс услуг \"{Title_preset.Text.Trim()}\" уже существует. Изменить состав?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                    return;
                NewPresetText.Text = "Обновить комплекс";
                Cost_preset.Text = preset.cost.ToString();
                presetGroup = preset;
                List<ServicePreset> lpres = HimBDEntities.GetContext().ServicePreset.Where(x => x.preset_group_id == preset.id).ToList();
                servicePresets.AddRange(lpres);
                List<int> selectedService = new List<int>();
                foreach (Services item in lpres.Select(x => x.Services).ToList())
                {
                    list.Add(new ServicesModel()
                    {
                        isSelected = true,
                        service = item,
                    });
                    selectedService.Add(item.id);
                }
                foreach (var item in HimBDEntities.GetContext().Services.ToList())
                {
                    if (!selectedService.Any(x => x == item.id))
                        list.Add(new ServicesModel() { isSelected = false, service = item });
                }
                LV_services_preset.ItemsSource = list;
                DG_SelectService.ItemsSource = servicePresets.ToList();
                return;
            }
            if(Cost_preset.Text.Length < 1)
            {
                MainVoid.ErrorMessage("Укажите стоимость комплекса");
                return;
            }
            LV_services_preset.ItemsSource = HimBDEntities.GetContext().Services.ToList().ConvertAll(x => new ServicesModel { isSelected = false, service = x });
            try
            {
                presetGroup = HimBDEntities.GetContext().PresetGroup.Add(new PresetGroup
                {
                    title = Title_preset.Text.Trim(),
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
            DG_SelectService.ItemsSource = null;
            DG_SelectService.ItemsSource = servicePresets.ToList();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(((CheckBox)sender).Tag.ToString());
            servicePresets.Add(new ServicePreset
            {
                service_id = id,
                Services = HimBDEntities.GetContext().Services.FirstOrDefault(x => x.id == id),
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

        private void RemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(((Button)sender).Tag.ToString());
            if (HimBDEntities.GetContext().Services.Any(x => x.category_id == id))
            {
                MainVoid.ErrorMessage("Невозможно удалить категорию из за ее принадлежности к одному или нескольким сервисам");
                return;
            }
            ServiceCategory serviceCategory = HimBDEntities.GetContext().ServiceCategory.FirstOrDefault(x => x.id == id);
            MessageBoxResult result = MessageBox.Show($"Удалить запись \"{serviceCategory.category_name}\"?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;
            HimBDEntities.GetContext().ServiceCategory.Remove(serviceCategory);
            HimBDEntities.GetContext().SaveChanges();
            updCategory();
        }
    }
}
