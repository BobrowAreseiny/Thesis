using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Thesis.Areas.UserArea.ProductsWindows;
using Thesis.Areas.UserArea.UserBasket;
using Thesis.Data.Model;
using TicketBooking;

namespace Thesis.Areas.UserArea.RegistrationAndAuthorization
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        private readonly List<Basket> _basket;
        private readonly ApplicationUser _account;
        private List<OrderData> _data = new List<OrderData>();

        public Profile(ApplicationUser user, List<Basket> basket)
        {
            InitializeComponent();
            _basket = basket;
            _account = user;
            Data(_account.CounterpartyId);
        }


        private void ProductCatalog(object sender, RoutedEventArgs e)
        {
            new ProductsCatalog(_account, _basket).Show();
            Close();
        }

        private void Data(int? userId)
        {
            List<UserOrder> usersOrders = new List<UserOrder>();
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                usersOrders = _context.UserOrder.Where(x => x.CounterpartyId == userId).ToList();
                if (usersOrders != null)
                {
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
            }
            if (_data != null)
            {
                _usersOrders.ItemsSource = _data;
            }
        }

        private void Sherch(object sender, TextChangedEventArgs e)
        {
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

        private void Help(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathToFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Help.chm";
                Process.Start(pathToFile);
            }
            catch
            {
                MessageBox.Show("Неизвестная ошибка");
            }
        }

        private void Creel(object sender, RoutedEventArgs e)
        {
            if (_account != null)
            {
                new Creel(_account, _basket).Show();
                Close();
            }
        }
    }
}
