using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Thesis.Data.Model;
using TicketBooking;

namespace Thesis.Areas.UserArea
{
    /// <summary>
    /// Логика взаимодействия для CreateAccountWindow.xaml
    /// </summary>
    public partial class CreateAccountWindow : Window
    {
        private readonly List<Basket> _basket;

        public CreateAccountWindow(List<Basket> basket)
        {
            InitializeComponent();
            _basket = basket;
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            new LoginWindow(_basket).Show();
            Close();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            bool errors = false;
            string fioRegex = "^[А - ЯЁ][а - яё] * [А - ЯЁ][а - яё] * [А - ЯЁ][а - яё] *$";
            string emailRegex = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            string phoneRegex = @"^(\+375|80)(29|25|44|33)(\d{3})(\d{2})(\d{2})$";
            string sityRegex = @"(^[\w\s]+,\s\w{2}$)";
            string indexRegex = @"(^\d{5}$)";
            int _townId = TownTable(townIndex.Text, townName.Text);
            int _adressId = 0;
            int _counterpartyId = 0;

            //errors = DataRegex(fio, fioRegex, errors);
            //errors = DataRegex(email, emailRegex, errors);
            //errors = DataRegex(telephone, phoneRegex, errors);
            //errors = DataRegex(street, emailRegex, errors); //
            //errors = DataRegex(buildingNumber, emailRegex, errors); //
            //errors = DataRegex(roomNumber, emailRegex, errors); //
            //errors = DataRegex(townName, sityRegex, errors);
            //errors = DataRegex(townIndex, indexRegex, errors);

            //if (errors == false)
            //{

            if (_townId != 0)
            {
                _adressId = AdressTable(street.Text, buildingNumber.Text, roomNumber.Text, _townId);
            }
            if (_adressId != 0)
            {
                _counterpartyId = CounterpartyTable(telephone.Text, fio.Text, "Вставить имя конторы", _adressId);
            }
            if (_adressId != 0)
            {
                ApplicationUser user = ApplicationUserTable(email.Text, Crypto.Hash(passward.Password), _counterpartyId);
                if (user != null)
                {
                    new MainWindow(user, _basket).Show();
                    Close();
                }
            }

            //}
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
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
        private void TextOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsText(e.Text);
        }
        private static bool IsText(string str)
        {
            Regex reg = new Regex("[^А-я]");
            return reg.IsMatch(str);
        }
        private static bool IsNumeric(string str)
        {
            Regex reg = new Regex("[^0-9]");
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
