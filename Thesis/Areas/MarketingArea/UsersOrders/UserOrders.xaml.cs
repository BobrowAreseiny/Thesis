using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data;
using Thesis.Data.Model;

namespace Thesis.Areas.MarketingArea.UsersOrders
{
    /// <summary>
    /// Логика взаимодействия для UsersOrders.xaml
    /// </summary>
    public partial class UserOrders : Page
    {
        private readonly string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
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
                NavigationService.Navigate(new SelectedOrder(order.Id));
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
                .Where(x => x.OrderNumber.Value.ToString() == textSearch.Text)
                .ToList();
            if (data != null)
            {
                _usersOrders.ItemsSource = null;
                _usersOrders.ItemsSource = data;
            }
            if (textSearch.Text == string.Empty)
            {
                _usersOrders.ItemsSource = null;
                _usersOrders.ItemsSource = _data;
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(userId);
            }
        }



        private void OrderReport(object sender, RoutedEventArgs e)
        {   
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                List<OrderConstruction> data = _context.OrderConstruction
                    .Where(x => x.UserOrderId == 1)
                    .ToList();
                byte[] reportExcel = new MarketExcelGenerator().Generate(data);
                File.WriteAllBytes(desktopPath + @"\Report.xlsx", reportExcel);
            }
        }

        private void SaleReport(object sender, RoutedEventArgs e)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                List<UserOrder> data = _context.UserOrder
                   .ToList();
                byte[] reportExcel = new MarketExcelGenerator().Generate(data);
                File.WriteAllBytes(desktopPath + @"\SaleReport.xlsx", reportExcel);
            }
        }
    }
}
