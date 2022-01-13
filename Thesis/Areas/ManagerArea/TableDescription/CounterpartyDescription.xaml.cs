using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Data.Entity;
using Thesis.Areas.ManagerArea.Table;
using Thesis.Data.Model;
using System.Windows.Controls;
using Thesis.Areas.ManagerArea.TableEditor;

namespace Thesis.Areas.ManagerArea.TableDescription
{
    /// <summary>
    /// Логика взаимодействия для CounterpartyDescription.xaml
    /// </summary>
    public partial class CounterpartyDescription : Window
    {
        private readonly List<OrderData> data = new List<OrderData>();

        public CounterpartyDescription(Counterparty user)
        {
            InitializeComponent();
            Data(user);
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            //new AddOrderConstruction(null).Show();
            //Close();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {

        }

        private void OrderDescription(object sender, SelectionChangedEventArgs e)
        {
            if (_orders.SelectedItem is OrderData order)
            {
                new UserOrderDescription(order).Show();
            }
        }

        private void UserInformation(object sender, SelectionChangedEventArgs e)
        {
            if (_user.SelectedItem is Counterparty user)
            {
                new AddCounterparty(user).Show();
            }
        }

        private void CounterpartyInformation(object sender, RoutedEventArgs e)
        {
            int counterpartyId = ((sender as Button).DataContext as UserAccount).CounterpartyId;
            Counterparty selectedCounterparty = new ApplicationDbContext().Counterparty
                    .Where(x => x.Id == counterpartyId)
                    .Include(x => x.Address)
                    .Include(x => x.ApplicationUser)
                    .Include(x => x.UserOrder)
                    .FirstOrDefault();
            new AddCounterparty(selectedCounterparty).Show();
        }

        private void Data(Counterparty counterparty)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                List<UserOrder> order = _context.UserOrder
                    .Where(x => x.CounterpartyId == counterparty.Id)
                    .ToList();
                if (order != null)
                {
                    foreach (UserOrder dataItem in order)
                    {
                        //OrderData item = new OrderData(dataItem.Id)
                        //{
                        //    OrderNumber = dataItem.OrderNumber,
                        //    Counterparty = dataItem.Counterparty.Name,
                        //    DateOfShipment = dataItem.DateOfShipment,
                        //    Status = dataItem.Status,
                        //    OrderConstructionCount = _context.OrderConstruction.Where(x => x.UserOrderId == dataItem.Id).Count(),
                        //    OrderConstructionMade = _context.OrderConstruction.Where(x => x.UserOrderId == dataItem.Id && x.Status == "Готов").Count(),
                        //};
                        //if (item.OrderConstructionMade == item.OrderConstructionCount)
                        //{
                        //    item.Status = "Готов";
                        //}
                        //data.Add(item);
                    }
                    if (data != null)
                    {
                        _orders.ItemsSource = data;
                        _user.Items.Add(counterparty);
                    }
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            //Close();
        }

        private void OrderCatalog(object sender, RoutedEventArgs e)
        {
            new MainManagerWindow().Show();
           // Close();
        }
    }
}
