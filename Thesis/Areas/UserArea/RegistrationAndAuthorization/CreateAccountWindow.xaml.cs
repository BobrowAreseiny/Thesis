using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Thesis.Areas.UserArea.ProductsWindows;
using Thesis.Data.Model;
using TicketBooking;

namespace Thesis.Areas.UserArea.RegistrationAndAuthorization
{
    /// <summary>
    /// Логика взаимодействия для CreateAccountWindow.xaml
    /// </summary>
    public partial class CreateAccountWindow : Window
    {
        private int cnt = 0;
        private readonly int maxLenght = 11;

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
            string phoneRegex = @"(8 0(25|29|33|34) ([0-9]{3}) ([0-9]{2}) ([0-9]{2}))";
            int? _counterpartyId;
            BrushesWhite();

            if (EmailIsValid(email.Text) & CountIsValid(fio) & CountIsValid(firm) & DataRegex(telephone, phoneRegex) & (passward.Password == confirmPassward.Password) & passward.Password.Length >= 5)
            {
                _counterpartyId = CounterpartyTable(telephone.Text, fio.Text, firm.Text, null);
                if (_counterpartyId != null)
                {
                    ApplicationUser user = ApplicationUserTable(email.Text, Crypto.Hash(passward.Password), (int)_counterpartyId);
                    if (user != null)
                    {
                        new ProductsCatalog(user).Show();
                        Close();
                    }
                }
            }
            else
            {
                passward.BorderBrush = Brushes.Red;
                confirmPassward.BorderBrush = Brushes.Red;
            }
        }

        private void BrushesWhite()
        {
            passward.BorderBrush = Brushes.White;
            confirmPassward.BorderBrush = Brushes.White;
            email.BorderBrush = Brushes.White;
            firm.BorderBrush = Brushes.White;
            fio.BorderBrush = Brushes.White;
            telephone.BorderBrush = Brushes.White;
        }

        private bool EmailIsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (Exception)
            {
                email.BorderBrush = Brushes.Red;
                return false;
            }
        }

        private bool CountIsValid(TextBox data)
        {
            if (data.Text.Length < 100 && data.Text.Length > 5)
            {
                return true;
            }
            else
            {
                data.BorderBrush = Brushes.Red;
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
            TextBox tb = sender as TextBox;
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                return;
            }
            else
            {
                cnt++;
            }

            if (cnt > maxLenght)
            {
                cnt = maxLenght;
                e.Handled = true;
                return;
            }
            tb.Text += e.Text;
            if (cnt == 1 || cnt == 4 || cnt == 7)
            {
                tb.Text += " ";
            }
            else if (cnt == 9)
            {
                tb.Text += " ";
            }
            e.Handled = true;
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
            try
            {
                int? _counterpartyId = null;
                using (ApplicationDbContent _context = new ApplicationDbContent())
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
            catch
            {
                return null;
            }
        }

        private ApplicationUser ApplicationUserTable(string email, string userPassword, int counterpartyId, int roleId = 3)
        {
            try
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
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
            catch
            {
                return null;
            }
        }

        private void telephone_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.CaretIndex = tb.Text.Length;
            cnt = tb.Text.Replace(" ", "").Replace("-", "").Length;
        }
    }
}
