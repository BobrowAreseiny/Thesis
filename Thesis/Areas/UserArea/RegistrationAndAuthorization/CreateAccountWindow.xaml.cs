using System;
using System.Collections.Generic;
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
            bool errors = false;
            string phoneRegex = @"(8 0(25|29|33|34) ([0-9]{3}( [0-9]{2}){2}))";
            string indexRegex = @"^\d{5}(?:[-\s]\d{4})?$";
            int _adressId = 0;
            int _counterpartyId = 0;
            errors = EmailIsValid(email.Text);
            errors = DataRegex(telephone, phoneRegex, errors);
            errors = DataRegex(townIndex, indexRegex, errors);
            errors = CountIsValid(street);
            errors = CountIsValid(roomNumber);
            errors = CountIsValid(townName);
            errors = CountIsValid(buildingNumber);

            if (errors == false)
            {
                //if (_townId != 0)
                //{
                //    _adressId = AdressTable(street.Text, buildingNumber.Text, roomNumber.Text, _townId);
                //}
                //if (_adressId != 0)
                //{
                    
                //}
                
                _counterpartyId = CounterpartyTable(telephone.Text, fio.Text, "Вставить имя конторы", _adressId);
                if (_adressId != 0)
                {
                    ApplicationUser user = ApplicationUserTable(email.Text, Crypto.Hash(passward.Password), _counterpartyId);
                    if (user != null)
                    {
                        new MainWindow(user).Show();
                        Close();
                    }
                }
            }
        }

        private bool EmailIsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return false;
            }
            catch (FormatException)
            {
                email.BorderBrush = Brushes.Red;
                return true;
            }
        }

        private bool CountIsValid(TextBox data)
        {
            try
            {
                if (data.Text.Length < 100)
                {
                    return false;
                }
                else
                {
                    data.Background = Brushes.Red;
                }
                return true;
            }
            catch (FormatException)
            {
                email.BorderBrush = Brushes.Red;
                return true;
            }
        }

        private bool DataRegex(TextBox data, string regex, bool errorExist)
        {
            if (Regex.IsMatch(data.Text, regex, RegexOptions.IgnoreCase))
            {
                if (errorExist == false)
                {
                    return false;
                }
                return true;
            }
            else
            {
                data.BorderBrush = Brushes.Red;
                return true;
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

        private int TownTable(string townIndex, string townName)
        {
            int _townId = 0;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Town town = _context.Town.Where(x => x.TownName == townName).FirstOrDefault();

                if (town == null)
                {
                    Town newTown = new Town
                    {
                        TownName = townName
                    };

                    _context.Town.Add(newTown);
                    _context.SaveChanges();
                    _townId = newTown.Id;
                }
                else
                {
                    _townId = town.Id;
                }
            }
            return _townId;
        }

        private int AdressTable(string street, string buildingNumber, string roomNumber, int townId)
        {
            int _adressId = 0;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Address address = new Address()
                {
                    Street = street,
                    BuildingNumber = buildingNumber,
                    RoomNumber = roomNumber,
                    TownId = townId,
                };
                _context.Address.Add(address);
                _context.SaveChanges();
                _adressId = address.Id;
            }
            return _adressId;
        }

        private int CounterpartyTable(string telephone, string fio, string name, int adressId)
        {
            int _counterpartyId = 0;
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
