using System.Linq;
using System.Windows.Controls;
using Thesis.Areas.ManagerArea.TableDescription;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.Table
{
    /// <summary>
    /// Логика взаимодействия для ProductTable.xaml
    /// </summary>
    public partial class ProductTablePage : Page
    {
        private readonly MainManagerWindow _window;

        public ProductTablePage(MainManagerWindow window)
        {
            InitializeComponent();
            _window = window;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _products.ItemsSource = _context.Product.ToList();
            }
        }

        private void ProductDescription(object sender, SelectionChangedEventArgs e)
        {
            Product selectedProduct = (Product)_products.SelectedItem;
            if (selectedProduct != null)
            {
                new ProductDescription(selectedProduct.Id).Show();
                _window.Close();
            }
        }
    }
}
