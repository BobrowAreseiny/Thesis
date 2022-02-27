using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Thesis.Areas.UserArea.ProductsWindows;
using Thesis.Data;
using Thesis.Data.Model;

namespace Thesis.Areas.UserArea.UserBasket
{
    /// <summary>
    /// Логика взаимодействия для Purchase.xaml
    /// </summary> 

    public partial class Purchase : Window
    {
        private readonly string pathToFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
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
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                Counterparty counterparty = _context.Counterparty
                    .Where(x => x.Id == _account.CounterpartyId)
                    .FirstOrDefault();

                if (counterparty.Address != null)
                {
                    street.Text = counterparty.Address.Street;
                    buildingNumber.Text = counterparty.Address.BuildingNumber;
                    roomNumber.Text = counterparty.Address.RoomNumber;
                    townName.Text = counterparty.Address.Town.TownName;
                    addressIndex.Text = counterparty.Address.AddressIndex;
                }
            }
        }

        private void Buy(object sender, RoutedEventArgs e)
        {
            string indexRegex = @"^\d{5}(?:[-\s]\d{4})?$";
            int? _townId;

            try
            {
                if (CountIsValid(street) & CountIsValid(buildingNumber) & CountIsValid(roomNumber) & CountIsValid(townName) & DataRegex(addressIndex, indexRegex) & dateOfShipment != null)
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
                                    if (AddOrderConstruction(orderId))
                                    {
                                        Notify(orderId);
                                        new ProductsCatalog(_account, null).Show();
                                        Close();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Неизвестная ошибка");
            }
        }

        private void Notify(int? _userOrderId)
        {
            try
            {
                if (_userOrderId != null)
                {
                    string path = string.Empty;
                    ApplicationUser appUser = null;
                    UserOrder userOrder = null;
                    using (ApplicationDbContent _context = new ApplicationDbContent())
                    {
                        userOrder = _context.UserOrder
                            .Where(x => x.Id == _userOrderId)
                            .FirstOrDefault();

                        appUser = userOrder.Counterparty.ApplicationUser.FirstOrDefault();
                        path = pathToFile + $@"\Bill\Report{_context.UserOrder.Where(x => x.Id == _userOrderId).FirstOrDefault().OrderNumber}.xlsx";
                    }
                    if (appUser != null)
                    {
                        MailAddress from = new MailAddress("otikominsk@gmail.com", "Otiko");
                        MailAddress to = new MailAddress(appUser.Email);
                        MailMessage mail = new MailMessage(from, to)
                        {
                            Subject = "Получен заказ",
                            Body = $"<h2>Здравствуйте {appUser.Counterparty.Name}, только что был сделан заказ № {userOrder.OrderNumber}. <br>" +
                            "<br><br>Внимание, сообщение отправлено автоматически, на него не нужно отвечать.</h2>"
                        };

                        mail.Attachments.Add(new Attachment(path));
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                        {
                            Credentials = new NetworkCredential("otikominsk@gmail.com", "52K262911"),
                            EnableSsl = true
                        };
                        smtp.Send(mail);

                    }
                }
            }
            catch
            {
                MessageBox.Show("Неизвестная ошибка");
            }
        }

        private int? AddUserOrder()
        {
            int? orderId = null;
            string number = string.Empty;
            using (ApplicationDbContent _context = new ApplicationDbContent())
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

        private bool AddOrderConstruction(int? orderId)
        {
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                UserOrder OrderNumber = new UserOrder();
                OrderConstruction Construction = new OrderConstruction();
                foreach (Basket item in _basket)
                {
                    OrderNumber = _context.UserOrder
                        .OrderBy(x => x.Id)
                        .Skip(_context.UserOrder.Count() - 1)
                        .FirstOrDefault();
                    Construction = _context.OrderConstruction
                        .OrderBy(x => x.Id)
                        .Skip(_context.OrderConstruction.Count() - 1)
                        .FirstOrDefault();

                    if (OrderNumber != null)
                    {
                        OrderConstruction construction = new OrderConstruction()
                        {
                            UserOrderId = orderId,
                            ProductSizeId = item.Size.Id,
                            Amount = item.Count,
                            Price = 0,
                            NumberOfProucts = Construction.NumberOfProucts + 1,
                            Status = "Ожидание",
                        };
                        _context.OrderConstruction.Add(construction);
                        _context.SaveChanges();
                    }
                }
                
                List<OrderConstruction> order = _context.OrderConstruction
                    .Where(x => x.UserOrderId == orderId)
                    .Include(x => x.UserOrder)
                    .Include(x => x.ProductSize)
                    .ToList();

                byte[] reportExcel = new MarketExcelGenerator().GenerateBill(order);//  генерация чека
                File.WriteAllBytes(pathToFile + $@"\Bill\Report{_context.UserOrder.Where(x => x.Id == orderId).FirstOrDefault().OrderNumber}.xlsx", reportExcel);
                return true;
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
            using (ApplicationDbContent _context = new ApplicationDbContent())
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
            using (ApplicationDbContent _context = new ApplicationDbContent())
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
            using (ApplicationDbContent _context = new ApplicationDbContent())
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
