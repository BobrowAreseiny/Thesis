using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.ProductInteraction
{
    /// <summary>
    /// Логика взаимодействия для Products.xaml
    /// </summary>
    public partial class Products : Page
    { 
        private readonly int? productId = null;
        public Products()
        {
            InitializeComponent();
        }

        public Products(int Id)
        {
            InitializeComponent();
            productId = Id;
        }

        private void Data(int? productId)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _products.ItemsSource = productId != null
                    ? _context.Product.Where(x => x.Id == productId).ToList()
                    : _context.Product.ToList();
            }
        }

        private void ProductDescription(object sender, SelectionChangedEventArgs e)
        {
            if (_products.SelectedItem is Product product)
            {
                NavigationService.Navigate(new ProductDescription(product.Id));
            }
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(productId);
            }
        }
    }
}
