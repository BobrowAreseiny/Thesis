﻿using Microsoft.Win32;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data.Model;

namespace Thesis.Areas.MarketingArea.UsersOrders
{
    /// <summary>
    /// Логика взаимодействия для SelectedOrder.xaml
    /// </summary>
    public partial class SelectedOrder : Page
    {
        private readonly int _userOrderId;

        public SelectedOrder(int OrderId)
        {
            InitializeComponent();
            _userOrderId = OrderId;
        }

        private void Data(int OrderId)
        {
            OrderData data = null;
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                if (_order.Items.Count != 0)
                {
                    _order.Items.Clear();
                }

                UserOrder order = _context.UserOrder
                    .Where(x => x.Id == OrderId)
                    .FirstOrDefault();

                if (order != null)
                {
                    int OrderConstructionCount = _context.OrderConstruction
                                                 .Where(x => x.UserOrderId == order.Id)
                                                 .Count();
                    int OrderConstructionMade = _context.OrderConstruction
                                                .Where(x => x.UserOrderId == order.Id && x.Status == "Готов")
                                                .Count();
                    data = new OrderData(order, OrderConstructionCount, OrderConstructionMade);
                }
                _orderConstruction.ItemsSource = null;
                _orderConstruction.ItemsSource = _context.OrderConstruction
                        .Where(x => x.UserOrder.OrderNumber == data.OrderNumber)
                        .Include(x => x.UserOrder)
                        .Include(x => x.ProductSize)
                        .Include(x => x.ProductSize.Product)
                        .ToList();

                _order.Items.Add(data);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(_userOrderId);
            }
        }

        private void Notify(object sender, RoutedEventArgs e)
        {
            ApplicationUser appUser = null;
            UserOrder userOrder = null;
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                userOrder = _context.UserOrder
                    .Where(x => x.Id == _userOrderId)
                    .FirstOrDefault();

                appUser = userOrder.Counterparty.ApplicationUser.FirstOrDefault();
            }

            OpenFileDialog OPF = new OpenFileDialog();
            string path = string.Empty;
            if (OPF.ShowDialog() == true)
            {
                path = OPF.FileName;
            }
            if (appUser != null)
            {
                try
                {
                    MailAddress from = new MailAddress("otikominsk@gmail.com", "Otiko");
                    MailAddress to = new MailAddress(appUser.Email);
                    MailMessage mail = new MailMessage(from, to)
                    {
                        Subject = "Статус заказа",
                        Body = $"<h2>Здравствуйте {appUser.Counterparty.Name}, только что был готов ваш заказ № {userOrder.OrderNumber}. <br>" +
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
                catch (Exception)
                {

                }
            }
        }
    }
}