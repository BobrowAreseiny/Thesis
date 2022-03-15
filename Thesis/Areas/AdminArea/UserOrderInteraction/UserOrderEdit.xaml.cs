using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.UserOrderInteraction
{
    /// <summary>
    /// Логика взаимодействия для UserOrderEdit.xaml
    /// </summary>
    public partial class UserOrderEdit : Page
    {
        private UserOrder _order = new UserOrder();

        public UserOrderEdit()
        {
            InitializeComponent();
            DataContext = _order;
            Data();
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
                Data();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
                {
                    _order.Counterparty = _context.Counterparty
                            .Where(x => x.Id == ((Counterparty)_counterparty.SelectedItem).Id)
                            .FirstOrDefault();
                    if (_order.Id == 0)
                    {
                        _context.UserOrder.Add(_order);
                    }
                    else
                    {
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

                    }
                    _context.SaveChanges();
                    NavigationService.GoBack();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка!");
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Data()
        {
            try
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
                {
                    if (_order.Id != 0)
                    {
                        _counterparty.SelectedItem = _context.Counterparty
                         .Where(x => x.Id == _order.CounterpartyId)
                         .FirstOrDefault();
                    }
                    else
                    {
                        _order.Status = "Ожидание";
                        UserOrder userOrderNumber = _context.UserOrder
                            .OrderBy(x => x.Id)
                            .Skip(_context.UserOrder.Count() - 1)
                            .FirstOrDefault();

                        if (userOrderNumber != null)
                        {
                            _order.OrderNumber = userOrderNumber.Id + 1;
                        }
                    }
                    _counterparty.ItemsSource = _context.Counterparty.ToList();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка!");
            }
        }
    }
}