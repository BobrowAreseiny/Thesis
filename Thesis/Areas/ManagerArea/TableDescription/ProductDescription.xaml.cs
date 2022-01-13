using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Areas.ManagerArea.TableEditor;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.TableDescription
{
    /// <summary>
    /// Логика взаимодействия для ProductDescription.xaml
    /// </summary>
    public partial class ProductDescription : Window
    {
        public ProductDescription(int productId)
        {
            InitializeComponent();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _productSize.ItemsSource = _context.ProductSize
                    .Where(x => x.ProductId == productId)
                    .ToList();
                Product product = _context.Product
                    .Where(x => x.Id == productId)
                    .FirstOrDefault();
                item.Items.Add(product);
            }
        }

        public ProductDescription()
        {
            InitializeComponent();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _productSize.ItemsSource = _context.ProductSize
                    .ToList();
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            //Написать
            Close();
        }

        //private void Edit(object sender, RoutedEventArgs e)
        //{
        //    //new AddAddress((sender as Button).DataContext as Address).Show();
        //    Close();
        //}

        private void Delete(object sender, RoutedEventArgs e)
        {
            ProductSize selectedProductSize = (sender as Button).DataContext as ProductSize;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                ProductSize productSize = _context.ProductSize
                    .Where(x => x.Id == selectedProductSize.Id)
                    .FirstOrDefault();
                _context.ProductSize.Remove(productSize);
                _context.SaveChanges();
            }
        }

      

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OrderCatalog(object sender, RoutedEventArgs e)
        {
            new MainManagerWindow().Show();
            Close();
        }

        private void SelectedItem(object sender, SelectionChangedEventArgs e)
        {
            ProductSize selectedProductSize = _productSize.SelectedItem as ProductSize;
            if (selectedProductSize != null)
            {
                new AddProductSize(selectedProductSize).Show();
                Close();
            }
        }
    }
}
