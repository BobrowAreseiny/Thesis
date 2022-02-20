using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
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
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                List<Product> products = _context.Product.Take(4).ToList();

                if (products != null)
                {
                    foreach (Product item in products)
                    {
                        data.Add(new DataProduct(item));
                    }
                    if (data != null)
                    {
                        _productList.ItemsSource = data;
                        //_newProductsList.ItemsSource = data.Skip(data.Count - 3).ToList();
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
            //if (_account == null)
            //{
            //    new LoginWindow().Show();
            //    Close();
            //}
            //else
            //{
            //    if (productId != null)
            //    {
            //        if (productId != null)
            //        {
                        new ProductDescription(productId, _account, _basket).Show();
                        Close();
            //        }
            //    }
            //}
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AccountShow(object sender, RoutedEventArgs e)
        {
            //if (_account == null)
            //{
                //new LoginWindow().Show();
                //Close();
            //}
            //else
            //{
                new Profile(_account, _basket).Show();
                Close();
            //}
        }

        private void Help(object sender, RoutedEventArgs e)
        {

            //if (_account == null)
            //{
            //    new LoginWindow().Show();
            //    Close();
            //}
            //else
            //{
                new ProductsCatalog(_account, _basket).Show();
                Close();
            //}
        }

        private void Registration(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        //private void Grid_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    StackPanel cbDropdown = _newProductsList.ItemContainerStyle.Resources.FindName("dropdownInnerContent") as StackPanel;
        //   // cbDropdown.IsChecked = false;
        //   // DoubleAnimation heightAnimation = new DoubleAnimation(0, _openCloseDuration)
        //    //{
        //    //    Duration = new Duration(TimeSpan.FromMilliseconds(200))
        //    //};
        //    //dropdownContent.BeginAnimation(HeightProperty, heightAnimation);
        //}

        //private void Grid_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    //cbDropdown.IsChecked = true;
        //    //dropdownInnerContent.Measure(new Size(dropdownContent.MaxWidth, dropdownContent.MaxHeight));
        //    //DoubleAnimation heightAnimation = new DoubleAnimation(0, dropdownInnerContent.DesiredSize.Height, _openCloseDuration)
        //    //{
        //    //    Duration = new Duration(TimeSpan.FromMilliseconds(200))
        //    //};
        //    //dropdownContent.BeginAnimation(HeightProperty, heightAnimation);
        //}
    }
}
