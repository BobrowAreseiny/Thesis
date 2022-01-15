using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis.Areas.ManagerArea;
using Thesis.Areas.UserArea.RegistrationAndAuthorization;
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
            List<DataProduct> data = new List<DataProduct>();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                List<Product> products = _context.Product.Take(20).ToList();
                if (products != null)
                {
                    foreach (Product item in products)
                    {
                        data.Add(new DataProduct(item));
                    }
                    if (data != null)
                    {
                        _productList.ItemsSource = data;
                        data.OrderByDescending(x => x.Id).Take(3).ToList();
                        _newProductsList.ItemsSource = data;
                    }
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

        private void Help(object sender, RoutedEventArgs e)
        {

        }
    }
}
