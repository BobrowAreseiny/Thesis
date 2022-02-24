using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Thesis.Data.Model;

namespace Thesis.Areas.UserArea.UserBasket
{
    /// <summary>
    /// Логика взаимодействия для AddToBasketWholesale.xaml
    /// </summary>
    public partial class AddToBasketWholesale : Window
    {
        private readonly string _pathToFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private readonly List<Basket> _basket;
        private readonly int _productId;
        private int _count;
    

        private class SelectedProduct
        {
            public string Size { get; set; }
            public string Name { get; set; }
            public BitmapFrame ProductImage { get; set; }
        }

        public AddToBasketWholesale(int productId, List<Basket> basket)
        {
            InitializeComponent();
            _count = 1;
            Data(productId);
            _basket = basket;
            _productId = productId;
        }

        private void Data(int productId)
        {
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                BitmapFrame image = null;
                Product dataProduct = _context.Product
                    .Where(x => x.Id == productId)
                    .FirstOrDefault();
                if (dataProduct != null)
                {
                    try
                    {
                        image = BitmapFrame.Create(new Uri(_pathToFile + @"\Data\Image\" + dataProduct.ProductImage));
                    }
                    catch
                    {
                        image = BitmapFrame.Create(new Uri(_pathToFile + @"\Data\ImageDefault\noimage.png"));
                    }

                    SelectedProduct selectedProduct = new SelectedProduct()
                    {
                        Name = dataProduct.Name,
                        ProductImage = image
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
            if (_count > -1 && _count < 100000)
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
            List<ProductSize> selectedProductSize;
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                selectedProductSize = _context.ProductSize
                    .Where(x => x.ProductId == _productId)
                    .Include(x => x.Product)
                    .ToList();
            }
            foreach (ProductSize item in selectedProductSize)
            {
                Basket addToBasket = _basket
                    .FirstOrDefault(x => x.Size.Id == item.Id);
                if (_count != 0)
                {
                    Basket basket = new Basket(item, _count);
                    if (_basket != null)
                    {
                        if (addToBasket != null)
                        {
                            _basket.Remove(addToBasket);
                            addToBasket.Count += basket.Count;
                            addToBasket.Cost = (double)addToBasket.Size.Product.Price * addToBasket.Count;
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
          

        private void SetCount(object sender, TextChangedEventArgs e)
        {
            if (countOfProducts.Text != string.Empty)
            {
                _count = Convert.ToInt32(countOfProducts.Text);
            }
            else
            {
                _count = 1;
            }
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsNumeric(e.Text);
        }
        private static bool IsNumeric(string str)
        {
            Regex reg = new Regex("[^0-9]");
            return reg.IsMatch(str);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
