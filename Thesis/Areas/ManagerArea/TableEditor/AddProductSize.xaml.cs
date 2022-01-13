using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Thesis.Areas.ManagerArea.TableDescription;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.TableEditor
{
    /// <summary>
    /// Логика взаимодействия для AddProductSize.xaml
    /// </summary>
    public partial class AddProductSize : Window
    {
        private readonly ProductSize _size = new ProductSize();

        public AddProductSize()
        {
            InitializeComponent();
            DataContext = _size;
        }

        public AddProductSize(ProductSize _selectedSize)
        {
            InitializeComponent();

            try
            {
                if (_selectedSize != null)
                {
                    _size = _selectedSize;
                }
                DataContext = _size;
                Data(_size);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            int productId = 0;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (_size.Id == 0)
                {
                    _context.ProductSize.Add(_size);
                }
                else
                {
                    ProductSize dataSize = _context.ProductSize
                        .Where(f => f.Id == _size.Id)
                        .Include(x => x.Product)
                        .FirstOrDefault();

                    _size.Product = _context.Product
                        .Where(x => x.Id == ((Product)_product.SelectedItem).Id)
                        .FirstOrDefault();

                    if (dataSize != null)
                    {
                        dataSize.CountSize = _size.CountSize;
                        dataSize.Size = _size.Size;
                        dataSize.Product = _size.Product;
                    }
                    _context.Entry(dataSize).State = EntityState.Modified;
                    _context.SaveChanges();
                    if (dataSize.ProductId != null)
                    {
                        productId = (int)dataSize.ProductId;
                        new ProductDescription(productId).Show();
                        Close();
                    }
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            if (_size.ProductId != null)
            {
                new ProductDescription((int)_size.ProductId).Show();
                Close();
            }
        }

        private void Data(ProductSize size)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _product.ItemsSource = _context.Product.ToList();

                _product.SelectedItem = _context.Product
                    .Where(x => x.Id == size.ProductId)
                    .FirstOrDefault();
            }
        }

        private void Pull(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
