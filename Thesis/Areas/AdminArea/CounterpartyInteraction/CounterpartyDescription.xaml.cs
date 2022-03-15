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
            ApplicationUser appUser = null;
            if (_accunt.Content != null)
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
                {
                    appUser = _context.ApplicationUser
                        .FirstOrDefault(x => x.Id == (int)_accunt.Content);
                }
            }
            if (appUser != null)
            {
                NavigationService.Navigate(new AccountEdit(appUser));
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

            using (ApplicationDbContent _context = new ApplicationDbContent())
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
                    _orders.ItemsSource = ordersData;
                    if (counterparty.ApplicationUser.Count != 0)
                    {
                        _accunt.Content = _context.ApplicationUser
                            .FirstOrDefault(x => x.CounterpartyId == counterparty.Id).Id;
                    }
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

        private void DeleteOrder(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Точно удалить данные?", "Внимание",
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if ((sender as Button) != null)
                {
                    using (ApplicationDbContent _context = new ApplicationDbContent())
                    {
                        int orderId = (int)((Button)sender).Content;
                        UserOrder selectedData = _context.UserOrder
                            .Where(x => x.Id == orderId)
                            .FirstOrDefault();
                        _context.UserOrder.Remove(selectedData);
                        _context.SaveChanges();
                    }
                    Data(_counterparty);
                }
            }
        }
    }
}
