using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.UserOrderInteraction
{
    /// <summary>
    /// Логика взаимодействия для OrderConstructionEdit.xaml
    /// </summary>
    public partial class OrderConstructionEdit : Page
    {
        private OrderConstruction _construction = new OrderConstruction();

        public OrderConstructionEdit()
        {
            InitializeComponent();
            DataContext = _construction;
            Data(_construction);
        }

        public OrderConstructionEdit(OrderConstruction _selectedConstruction)
        {
            InitializeComponent();
            try
            {
                if (_selectedConstruction != null)
                {
                    _construction = _selectedConstruction;
                }
                DataContext = _construction;
                Data(_construction);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _construction.ProductSize = _context.ProductSize
                        .Where(x => x.Id == ((ProductSize)_productSize.SelectedItem).Id)
                        .FirstOrDefault();
                _construction.UserOrder = _context.UserOrder
                    .Where(x => x.Id == ((UserOrder)_userOrder.SelectedItem).Id)
                    .FirstOrDefault();

                if (_construction.Id == 0)
                {
                    _context.OrderConstruction.Add(_construction);
                }
                else
                {
                    OrderConstruction data = _context.OrderConstruction
                        .Where(f => f.Id == _construction.Id)
                        .Include(x => x.ProductSize)
                        .Include(x => x.UserOrder)
                        .FirstOrDefault();

                    if (data != null)
                    {
                        data.Price = _construction.Price;
                        data.Status = _construction.Status;
                        data.NumberOfProucts = _construction.NumberOfProucts;
                        data.Amount = _construction.Amount;
                        data.ProductSize = _construction.ProductSize;
                        data.UserOrder = _construction.UserOrder;
                    }
                    _context.Entry(data).State = EntityState.Modified;
                }
                _context.SaveChanges();
            }
            NavigationService.GoBack();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Data(OrderConstruction construction)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _productSize.ItemsSource = _construction.Id != 0
                    ? _context.ProductSize.Where(x => x.ProductId == construction.ProductSize.ProductId).ToList()
                    : _context.ProductSize.ToList();
                _userOrder.ItemsSource = _context.UserOrder.ToList();

                if (_construction.Id != 0)
                {
                    _userOrder.SelectedItem = _context.UserOrder
                        .Where(x => x.Id == construction.UserOrderId)
                        .FirstOrDefault();
                    _productSize.SelectedItem = _context.ProductSize
                        .Where(x => x.Id == construction.ProductSizeId)
                        .FirstOrDefault();
                }
            }
        }
    }
}
