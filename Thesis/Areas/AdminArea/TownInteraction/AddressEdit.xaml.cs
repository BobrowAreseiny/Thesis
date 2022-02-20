using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.TownInteraction
{
    /// <summary>
    /// Логика взаимодействия для AddressEdit.xaml
    /// </summary>
    public partial class AddressEdit : Page
    {
        private readonly Address _address = new Address();
        public AddressEdit()
        {
            InitializeComponent();
            DataContext = _address;
            Data(_address);
        }

        public AddressEdit(Address _selectedAddress)
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
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _address.Town = _context.Town
                    .Where(x => x.Id == ((Town)_towns.SelectedItem).Id)
                    .FirstOrDefault();
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

                    if (data != null)
                    {
                        data.RoomNumber = _address.RoomNumber;
                        data.Street = _address.Street;
                        data.BuildingNumber = _address.BuildingNumber;
                        data.AddressIndex = _address.AddressIndex;
                        data.Town = _address.Town;
                    }
                    _context.Entry(data).State = EntityState.Modified;
                }
                _context.SaveChanges();
                NavigationService.GoBack();
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Data(Address _address)
        {
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _towns.ItemsSource = _context.Town.ToList();
                if (_address.Id != 0)
                {
                    _towns.SelectedItem = _context.Town
                        .Where(x => x.Id == _address.TownId)
                        .FirstOrDefault();
                }
            }
        }       
    }
}
