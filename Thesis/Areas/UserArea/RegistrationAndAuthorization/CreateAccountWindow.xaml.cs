using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Thesis.Data.Model;
using TicketBooking;

namespace Thesis.Areas.UserArea.RegistrationAndAuthorization
{
    /// <summary>
    /// Логика взаимодействия для CreateAccountWindow.xaml
    /// </summary>
    public partial class CreateAccountWindow : Window
    {
        public CreateAccountWindow()
        {
            InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            string phoneRegex = @"(8 0(25|29|33|34) ([0-9]{3}( [0-9]{2}){2}))";
            int? _counterpartyId;

            if (passward.Password == confirmPassward.Password)
            {
                if (EmailIsValid(email.Text) & CountIsValid(fio) & DataRegex(telephone, phoneRegex))
                {
                    _counterpartyId = CounterpartyTable(telephone.Text, fio.Text, firm.Text, null);
                    if (_counterpartyId != null)
                    {
                        ApplicationUser user = ApplicationUserTable(email.Text, Crypto.Hash(passward.Password), (int)_counterpartyId);
                        if (user != null)
                        {
                            new MainWindow(user).Show();
                            Close();
                        }
                    }
                }
            }
            else
            {
                passward.BorderBrush = Brushes.Red;
                confirmPassward.BorderBrush = Brushes.Red;
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

        private bool CountIsValid(TextBox data)
        {
            if (data.Text.Length < 100)
            {
                return true;
            }
            else
            {
                data.Background = Brushes.Red;
            }
            return false;
        }

        private bool DataRegex(TextBox data, string regex)
        {
            if (Regex.IsMatch(data.Text, regex, RegexOptions.IgnoreCase))
            {
                return true;
            }
            else
            {
                data.BorderBrush = Brushes.Red;
                return false;
            }
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsNumeric(e.Text);
        }
        private static bool IsNumeric(string str)
        {
            Regex reg = new Regex("[^0-9]");
            return reg.IsMatch(str);
        }

        private void TextOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsText(e.Text);
        }
        private static bool IsText(string str)
        {
            Regex reg = new Regex("[^А-я]");
            return reg.IsMatch(str);
        }

        private void EngTextOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = EngIsText(e.Text);
        }
        private static bool EngIsText(string str)
        {
            Regex reg = new Regex("[^A-z0-9@.]");
            return reg.IsMatch(str);
        }

        private int? CounterpartyTable(string telephone, string fio, string name, int? adressId)
        {
            int? _counterpartyId = null;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Counterparty counterparty = new Counterparty()
                {
                    Telephone = telephone,
                    ContactPerson = fio,
                    Name = name,
                    AddressId = adressId,
                };
                _context.Counterparty.Add(counterparty);
                _context.SaveChanges();
                _counterpartyId = counterparty.Id;
            }
            return _counterpartyId;
        }

        private ApplicationUser ApplicationUserTable(string email, string userPassword, int counterpartyId, int roleId = 2)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    Email = email,
                    UserPassword = userPassword,
                    CounterpartyId = counterpartyId,
                    UserRoleId = roleId,
                };
                _context.ApplicationUser.Add(applicationUser);
                _context.SaveChanges();
                return applicationUser;
            }
        }
    }
}
