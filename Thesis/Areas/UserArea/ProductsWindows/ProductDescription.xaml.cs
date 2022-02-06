using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Thesis.Areas.UserArea.UserBasket;
using Thesis.Data.Model;

namespace Thesis.Areas.UserArea.ProductsWindows
{
    /// <summary>
    /// Логика взаимодействия для ProductDescription.xaml
    /// </summary>
    public partial class ProductDescription : Window
    {
        private readonly string _pathToFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private readonly List<Basket> _basket;
        private readonly ApplicationUser _account;
        private readonly int? _productId;

        public ProductDescription(int? productId, ApplicationUser user, List<Basket> basket)
        {
            InitializeComponent();
            Data(productId);
            _basket = basket;
            _account = user;
            _productId = productId;
        }

        private void AddToBasketWholesale(object sender, RoutedEventArgs e)
        {
            if (_productId != null)
            {
                new AddToBasketWholesale((int)_productId, _basket).ShowDialog();
            }
        }

        private void Basket(object sender, RoutedEventArgs e)
        {
            new Creel(_account, _basket).Show();
            Close();
        }

        private void ProductCatalog(object sender, RoutedEventArgs e)
        {
            new ProductsCatalog(_account,_basket).Show();
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
                try
                {
                    productImage.Source = BitmapFrame.Create(new Uri(_pathToFile + @"\Data\Image\" + product.ProductImage));
                }
                catch
                {
                    productImage.Source = BitmapFrame.Create(new Uri(_pathToFile + @"\Data\ImageDefault\noimage.png"));
                }
            }
        }

        private void AddToBasket(object sender, SelectionChangedEventArgs e)
        {
            if (_productSize.SelectedItem is ProductSize selectedSize)
            {
                Basket selecteditem = _basket
                  .Where(x => x.Size.Id == selectedSize.Id)
                  .FirstOrDefault();
                if (selecteditem != null)
                {
                    new AddToBasket(selecteditem, _basket).ShowDialog();
                }
                else
                {
                    new AddToBasket(selectedSize, _basket).ShowDialog();
                }
            };
        }

        private void Description(object sender, RoutedEventArgs e)
        {

        }
    }
}
