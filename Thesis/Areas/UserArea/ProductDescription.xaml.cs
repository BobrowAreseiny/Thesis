using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thesis.Areas.UserArea.UserBasket;
using Thesis.Data.Model;

namespace Thesis.Areas.UserArea
{
    /// <summary>
    /// Логика взаимодействия для ProductDescription.xaml
    /// </summary>
    public partial class ProductDescription : Window
    {
        private readonly string _pathToFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private readonly List<Basket> _basket;
        private readonly ApplicationUser _account;

        public ProductDescription(int? productId, ApplicationUser user, List<Basket> basket)
        {
            InitializeComponent();
            Data(productId);
            _basket = basket;
            _account = user;
        }

        private void Buy(object sender, RoutedEventArgs e)
        {

        }

        private void AddToBasket(object sender, RoutedEventArgs e)
        {
            if (_productSize.SelectedItem is ProductSize selectedSize)
            {
                new AddToBasket(selectedSize, _basket).Show();
            };
        }

        private void Basket(object sender, RoutedEventArgs e)
        {
            new Creel(_account, _basket).Show();
            Close();
        }

        private void ProductCatalog(object sender, RoutedEventArgs e)
        {
            new MainWindow(_account,_basket).Show();
            Close();
        }

        private void Data(int? productId)
        {
            Product product = null;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _productSize.ItemsSource = _context.ProductSize
                    .Where(x => x.ProductId == productId)
                    .ToList();
                product = _context.Product
                    .Where(x => x.Id == productId)
                    .FirstOrDefault();
            }
            if (product != null)
            {
                DataContext = product;
                Uri image = new Uri(_pathToFile + @"\Data\Image\" + product.ProductImage);
                if (image != null)
                {
                    productImage.Source = BitmapFrame.Create(image);
                }
            }
        }
    }
}
