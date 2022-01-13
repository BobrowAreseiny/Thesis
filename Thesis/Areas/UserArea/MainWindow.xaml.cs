using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis.Areas.ManagerArea;
using Thesis.Data.Model;

namespace Thesis.Areas.UserArea
{
    public partial class MainWindow : Window
    {
        private readonly string pathToFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private readonly List<Basket> _basket = new List<Basket>();
        private readonly ApplicationUser _account = new ApplicationUser();

        public MainWindow()
        {
            InitializeComponent();
            Data();
        }

        public MainWindow(ApplicationUser user, List<Basket> basket)
        {
            InitializeComponent();
            Data();
            _basket = basket;
            _account = user;
        }

        public void Data()
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                List<Product> products = _context.Product.Take(20).ToList();
                List<Product> newProducts = _context.Product.OrderBy(x => x.CountOnStorage).Take(3).ToList();
                if (products != null)
                {
                    foreach (Product item in products)
                    {
                        if (item.ProductImage != string.Empty)
                        {
                            item.ProductImage = pathToFile + @"\Data\Image\" + item.ProductImage;
                        }
                    }
                    _productList.ItemsSource = products;
                }
                if (newProducts != null)
                {
                    foreach (Product item in newProducts)
                    {
                        if (item.ProductImage != string.Empty)
                        {
                            item.ProductImage = pathToFile + @"\Data\Image\" + item.ProductImage;
                        }
                    }
                    _newProductsList.ItemsSource = newProducts;
                }
            }
        }
   
        private void AdminArea(object sender, RoutedEventArgs e)
        {
            MainManagerWindow window = new MainManagerWindow();
            window.Show();
        }

        private void DataSelector(object sender, MouseButtonEventArgs e)
        {
            Data();
        }

        private void AddToBasket(object sender, EventArgs e)
        {

        }

        private void ProductDescription(object sender, RoutedEventArgs e)
        {
            int? productId = (sender as Button).Content as int?;
            if (productId != null)
            {
                if (productId != null)
                {
                    new ProductDescription(productId, _account, _basket).Show();
                    Close();
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void AccountShow(object sender, RoutedEventArgs e)
        {
            new LoginWindow(_basket).Show();
            Close();
        }

        private void ClickBackAndForward(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Back(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Next(object sender, RoutedEventArgs e)
        {

        }

        private void Back_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Next_GotFocus(object sender, RoutedEventArgs e)
        {

        }
     
        private void AddToLove(object sender, RoutedEventArgs e)
        {

        }

        public void LoveNew()
        {

        }

        
        private void DockPanel_MouseEnter(object sender, RoutedEventArgs e)
        {

        }



       
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void Help_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
