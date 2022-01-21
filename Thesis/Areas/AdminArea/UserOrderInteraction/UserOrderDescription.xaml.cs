using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.UserOrderInteraction
{
    /// <summary>
    /// Логика взаимодействия для UserOrderDescription.xaml
    /// </summary>
    public partial class UserOrderDescription : Page
    {
        private readonly int _userOrderId;

        public UserOrderDescription(int OrderId)
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

                UserOrder order = _context.UserOrder.Where(x => x.Id == OrderId).FirstOrDefault();


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


        private void OrderEdit(object sender, SelectionChangedEventArgs e)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (_order.SelectedItem is OrderData item)
                {
                    UserOrder orderEdit = _context.UserOrder
                        .FirstOrDefault(x => x.OrderNumber == item.OrderNumber);
                    NavigationService.Navigate(new UserOrderEdit(orderEdit));
                }
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (_orderConstruction.SelectedItem is OrderConstruction construction)
            {
                NavigationService.Navigate(new OrderConstructionEdit(construction));
            }         
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (_orderConstruction.SelectedItem is OrderConstruction construction)
            {
                using (ApplicationDbContext _context = new ApplicationDbContext())
                {
                    var data = _context.OrderConstruction
                        .Where(x => x.Id == construction.Id)
                        .FirstOrDefault();
                    _context.Entry(data).State = EntityState.Deleted;
                    _context.SaveChanges();
                }
                Data(_userOrderId);
            }
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(_userOrderId);
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

        private void AddOrderConstruction(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderConstructionEdit());
        }
    }
}