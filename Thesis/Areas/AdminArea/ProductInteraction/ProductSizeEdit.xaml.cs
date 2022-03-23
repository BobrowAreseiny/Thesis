using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.ProductInteraction
{
    /// <summary>
    /// Логика взаимодействия для ProductSizeEdit.xaml
    /// </summary>
    public partial class ProductSizeEdit : Page
    { 
        private readonly ProductSize _size = new ProductSize();

        public ProductSizeEdit()
        {
            InitializeComponent();
        }

        public ProductSizeEdit(ProductSize size)
        {
            InitializeComponent();
            _size = size;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
                {
                    int? productId = (_product.SelectedItem as Product).Id;
                    _size.Product = _context.Product
                                .Where(x => x.Id == productId)
                                .FirstOrDefault();

                    if (_size.Id == 0)
                    {
                        _context.ProductSize.Add(_size);
                    }
                    else
                    {
                        if (productId != null)
                        {
                            ProductSize data = _context.ProductSize
                                .Where(f => f.Id == _size.Id)
                                .Include(x => x.Product)
                                .FirstOrDefault();

                            if (data != null)
                            {
                                data.CountSize = _size.CountSize;
                                data.Size = _size.Size;
                                data.Product = _size.Product;
                            }
                            _context.Entry(data).State = EntityState.Modified;

                        }
                    }
                    _context.SaveChanges();
                    NavigationService.GoBack();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка!");
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Data(ProductSize size)
        {
            DataContext = size;
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());

                _product.ItemsSource = _context.Product.ToList();
                if (size != null)
                {
                    _product.SelectedItem = _context.Product
                        .Where(x => x.Id == _size.ProductId)
                        .FirstOrDefault();
                }
            }
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(_size);
            }
        }
    }
}
