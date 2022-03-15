using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            try
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
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
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка!");
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Data(OrderConstruction construction)
        {
            try
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
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
                    else
                    {
                        OrderConstruction orderConstructionNumber = _context.OrderConstruction
                            .OrderBy(x => x.Id)
                            .Skip(_context.OrderConstruction.Count() - 1)
                            .FirstOrDefault();

                        _construction.Status = "Ожидание";
                        _construction.NumberOfProucts = orderConstructionNumber.Id + 1;
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка!");
            }
        }
    }

    [ValueConversion(typeof(decimal), typeof(string))]
    public class CostConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return ((decimal)value).ToString("#,###", culture) + " руб.";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            decimal result;
            if (decimal.TryParse(value.ToString(), System.Globalization.NumberStyles.Any,
                         culture, out result))
            {
                return result;
            }
            else if (decimal.TryParse(value.ToString().Replace(" руб.", ""), System.Globalization.NumberStyles.Any,
                         culture, out result))
            {
                return result;
            }
            return value;
        }
    }

    [ValueConversion(typeof(decimal), typeof(string))]
    public class CountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return ((decimal)value).ToString("#,###", culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            decimal result;
            try
            {
                if (decimal.TryParse(value.ToString(), System.Globalization.NumberStyles.Any,
                             culture, out result))
                {
                    return result;
                }
                else if (decimal.TryParse(value.ToString().Replace("", ""), System.Globalization.NumberStyles.Any,
                             culture, out result))
                {
                    return result;
                }
            }
            catch
            {

            }
            return value;
        }
    }
}

