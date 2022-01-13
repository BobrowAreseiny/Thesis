using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.UserOrderInteraction
{
    /// <summary>
    /// Логика взаимодействия для UsersOrders.xaml
    /// </summary>
    public partial class UserOrders : Page
    {
        private readonly List<OrderData> data = new List<OrderData>();
        private readonly int? userId = null;

        public UserOrders()
        {
            InitializeComponent();
        }

        public UserOrders(int Id)
        {
            InitializeComponent();
            userId = Id;
        }

        private void Data(int? userId)
        {
            List<UserOrder> usersOrders = new List<UserOrder>();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());

                usersOrders = userId == null ? _context.UserOrder.ToList() : _context.UserOrder.Where(x => x.CounterpartyId == userId).ToList();

                foreach (UserOrder dataItem in usersOrders)
                {
                    int OrderConstructionCount = _context.OrderConstruction
                                                 .Where(x => x.UserOrderId == dataItem.Id)
                                                 .Count();
                    int OrderConstructionMade = _context.OrderConstruction
                                                .Where(x => x.UserOrderId == dataItem.Id && x.Status == "Готов")
                                                .Count();

                    OrderData item = new OrderData(dataItem, OrderConstructionCount, OrderConstructionMade);
                    data.Add(item);
                }
            }
            if (data != null)
            {
                _usersOrders.ItemsSource = data;
            }
        }

        private void OrderSelector(object sender, SelectionChangedEventArgs e)
        {
            if (_usersOrders.SelectedItem is OrderData order)
            {
                NavigationService.Navigate(new UserOrderDescription(order.Id));
            }
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(userId);
            }
        }
    }
}
