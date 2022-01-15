using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Thesis.Data.Model;

namespace Thesis.Areas.UserArea.UserBasket
{
    /// <summary>
    /// Логика взаимодействия для AddToBasket.xaml
    /// </summary>
    public partial class AddToBasket : Window
    {
        private class SelectedProduct
        {
            public string Size { get; set; }
            public string Name { get; set; }
            public BitmapFrame ProductImage { get; set; }
        }

        private readonly string _pathToFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private readonly ProductSize _size;
        private readonly List<Basket> _basket;
        private int _count = 1;

        public AddToBasket(ProductSize size, List<Basket> basket)
        {
            InitializeComponent();
            Data(size);
            _size = size;
            _basket = basket;
        }

        private void Data(ProductSize productSize)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Product dataProduct = _context.Product.Where(x => x.Id == productSize.ProductId).FirstOrDefault();
                if (dataProduct != null)
                {
                    Uri image = new Uri(_pathToFile + @"\Data\Image\" + dataProduct.ProductImage);
                    SelectedProduct selectedProduct = new SelectedProduct()
                    {
                        Name = dataProduct.Name,
                        ProductImage = BitmapFrame.Create(image),
                        Size = productSize.Size
                    };
                    DataContext = selectedProduct;
                    countOfProducts.Text = _count.ToString();
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (_count > 0)
            {
                _count++;
                countOfProducts.Text = Convert.ToString(_count);
            }
        }

        private void Minus(object sender, RoutedEventArgs e)
        {
            if (_count > 1)
            {
                _count--;
                countOfProducts.Text = Convert.ToString(_count);
            }
        }

        private void AddBasket(object sender, RoutedEventArgs e)
        {
            Basket basket = new Basket(_size, _count);
            if (_basket != null)
            {
                Basket addToBasket = _basket.Where(x => x.Size.Id == basket.Size.Id).FirstOrDefault();
                if (addToBasket != null)
                {
                    _basket.Remove(addToBasket);
                    addToBasket.Count += basket.Count;
                    _basket.Add(addToBasket);
                }
                else
                {
                    _basket.Add(basket);
                }
            }
            else
            {
                _basket.Add(basket);
            }
            Close();
        }
    }
}

