using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Thesis.Areas.ManagerArea.TableDescription;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea
{
    /// <summary>
    /// Логика взаимодействия для AddOrderConstruction.xaml
    /// </summary>
    public partial class AddOrderConstruction : Window
    {
        private readonly OrderConstruction _construction = new OrderConstruction();
        private readonly OrderData _data;

        public AddOrderConstruction()
        {
            InitializeComponent();
            DataContext = _construction;
        }

        public AddOrderConstruction(OrderConstruction _selectedConstruction, OrderData data)
        {
            InitializeComponent();
            _data = data;
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
                if (_construction.Id == 0)
                {
                    _context.OrderConstruction.Add(_construction);
                }
                else
                {
                    _construction.ProductSize = _context.ProductSize
                        .Where(x => x.Id == ((ProductSize)_productSize.SelectedItem).Id)
                        .FirstOrDefault();

                    _construction.UserOrder = _context.UserOrder
                        .Where(x => x.Id == ((UserOrder)_userOrder.SelectedItem).Id)
                        .FirstOrDefault();

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
                    _context.SaveChanges();

                    _data.OrderConstructionCount = _context.OrderConstruction
                         .Where(x => x.UserOrderId == _construction.UserOrderId)
                         .Count();
                    _data.OrderConstructionMade = _context.OrderConstruction
                        .Where(x => x.UserOrderId == _construction.UserOrderId && x.Status == "Готов")
                        .Count();
                    if (_data.OrderConstructionMade == _data.OrderConstructionCount)
                    {
                        _data.Status = "Готов";
                    }
                }
            }
            new UserOrderDescription(_data).Show();
            Close();
        } 

        private void Exit(object sender, RoutedEventArgs e)
        {
            new UserOrderDescription(_data).Show();
            Close();
        }

        private void Data(OrderConstruction construction)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _userOrder.ItemsSource = _context.UserOrder.ToList();
                _productSize.ItemsSource = _context.ProductSize
                    .Where(x => x.ProductId == construction.ProductSize.ProductId)
                    .ToList();

                _userOrder.SelectedItem = _context.UserOrder
                    .Where(x => x.Id == construction.UserOrderId)
                    .FirstOrDefault();
                _productSize.SelectedItem = _context.ProductSize
                    .Where(x => x.Id == construction.ProductSizeId)
                    .FirstOrDefault();
            }
        }

        private void Pull(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
