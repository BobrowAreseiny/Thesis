using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.UserOrderInteraction
{
    /// <summary>
    /// Логика взаимодействия для UserOrderEdit.xaml
    /// </summary>
    public partial class UserOrderEdit : Page
    {
        private readonly UserOrder _order = new UserOrder();

        public UserOrderEdit()
        {
            InitializeComponent();
            DataContext = _order;
        }

        public UserOrderEdit(UserOrder order)
        {
            InitializeComponent();
            try
            {
                if (order != null)
                {
                    _order = order;
                }
                DataContext = _order;
                Data(_order);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (_order.Id == 0)
                {
                    _context.UserOrder.Add(_order);
                }
                else
                {
                    _order.Counterparty = _context.Counterparty
                        .Where(x => x.Id == ((Counterparty)_counterparty.SelectedItem).Id)
                        .FirstOrDefault();

                    UserOrder data = _context.UserOrder
                        .Where(f => f.Id == _order.Id)
                        .Include(x => x.Counterparty)
                        .FirstOrDefault();

                    if (data != null)
                    {
                        data.OrderNumber = _order.OrderNumber;
                        data.DateOfPayment = _order.DateOfPayment;
                        data.Status = _order.Status;
                        data.DateOfShipment = _order.DateOfShipment;
                        data.OrderNumber = _order.OrderNumber;
                        data.TotalAmount = _order.TotalAmount;
                        data.Counterparty = _order.Counterparty;
                    }
                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();
                    NavigationService.GoBack();                    
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Data(UserOrder order)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _counterparty.ItemsSource = _context.Counterparty.ToList();
                _counterparty.SelectedItem = _context.Counterparty
                    .Where(x => x.Id == _order.CounterpartyId)
                    .FirstOrDefault();
            }
        }

        private void Pull(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {

            }
        }
    }
}