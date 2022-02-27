using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Thesis.Areas.UserArea.ProductsWindows;
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
            email.Text = _account.Email;
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


        private void Edit(object sender, RoutedEventArgs e)
        {
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                ApplicationUser user = _context.ApplicationUser
                            .Where(x => x.Id == _account.Id)
                            .FirstOrDefault();
                if (user != null)
                {
                    if (EmailIsValid(email.Text) && user.UserPassword == Crypto.Hash(passward.Password))
                    {
                        user.Email = email.Text;
                        passward.BorderBrush = Brushes.White;
                    }
                    else
                    {
                        passward.BorderBrush = Brushes.Red;
                    }
                    if (PasswordIsValid())
                    {
                        user.UserPassword = Crypto.Hash(newPassward.Password);
                    }
                    _context.SaveChanges(); 
                    MessageBox.Show("Данные успешно изменены.// Стоит поменять на нормальный вывод");
                }
            }
           
        }

        private bool EmailIsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                email.BorderBrush = Brushes.Red;
                return false;
            }
        }
        private bool PasswordIsValid()
        {
            try
            {
                if (_account.UserPassword == Crypto.Hash(passward.Password) && newPassward.Password.Length > 4)
                {
                    return true;
                }
                else
                {
                    passward.BorderBrush = Brushes.Red;
                    newPassward.BorderBrush = Brushes.Red;
                }
                return false;
            }
            catch (FormatException)
            {
                passward.BorderBrush = Brushes.Red;
                newPassward.BorderBrush = Brushes.Red;
                return false;
            }
        }

        private void newPassward_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
