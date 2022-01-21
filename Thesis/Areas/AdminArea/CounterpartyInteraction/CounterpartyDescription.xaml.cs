using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Areas.AdminArea.UserOrderInteraction;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.CounterpartyInteraction
{
    /// <summary>
    /// Логика взаимодействия для CounterpartyDescription.xaml
    /// </summary>
    public partial class CounterpartyDescription : Page
    {
        private readonly Counterparty _counterparty;

        public CounterpartyDescription(Counterparty counterparty)
        {
            InitializeComponent();
            _counterparty = counterparty;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserOrderEdit());
        }

        private void Delete(object sender, RoutedEventArgs e)
        {

        }

        private void OrderDescription(object sender, SelectionChangedEventArgs e)
        {
            if (_orders.SelectedItem is OrderData order)
            {
                NavigationService.Navigate(new UserOrderDescription(order.Id));
            }
        }

        private void UserInformation(object sender, SelectionChangedEventArgs e)
        {
            if (_conterparty.SelectedItem is Counterparty user)
            {
                NavigationService.Navigate(new CounterpartyEdit(user));
            }
        }

        private void AccountDescription(object sender, RoutedEventArgs e)
        {
            ApplicationUser user = new ApplicationUser();

            if (((Button)sender).Content is ApplicationUser selectedUser)
            {
                if (user != null)
                {
                    NavigationService.Navigate(new AccountEdit(selectedUser));
                }
            }
            else
            {
                NavigationService.Navigate(new AccountEdit(_counterparty.Id));
            }
        }

        private void Data(Counterparty counterparty)
        {
            List<OrderData> ordersData = new List<OrderData>();
            OrderData orderData = null;

            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                if (_conterparty.Items.Count != 0)
                {
                    _conterparty.Items.Clear();
                }

                List<UserOrder> order = _context.UserOrder
                    .Where(x => x.CounterpartyId == counterparty.Id)
                    .ToList();

                if (order != null)
                {
                    foreach (UserOrder dataItem in order)
                    {
                        int OrderConstructionCount = _context.OrderConstruction
                            .Where(x => x.UserOrderId == dataItem.Id)
                            .Count();
                        int OrderConstructionMade = _context.OrderConstruction
                            .Where(x => x.UserOrderId == dataItem.Id && x.Status == "Готов")
                            .Count();

                        orderData = new OrderData(dataItem, OrderConstructionCount, OrderConstructionMade);

                        ordersData.Add(orderData);

                    }
                    _accunt.Content = _context.ApplicationUser
                        .Where(x => x.CounterpartyId == counterparty.Id)
                        .FirstOrDefault();
                    _orders.ItemsSource = ordersData;
                    _conterparty.Items.Add(counterparty);
                }
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
                Data(_counterparty);
            }
        }
    }
}
