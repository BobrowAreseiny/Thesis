using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Thesis.Data.Model;
using TicketBooking;

namespace Thesis.Areas.UserArea.UserBasket
{
    /// <summary>
    /// Логика взаимодействия для Purchase.xaml
    /// </summary> 

    public partial class Purchase : Window
    {
        private readonly List<Basket> _basket = new List<Basket>();
        private readonly ApplicationUser _account = null;

     

        public Purchase(ApplicationUser user, List<Basket> basket)
        {
            InitializeComponent();
            _account = user;
            _basket = basket;
            Data();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Data()
        {
            double price = 0;
            foreach (Basket item in _basket)
            {
                price += item.Cost;
            }
            if (price != 0)
            {
                itogCount.Content = price;
                _creel.ItemsSource = _basket;
            }           
        }

        private void Buy(object sender, RoutedEventArgs e)
        {
            string indexRegex = @"^\d{5}(?:[-\s]\d{4})?$";
            int? _townId;

            if (CountIsValid(street) & CountIsValid(buildingNumber) & CountIsValid(roomNumber) & CountIsValid(townName) & DataRegex(addressIndex, indexRegex))
            {
                _townId = TownTable(townName.Text);
                if (_townId != null)
                {
                    int? accountId = AdressTable(street.Text, buildingNumber.Text, roomNumber.Text, addressIndex.Text, (int)_townId);
                    if (accountId != null)
                    {
                        if (CounterpartyEdit(accountId))
                        {
                            int? orderId = AddUserOrder();
                            if (orderId != null)
                            {
                                AddOrderConstruction(orderId);
                                Close();
                            }
                        }
                    }
                }
            }
        }

        private int? AddUserOrder()
        {
            int? orderId = null;
            string number = string.Empty;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                int count = _context.UserOrder.Count();
                Counterparty counterparty = _context.Counterparty
                    .Where(x => x.Id == _account.CounterpartyId)
                    .FirstOrDefault();
                UserOrder orderNumber = _context.UserOrder
                    .OrderByDescending(x => x.OrderNumber)
                    .FirstOrDefault();

                UserOrder order = new UserOrder()
                {
                    CounterpartyId = counterparty.Id,
                    DateOfPayment = DateTime.Now,
                    Status = "Ожидание",
                    TotalAmount = 0,
                    OrderNumber = orderNumber.OrderNumber + 1,
                    DateOfShipment = DateTime.Now.AddDays(Convert.ToInt32(dateOfShipment.Text)),
                };
                _context.UserOrder.Add(order);
                _context.SaveChanges();
                orderId = order.Id;
            }
            return orderId;
        }

        private void AddOrderConstruction(int? orderId)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                foreach (var item in _basket)
                {
                    string OrderNumber = Crypto.Hash(Crypto.Hash(DateTime.Now.AddDays(14).ToString()) + DateTime.Now.ToString());
                    var y = OrderNumber.Trim(new char[] { '=', '+', ' ' }).Skip(OrderNumber.Length - 4);
                    OrderConstruction construction = new OrderConstruction()
                    {
                        UserOrderId = orderId,
                        ProductSizeId = item.Size.Id,
                        Amount = item.Count,
                       // NumberOfProucts = "#" + OrderNumber.Trim(new char[] { '=', '+', ' ' }).Skip(OrderNumber.Length - 4),
                        Price = (decimal)(item.Size.Product.Price * item.Count),
                        Status = "Ожидание",
                    };
                    _context.OrderConstruction.Add(construction);
                }
                _context.SaveChanges();
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

        private int TownTable(string townName)
        {
            int _townId = 0;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Town town = _context.Town.Where(x => x.TownName.ToLower() == townName.ToLower()).FirstOrDefault();

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

        private bool CounterpartyEdit(int? addressId)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Counterparty counterparty = _context.Counterparty
                    .Where(x => x.Id == _account.CounterpartyId)
                    .FirstOrDefault();
                if (counterparty != null)
                {
                    counterparty.AddressId = addressId;
                }
                _context.Entry(counterparty).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return true;
        }

        private int? AdressTable(string street, string buildingNumber, string roomNumber, string addressIndex, int townId)
        {
            int? addressId = null;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Address address = new Address()
                {
                    Street = street,
                    BuildingNumber = buildingNumber,
                    RoomNumber = roomNumber,
                    AddressIndex = addressIndex,
                    TownId = townId,
                };
                _context.Address.Add(address);
                _context.SaveChanges();
                addressId = address.Id;
            }
            return addressId;
        }
    }
}
