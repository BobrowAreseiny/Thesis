using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Data.Model;

namespace Thesis.Areas.MarketingArea
{
    /// <summary>
    /// Логика взаимодействия для MainMarketingArea.xaml
    /// </summary>
    public partial class MainMarketingWindow : Window
    {
        private readonly List<OrderData> data = new List<OrderData>();

        public MainMarketingWindow()
        {
            InitializeComponent();
            Data();
        }

        private void Data()
        {
            List<UserOrder> usersOrders = new List<UserOrder>();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                usersOrders = _context.UserOrder.ToList();

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
                //NavigationService.Navigate(new UserOrderDescription(order.Id));
            }
        }
    }
}
