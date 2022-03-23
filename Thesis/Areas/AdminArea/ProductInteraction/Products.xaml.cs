using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.ProductInteraction
{
    /// <summary>
    /// Логика взаимодействия для Products.xaml
    /// </summary>
    public partial class Products : Page
    { 
        private readonly int? productId = null;
        private List<Product> _productsdata = new List<Product>();

        public Products()
        {
            InitializeComponent();
        }

        public Products(int Id)
        {
            InitializeComponent();
            productId = Id;
        }

        private void Data(int? productId)
        {
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _productsdata = productId != null
                    ? _context.Product.Where(x => x.Id == productId).ToList()
                    : _context.Product.ToList();
                if (_productsdata != null)
                {
                    _products.ItemsSource = null;
                    _products.ItemsSource = _productsdata;
                }
            }
        }

        private void ProductDescription(object sender, SelectionChangedEventArgs e)
        {
            if (_products.SelectedItem is Product product)
            {
                _count.IsChecked = false;
                _name.IsChecked = false;
                _cost.IsChecked = false;
                NavigationService.Navigate(new ProductDescription(product.Id));
            }
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(productId);
            }
        }

        private void DeleteMaterial(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Точно удалить данные?", "Внимание",
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if ((sender as Button) != null)
                {
                    using (ApplicationDbContent _context = new ApplicationDbContent())
                    {
                        int productId = (int)((Button)sender).Content;
                        Product selectedData = _context.Product
                            .Where(x => x.Id == productId)
                            .FirstOrDefault();
                        _context.Product.Remove(selectedData);
                        _context.SaveChanges();
                    }
                    Data(productId);
                }
            }
        }

        private void AddProduct(object sender, RoutedEventArgs e)
        {
            _count.IsChecked = false;
            _name.IsChecked = false;
            _cost.IsChecked = false;
            NavigationService.Navigate(new ProductEdit());
        }

        private void CountSort(object sender, RoutedEventArgs e)
        {
            List<Product> data = _productsdata;
            if (_count.IsChecked == true)
            {
                _name.IsChecked = false;
                _cost.IsChecked = false;
                DataSort(data.OrderByDescending(x => x.CountOnStorage).ToList());
            }
            else
            {
                _products.ItemsSource = null;
                _products.ItemsSource = _productsdata;
            }
        }

        private void CostSort(object sender, RoutedEventArgs e)
        {
            List<Product> data = _productsdata;
            if (_cost.IsChecked == true)
            {
                _name.IsChecked = false;
                _count.IsChecked = false;
                DataSort(data = data.OrderByDescending(x => x.Price).ToList());
            }
            else
            {
                _products.ItemsSource = null;
                _products.ItemsSource = _productsdata;
            }
        }

        private void NameSort(object sender, RoutedEventArgs e)
        {
            List<Product> data = _productsdata;
            if (_name.IsChecked == true)
            {
                _count.IsChecked = false;
                _cost.IsChecked = false;
                DataSort(data.OrderBy(x => x.Name).ToList());
            }           
            else
            {
                _products.ItemsSource = null;
                _products.ItemsSource = _productsdata;
            }
        }        

        private void Sherch(object sender, TextChangedEventArgs e)
        {
            _count.IsChecked = false;
            _name.IsChecked = false;
            _cost.IsChecked = false;
            List<Product> data = _productsdata
                .Where(x => x.Name.ToLower().Contains(textSearch.Text.ToLower()))
                .ToList();
            if (data != null)
            {
                DataSort(data);
            }
        }

        private void DataSort(List<Product> data)
        {
            _products.ItemsSource = null;
            _products.ItemsSource = data;
        }
    }
}
