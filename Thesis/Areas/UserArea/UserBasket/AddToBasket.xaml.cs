using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
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
        private int _count;
        private bool isProductChaange = false;

        public AddToBasket(ProductSize size, List<Basket> basket)
        {
            InitializeComponent();
            _count = 1;
            Data(size);
            _size = size;
            _basket = basket;
        }
        public AddToBasket(Basket product, List<Basket> basket)
        {
            InitializeComponent();
            _count = product.Count; 
            _size = product.Size;
            _basket = basket;
            isProductChaange = true;
            Data(_size);
        }

        private void Data(ProductSize productSize)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                BitmapFrame image = null;
                Product dataProduct = _context.Product.Where(x => x.Id == productSize.ProductId).FirstOrDefault();
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
                        ProductImage = image,
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
            if (_count != 0)
            {
                Basket basket = new Basket(_size, _count);
                if (_basket != null)
                {
                    Basket addToBasket = _basket.Where(x => x.Size.Id == basket.Size.Id).FirstOrDefault();
                    if (addToBasket != null)
                    {
                        _basket.Remove(addToBasket);

                        if (isProductChaange != true)
                        {
                            addToBasket.Count += basket.Count;
                        }
                        else
                        {
                            addToBasket.Count = basket.Count;
                        }
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

        private void SetCount(object sender, System.Windows.Controls.TextChangedEventArgs e)
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
    }
}

