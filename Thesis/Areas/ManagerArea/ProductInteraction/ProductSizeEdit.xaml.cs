using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.ProductInteraction
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
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (_size.Id == 0)
                {
                    _context.ProductSize.Add(_size);
                }
                else
                {
                    int? productId = (_product.SelectedItem as Product).Id;
                    if (productId != null)
                    {
                        _size.Product = _context.Product
                            .Where(x => x.Id == productId)
                            .FirstOrDefault();

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
                        _context.SaveChanges();
                        NavigationService.GoBack();
                    }
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Data(ProductSize size)
        {
            DataContext = size;
            using (ApplicationDbContext _context = new ApplicationDbContext())
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
