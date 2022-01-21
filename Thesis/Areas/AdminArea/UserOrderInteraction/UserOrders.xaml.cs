using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.UserOrderInteraction
{
    /// <summary>
    /// Логика взаимодействия для UsersOrders.xaml
    /// </summary>
    public partial class UserOrders : Page
    {
        private List<OrderData> _data = new List<OrderData>();
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
                _data = new List<OrderData>();
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
                    _data.Add(item);
                }
            }
            if (_data != null)
            {
                _usersOrders.ItemsSource = _data;
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


        private void DateSort(object sender, RoutedEventArgs e)
        {
            List<OrderData> data = _data;
            if (_number.IsChecked == true)
            {
                data = data.OrderBy(x => x.DateOfShipment).ToList();
                _usersOrders.ItemsSource = null;
                _usersOrders.ItemsSource = data;
            }
            else
            {
                _usersOrders.ItemsSource = null;
                _usersOrders.ItemsSource = _data;
            }
        }

        private void Sherch(object sender, TextChangedEventArgs e)
        {
            _number.IsChecked = false;
            List<OrderData> data = _data
                .Where(x => x.OrderNumber.ToLower().Contains(textSearch.Text.ToLower()))
                .ToList();
            if (data != null)
            {
                _usersOrders.ItemsSource = null;
                _usersOrders.ItemsSource = data;
            }
        }

        private void DeleteOrder(object sender, RoutedEventArgs e)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                int orderId = (int)((Button)sender).Content;
                UserOrder selectedData = _context.UserOrder.Where(x => x.Id == orderId).FirstOrDefault();
                _context.UserOrder.Remove(selectedData);
                _context.SaveChanges();
            }
            Data(userId);
        }

        private void AddOrderNumber(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserOrderEdit());
        }
    }
}
