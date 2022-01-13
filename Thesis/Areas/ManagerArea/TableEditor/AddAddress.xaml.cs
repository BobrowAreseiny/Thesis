using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Thesis.Areas.ManagerArea.TableDescription;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.TableEditor
{
    /// <summary>
    /// Логика взаимодействия для AddAddress.xaml
    /// </summary>
    public partial class AddAddress : Window
    {
        private readonly Address _address = new Address();
        public AddAddress()
        {
            InitializeComponent();
            DataContext = _address;
        }

        public AddAddress(Address _selectedAddress)
        {
            InitializeComponent();
            try
            {
                if (_selectedAddress != null)
                {
                    _address = _selectedAddress;
                }
                DataContext = _address;
                Data(_address);
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
                if (_address.Id == 0)
                {
                    _context.Address.Add(_address);
                }
                else
                {
                    Address data = _context.Address
                        .Where(f => f.Id == _address.Id)
                        .Include(x => x.Town)
                        .FirstOrDefault();

                    _address.Town = _context.Town
                        .Where(x => x.Id == ((Town)_towns.SelectedItem).Id)
                        .FirstOrDefault();

                    if (data != null)
                    {
                        data.RoomNumber = _address.RoomNumber;
                        data.Street = _address.Street;
                        data.BuildingNumber = _address.BuildingNumber;
                        data.AddressIndex = _address.AddressIndex;
                        data.Town = _address.Town;
                    }
                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }

            new TownDescription().Show();
            Close();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            new TownDescription().Show();
            Close();
        }

        private void Data(Address _address)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _towns.ItemsSource = _context.Town.ToList();

                _towns.SelectedItem = _context.Town
                    .Where(x => x.Id == _address.TownId)
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
