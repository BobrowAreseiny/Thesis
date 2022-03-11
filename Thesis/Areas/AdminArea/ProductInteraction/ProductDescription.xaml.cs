using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.ProductInteraction
{
    /// <summary>
    /// Логика взаимодействия для ProductDescription.xaml
    /// </summary>
    public partial class ProductDescription : Page
    {
        private readonly int _productId;
        private readonly Product product1 = new Product();
        public ProductDescription(int productId)
        {
            InitializeComponent();
            _productId = productId;
        }

        private void Data(int productId)
        {
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                if (_product.Items.Count != 0)
                {
                    _product.Items.Clear();
                }
                _productSize.ItemsSource = _context.ProductSize
                    .Where(x => x.ProductId == productId)
                    .ToList();
                Product product = _context.Product
                    .Where(x => x.Id == productId)
                    .FirstOrDefault();
                _product.Items.Add(product);
                _context.Product
                    .Where(p => p.Id == productId)
                    .ToList();
            }
        }

        private void SelectedItem(object sender, SelectionChangedEventArgs e)
        {
            if (_productSize.SelectedItem is ProductSize productSize)
            {
                NavigationService.Navigate(new ProductSizeEdit(productSize));
            }
        }
        
        private void ProductEdit(object sender, SelectionChangedEventArgs e)
        {
            if (_product.SelectedItem is Product product)
            {
                NavigationService.Navigate(new ProductEdit(product));
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductSizeEdit());
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            int sizeId = (int)(sender as Button).Content;
            if (sizeId != 0)
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
                {
                    ProductSize data = _context.ProductSize.Where(x => x.Id == sizeId).FirstOrDefault();
                    _context.Entry(data).State = EntityState.Deleted;
                    _context.SaveChanges();
                    Data(_productId);
                }
            }
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(_productId);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
