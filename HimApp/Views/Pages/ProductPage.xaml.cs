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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
            WConnect.MainWindowMethod.PageTitle.Text = "Склад/Товары";
            PreLoad();
        }

        private void PreLoad()
        {
            updLV();
            updDGCategory();
            unit.ItemsSource = HimBDEntities.GetContext().Unit.ToList();
        }

        private void updDGCategory()
        {
            DG_Category.ItemsSource = HimBDEntities.GetContext().ProductCategory.ToList();
            category.ItemsSource = HimBDEntities.GetContext().ProductCategory.ToList();
        }

        private bool valid()
        {
            List<string> errorlist = new List<string>();
            errorlist.Clear();
            if (string.IsNullOrEmpty(article_product.Text))
                errorlist.Add("Вы не указали артикул товара");
            if (HimBDEntities.GetContext().Product.Any(x => x.article.ToLower().Trim() == article_product.Text.ToLower().Trim()))
                errorlist.Add("Артикул занят");
            if (string.IsNullOrEmpty(name_product.Text))
                errorlist.Add("Вы не указали название товара");
            if (string.IsNullOrEmpty(count_product.Text))
                errorlist.Add("Вы не указали колличество товара");
            if (string.IsNullOrEmpty(price.Text))
                errorlist.Add("Вы не указали цену товара");
            if (string.IsNullOrEmpty(unit.Text))
                errorlist.Add("Вы не выбрали еденицу измерения товара");
            if (string.IsNullOrEmpty(category.Text))
                errorlist.Add("Вы не выбрали категорию товара");
            if (errorlist.Count > 0)
            {
                string result = "Вы допустили данные ошибки: \n";
                foreach (var item in errorlist)
                    result += item + "\n";
                MainVoid.ErrorMessage(result);
                return false;
            }
            return true;
        }

        private void NewProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!valid())
                return;
            try
            {
                HimBDEntities.GetContext().Product.Add(new Product()
                {
                    article = article_product.Text.Trim().ToUpper(),
                    name = name_product.Text.Trim(),
                    brand = brand_product.Text.Trim(),
                    count = int.Parse(count_product.Text.Trim()),
                    price = double.Parse(price.Text.Trim()),
                    unit_id = ((Unit)unit.SelectedItem).id,
                    category_id = ((ProductCategory)category.SelectedItem).id,
                });
                HimBDEntities.GetContext().SaveChanges();
                MainVoid.InformationMessage($"Товар {name_product.Text.Trim()} добавлен.");
                updLV();
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
                return;
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            updLV(((TextBox)sender).Text.Trim().ToLower());
        }

        private void updLV(string search = "")
        {
            if (search == "")
            {
                LV_Product.ItemsSource = HimBDEntities.GetContext().Product.ToList();
                return;
            }
            LV_Product.ItemsSource = HimBDEntities.GetContext().Product.Where(x => x.article.Contains(search.Trim().ToLower()) ||
                                                                                   x.name.Contains(search.Trim().ToLower()) ||
                                                                                   x.brand.Contains(search.Trim().ToLower())
                                                                             ).ToList();
        }

        private void NewCategory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(category_product.Text))
            {
                MainVoid.ErrorMessage("Укажите название категории");
                return;
            }
            try
            {
                HimBDEntities.GetContext().ProductCategory.Add(new ProductCategory()
                {
                    category_name = category_product.Text.Trim(),
                });
                HimBDEntities.GetContext().SaveChanges();
                MainVoid.InformationMessage($"Категория {name_product.Text.Trim()} добавлена.");
                updDGCategory();
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
                return;
            }
        }

        private void OnlyNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            MainVoid.OnlyNumber((TextBox)sender, e);
        }

        private void RemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(((Button)sender).Tag.ToString());
            if (HimBDEntities.GetContext().Product.Any(x => x.category_id == id))
            {
                MainVoid.ErrorMessage("Невозможно удалить категорию из за ее принадлежности к одному или нескольким товарам");
                return;
            }
            ProductCategory productCategory = HimBDEntities.GetContext().ProductCategory.FirstOrDefault(x => x.id == id);
            MessageBoxResult result = MessageBox.Show($"Удалить запись \"{productCategory.category_name}\"?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;
            try
            {
                HimBDEntities.GetContext().ProductCategory.Remove(productCategory);
                HimBDEntities.GetContext().SaveChanges();
                updDGCategory();
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            StackPanel parrent = (StackPanel)((Button)sender).Parent;
            int id = int.Parse(parrent.Tag.ToString());
            TextBlock text = ((TextBlock)((StackPanel)parrent.Children[1]).Children[0]);
            int count = int.Parse(text.Text);
            if (count >= 1)
            {
                count--;
                text.Text = count.ToString();
            }
            try
            {
                HimBDEntities.GetContext().Product.FirstOrDefault(x => x.id == id).count = count;
                HimBDEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            StackPanel parrent = (StackPanel)((Button)sender).Parent;
            int id = int.Parse(parrent.Tag.ToString());
            TextBlock text = ((TextBlock)((StackPanel)parrent.Children[1]).Children[0]);
            int count = int.Parse(text.Text);
            if (count < 1000)
            {
                count++;
                text.Text = count.ToString();
            }
            try
            {
                HimBDEntities.GetContext().Product.FirstOrDefault(x => x.id == id).count = count;
                HimBDEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
        }

        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(((Button)sender).Tag.ToString());
            Product product = HimBDEntities.GetContext().Product.FirstOrDefault(x => x.id == id);
            MessageBoxResult result = MessageBox.Show($"Удалить запись \"{product.name}\"?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;
            try
            {
                HimBDEntities.GetContext().Product.Remove(product);
                HimBDEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MainVoid.FatalErrorMessage(ex.Message);
            }
            updLV();
        }

        private void NextTxt_KeyDown(object sender, KeyEventArgs e)
        {
            switch (((TextBox)sender).Name)
            {
                case "article_product":
                    if (e.Key == Key.Enter)
                        name_product.Focus();
                    break;
                case "name_product":
                    if (e.Key == Key.Enter)
                        brand_product.Focus();
                    break;
                case "brand_product":
                    if (e.Key == Key.Enter)
                        count_product.Focus();
                    break;
                case "count_product":
                    if (e.Key == Key.Enter)
                        price.Focus();
                    break;
            }
        }
    }
}
