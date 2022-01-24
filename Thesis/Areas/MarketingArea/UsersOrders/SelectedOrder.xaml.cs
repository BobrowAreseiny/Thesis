using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data.Model;

namespace Thesis.Areas.MarketingArea.UsersOrders
{
    /// <summary>
    /// Логика взаимодействия для SelectedOrder.xaml
    /// </summary>
    public partial class SelectedOrder : Page
    {
        private readonly int _userOrderId;

        public SelectedOrder(int OrderId)
        {
            InitializeComponent();
            _userOrderId = OrderId;
        }

        private void Data(int OrderId)
        {
            OrderData data = null;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                if (_order.Items.Count != 0)
                {
                    _order.Items.Clear();
                }

                UserOrder order = _context.UserOrder
                    .Where(x => x.Id == OrderId)
                    .FirstOrDefault();

                if (order != null)
                {
                    int OrderConstructionCount = _context.OrderConstruction
                                                 .Where(x => x.UserOrderId == order.Id)
                                                 .Count();
                    int OrderConstructionMade = _context.OrderConstruction
                                                .Where(x => x.UserOrderId == order.Id && x.Status == "Готов")
                                                .Count();
                    data = new OrderData(order, OrderConstructionCount, OrderConstructionMade);
                }
                _orderConstruction.ItemsSource = null;
                _orderConstruction.ItemsSource = _context.OrderConstruction
                        .Where(x => x.UserOrder.OrderNumber == data.OrderNumber)
                        .Include(x => x.UserOrder)
                        .Include(x => x.ProductSize)
                        .Include(x => x.ProductSize.Product)
                        .ToList();

                _order.Items.Add(data);
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

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(_userOrderId);
            }
        }
    }
}
