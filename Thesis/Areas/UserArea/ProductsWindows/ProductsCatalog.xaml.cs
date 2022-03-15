using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Areas.UserArea.RegistrationAndAuthorization;
using Thesis.Areas.UserArea.UserBasket;
using Thesis.Data.Model;

namespace Thesis.Areas.UserArea.ProductsWindows
{
    /// <summary>
    /// Логика взаимодействия для ProductsCatalog.xaml
    /// </summary>
    public partial class ProductsCatalog : Window
    {
        private readonly List<Basket> _basket = new List<Basket>();
        private List<DataProduct> _data = new List<DataProduct>();
        private List<DataProduct> _staticData = new List<DataProduct>();
        private readonly ApplicationUser _account = null;
        private int productsCount = 12;

        public ProductsCatalog(ApplicationUser user)
        {
            InitializeComponent();
            Data();
            _account = user;
        }

        public ProductsCatalog(ApplicationUser user, List<Basket> basket)
        {
            InitializeComponent();
            Data();
            if (basket != null)
            {
                _basket = basket;
            }
            _account = user;
        }

        private void Data()
        {
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                //if (_context.Product.Count() == _staticData.Count()) 
                //{
                _data = new List<DataProduct>();
                foreach (Product item in _context.Product.Take(productsCount).ToList())
                {
                    _data.Add(new DataProduct(item));
                }
                if (_data != null)
                {
                    _productList.ItemsSource = null;
                    _productList.ItemsSource = _data;
                    _staticData = _data;
                }
                _material.ItemsSource = _context.Material.ToList();
                _sex.ItemsSource = _context.Sex.ToList();
                _season.ItemsSource = _context.Season.ToList();
                // }
            }
        }

        private void ProductDescription(object sender, SelectionChangedEventArgs e)
        {
            if (_productList.SelectedItem is DataProduct data)
            {
                new ProductDescription(data.Id, _account, _basket).Show();
                Close();
            }
        }

        private void Basket(object sender, RoutedEventArgs e)
        {
            if (_account != null)
            {
                new Creel(_account, _basket).Show();
                Close();
            }
        }

        private void NameSort(object sender, RoutedEventArgs e)
        {
            _data = _data.OrderBy(x => x.Name).ToList();
            DropComboItems();
            DataSort(_data);
            _name.IsChecked = true;
        }

        private void DateSort(object sender, RoutedEventArgs e)
        {
            _data = _data.OrderBy(x => x.DateOfCreation).ToList();
            DropComboItems();
            DataSort(_data);
            _date.IsChecked = false;
        }

        private void CostSort(object sender, RoutedEventArgs e)
        {
            _data = _data.OrderByDescending(x => x.Price).ToList();
            DropComboItems();
            DataSort(_data);
            _cost.IsChecked = false;
        }

        private void ChangeValueComboBox(object sender, SelectionChangedEventArgs e)
        {
            if (_material.SelectedItem == null && _sex.SelectedItem == null && _season.SelectedItem == null)
            {
                DataSort(_staticData);
            }
            else
            {
                OneSort();
                TwoSort();
                ThreeSort();
                DataSort(_data);
            }
        }

        private void DropSettings(object sender, RoutedEventArgs e)
        {
            DropComboItems();
            DataSort(_staticData);
        }

        private void Sherch(object sender, TextChangedEventArgs e)
        {
            List<DataProduct> data = _data
                .Where(x => x.Name.ToLower().Contains(textSearch.Text.ToLower()))
                .ToList();
            if (data != null)
            {
                DataSort(data);
            }
        }

        private void AddProducts(object sender, RoutedEventArgs e)
        {
            productsCount += productsCount + 10;
            DropComboItems();
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _data = new List<DataProduct>();
                foreach (Product item in _context.Product.Take(productsCount).ToList())
                {
                    _data.Add(new DataProduct(item));
                }
                if (_data != null)
                {
                    _productList.ItemsSource = null;
                    _productList.ItemsSource = _data;
                    _staticData = _data;
                }
            }
        }

        private void DataSort(List<DataProduct> data)
        {
            _productList.ItemsSource = null;
            _productList.ItemsSource = data;
        }

        private void OneSort()
        {
            if (_material.SelectedItem != null)
            {
                _data = _staticData.Where(x => x.MaterialId == (_material.SelectedItem as Material).Id).ToList();
            }
            if (_sex.SelectedItem != null)
            {
                _data = _staticData.Where(x => x.SexId == (_sex.SelectedItem as Sex).Id).ToList();
            }
            if (_season.SelectedItem != null)
            {
                _data = _staticData.Where(x => x.SeasonId == (_season.SelectedItem as Season).Id).ToList();
            }
        }

        private void TwoSort()
        {
            if (_material.SelectedItem != null && _sex.SelectedItem != null)
            {
                _data = _staticData.Where(x => x.MaterialId == (_material.SelectedItem as Material).Id
                    && x.SexId == (_sex.SelectedItem as Sex).Id).ToList();
            }
            if (_season.SelectedItem != null && _sex.SelectedItem != null)
            {
                _data = _staticData.Where(x => x.SeasonId == (_season.SelectedItem as Season).Id
                    && x.SexId == (_sex.SelectedItem as Sex).Id).ToList();
            }
            if (_material.SelectedItem != null && _season.SelectedItem != null)
            {
                _data = _staticData.Where(x => x.MaterialId == (_material.SelectedItem as Material).Id
                    && x.SeasonId == (_season.SelectedItem as Season).Id).ToList();
            }
        }

        private void ThreeSort()
        {
            if (_material.SelectedItem != null && _sex.SelectedItem != null && _season.SelectedItem != null)
            {
                _data = _staticData.Where(x => x.MaterialId == (_material.SelectedItem as Material).Id
                    && x.SexId == (_sex.SelectedItem as Sex).Id
                    && x.SeasonId == (_season.SelectedItem as Season).Id).ToList();
            }
            DropComboItems();
        }

        private void DropComboItems()
        {
            _name.IsChecked = false;
            _date.IsChecked = false;
            _cost.IsChecked = false;
        }

        private void Help(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathToFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Help.chm";
                Process.Start(pathToFile);
            }
            catch
            {
                MessageBox.Show("Неизвестная ошибка");
            }
        }

        private void Profile(object sender, RoutedEventArgs e)
        {
            if (_account != null)
            {
                new Profile(_account, _basket).Show();
                Close();
            }
        }
    }
}
