using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Areas.AdminArea;
using Thesis.Areas.ManagerArea.TableEditor;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.TableDescription
{
    /// <summary>
    /// Логика взаимодействия для UserOrderAdmin.xaml
    /// </summary>
    public partial class UserOrderDescription : Window
    {
        private readonly OrderData _data;

        public UserOrderDescription(OrderData data)
        {
            InitializeComponent();
            _data = data;
            Data(data);
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (_data != null)
            {
                new AddOrderConstruction(null, _data).Show();
                Close();
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (_data != null)
            {
                new AddOrderConstruction((sender as Button).DataContext as OrderConstruction, _data).Show();
                Close();
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {

        }

        private void Data(OrderData data)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _orderConstruction.ItemsSource = _context.OrderConstruction
                    .Where(x => x.UserOrder.OrderNumber == data.OrderNumber)
                    .Include(x => x.UserOrder)
                    .Include(x => x.ProductSize)
                    .Include(x => x.ProductSize.Product)
                    .ToList();

                item.Items.Add(data);
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

        private void OrderDescription(object sender, RoutedEventArgs e)
        {
            string userOrder = ((Button)sender).Content.ToString();
            UserOrder order = new UserOrder();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                order = _context.UserOrder
                    .Where(x => x.OrderNumber == userOrder)
                    .FirstOrDefault();

            }
            if (order != null)
            {
                new AddOrder(order, _data).Show();
                Close();
            }
        }
    }
}
