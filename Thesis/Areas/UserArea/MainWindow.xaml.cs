using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis.Areas.ManagerArea;
using Thesis.Areas.MarketingArea;
using Thesis.Areas.UserArea.ProductsWindows;
using Thesis.Areas.UserArea.RegistrationAndAuthorization;
using Thesis.Data.Model;

namespace Thesis.Areas.UserArea
{
    public partial class MainWindow : Window
    {
        private readonly List<Basket> _basket = new List<Basket>();
        private readonly ApplicationUser _account = null;

        public MainWindow()
        {
            InitializeComponent();
            Data();
        }

        public MainWindow(ApplicationUser user)
        {
            InitializeComponent();
            Data();
            _account = user;
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
            List<Product> loveProducts = new List<Product>();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                List<Product> products = _context.Product.Take(20).ToList();
                int productCount = products.Count();
                if (productCount > 3)
                {
                    loveProducts = _context.Product
                        .OrderByDescending(x => x.CountOnStorage)
                        .Skip(productCount - 3).Take(3)
                        .ToList();
                    if (loveProducts != null)
                    {
                        _newProductsList.ItemsSource = loveProducts;
                    }
                }

                if (products != null)
                {
                    foreach (Product item in products)
                    {
                        data.Add(new DataProduct(item));
                    }
                    if (data != null)
                    {
                        _productList.ItemsSource = data;
                    }
                }
            }
        }

        private void AdminArea(object sender, RoutedEventArgs e)
        {
            MainManagerWindow window = new MainManagerWindow();
            window.Show();
        }

        private void ManagerArea(object sender, RoutedEventArgs e)
        {
            MainMarketingWindow window = new MainMarketingWindow();
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
            if (_account == null)
            {
                new LoginWindow().Show();
                Close();
            }
            else
            {
                if (productId != null)
                {
                    if (productId != null)
                    {
                        new ProductDescription(productId, _account, _basket).Show();
                        Close();
                    }
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void AccountShow(object sender, RoutedEventArgs e)
        {
            if (_account == null)
            {
                new LoginWindow().Show();
                Close();
            }
            else
            {
                new Profile(_account, _basket).Show();
                Close();
            }
        }

        private void Help(object sender, RoutedEventArgs e)
        {

            if (_account == null)
            {
                new LoginWindow().Show();
                Close();
            }
            else
            {
                new ProductsCatalog(_account, _basket).Show();
                Close();
            }
        }

        private void Registration(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }
    }
}
