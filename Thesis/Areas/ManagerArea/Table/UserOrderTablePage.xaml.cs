using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Areas.ManagerArea.TableDescription;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.Table
{
    /// <summary>
    /// Логика взаимодействия для UserOrderConstruction.xaml
    /// </summary>
    public partial class UserOrderTablePage : Page
    {
        private readonly Window _window;

        public UserOrderTablePage(MainManagerWindow window)
        {
            InitializeComponent();
            _window = window;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Data(null);
            }
        }
        public UserOrderTablePage(CounterpartyDescription window, int counterpartyId)
        {
            InitializeComponent();
            //_window = window;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Data(counterpartyId);
            }
        }

        private void Data(int? counterpartyId)
        {
            List<UserOrder> order = new List<UserOrder>();
           
            List<OrderData> data = new List<OrderData>();

            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                order = counterpartyId == null ? _context.UserOrder.ToList()
                : _context.UserOrder.Where(x => x.CounterpartyId == counterpartyId).ToList();
                if (order != null)
                {
                    foreach (UserOrder dataItem in order)
                    {
                        //OrderData item = new OrderData(dataItem.Id)
                        //{
                        //    OrderNumber = dataItem.OrderNumber,
                        //    Counterparty = dataItem.Counterparty.Name,
                        //    DateOfShipment = dataItem.DateOfShipment,
                        //    Status = dataItem.Status,
                        //    OrderConstructionCount = _context.OrderConstruction.Where(x => x.UserOrderId == dataItem.Id).Count(),
                        //    OrderConstructionMade = _context.OrderConstruction.Where(x => x.UserOrderId == dataItem.Id && x.Status == "Готов").Count(),
                        //};
                        //if (item.OrderConstructionMade == item.OrderConstructionCount)
                        //{
                        //    item.Status = "Готов";
                        //}
                       // data.Add(item);
                    }
                    if (data != null)
                    {
                        item.ItemsSource = data;
                    }
                }
            }
        }

        private void OrderDescription(object sender, SelectionChangedEventArgs e)
        {
            if (item.SelectedItem is OrderData dataItem)
            {
                new UserOrderDescription(dataItem).Show();
                if (_window != null)
                {
                    _window.Close();
                }
            }
        }
    }
}
