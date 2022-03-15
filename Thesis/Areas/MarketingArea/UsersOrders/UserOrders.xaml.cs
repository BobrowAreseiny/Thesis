using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data;
using Thesis.Data.Model;

namespace Thesis.Areas.MarketingArea.UsersOrders
{
    /// <summary>
    /// Логика взаимодействия для UsersOrders.xaml
    /// </summary>
    public partial class UserOrders : Page
    {
        private readonly string pathToFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private List<OrderData> _data = new List<OrderData>();
        private readonly int? userId = null;

        public UserOrders()
        {
            InitializeComponent();
        }

        public UserOrders(int Id)
        {
            InitializeComponent();
            userId = Id;
        }

        private void Data(int? userId)
        {
            List<UserOrder> usersOrders = new List<UserOrder>();
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                _data = new List<OrderData>();
                usersOrders = userId == null ? _context.UserOrder.ToList() : _context.UserOrder.Where(x => x.CounterpartyId == userId).ToList();

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
            if (_data != null)
            {
                _usersOrders.ItemsSource = _data;
            }
        }

        private void OrderSelector(object sender, SelectionChangedEventArgs e)
        {
            if (_usersOrders.SelectedItem is OrderData order)
            {
                NavigationService.Navigate(new SelectedOrder(order.Id));
            }
        }

        private void NameSort(object sender, RoutedEventArgs e)
        {
            List<OrderData> data = _data;
            if (_number.IsChecked == true)
            {
                data = data.Where(x => x.Status == "Готов").ToList();
                _date.IsChecked = false;
                _usersOrders.ItemsSource = null;
                _usersOrders.ItemsSource = data;
            }
            else
            {
                _usersOrders.ItemsSource = null;
                _usersOrders.ItemsSource = _data;
            }
        }
        private void DateSort(object sender, RoutedEventArgs e)
        {
            List<OrderData> data = _data;
            if (_date.IsChecked == true)
            {
                data = data.OrderByDescending(x => x.DateOfShipment).ToList();
                _number.IsChecked = false;
                _usersOrders.ItemsSource = null;
                _usersOrders.ItemsSource = data;
            }
            else
            {
                _usersOrders.ItemsSource = null;
                _usersOrders.ItemsSource = _data;
            }
        }

        private void Sherch(object sender, TextChangedEventArgs e)
        {
            _number.IsChecked = false;
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

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(userId);
            }
        }


        private void SaleReport(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
                {
                    List<UserOrder> data = _context.UserOrder
                       .Where(x => x.Status == "Готов")
                       .ToList();

                    byte[] reportExcel = new MarketExcelGenerator().Generate(data);
                    string commandText = pathToFile + @"\Sale.xlsx";
                    File.WriteAllBytes(commandText, reportExcel);

                    OpenFile(commandText, reportExcel);
                }
            }
            catch
            {
                MessageBox.Show("Ошибка генерации отчета");
            }
        }

        private void ProductReport(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
                {
                    List<OrderConstruction> data = _context.OrderConstruction
                       .Where(x => x.Status == "Готов")
                       .ToList();

                    byte[] reportExcel = new MarketExcelGenerator().Generate(data, 3);
                    string commandText = pathToFile + @"\Product.xlsx";
                    File.WriteAllBytes(commandText, reportExcel);

                    OpenFile(commandText, reportExcel);
                }
            }
            catch
            {
                MessageBox.Show("Ошибка генерации отчета");
            }
        }

        private void OpenFile(string path, byte[] reportExcel)
        {
            File.WriteAllBytes(path, reportExcel);
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = path;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }

        private void TTNDel(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show($"Удалить накладные?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo($@"{pathToFile}\Bill");

                    foreach (FileInfo file in dirInfo.GetFiles())
                    {
                        file.Delete();
                    }
                    MessageBox.Show("Данные удалены");
                }
            }
            catch
            {
                MessageBox.Show("Неизвестная ошибка");
            }
        }

        private void BillDel(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show($"Удалить чеки?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo($@"{pathToFile}\TTN");

                    foreach (FileInfo file in dirInfo.GetFiles())
                    {
                        file.Delete();
                    }
                    MessageBox.Show("Данные удалены");
                }
            }
            catch
            {
                MessageBox.Show("Неизвестная ошибка");
            }
        }
    }
}
