using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.TownInteraction
{
    /// <summary>
    /// Логика взаимодействия для TownDescription.xaml
    /// </summary>
    public partial class TownDescription : Page
    {
        private readonly Town _selectedTown;
        private List<Address> _address;

        public TownDescription()
        {
            InitializeComponent();
        }

        public TownDescription(Town town)
        {
            InitializeComponent();
            _selectedTown = town;
        }

        private void Data(Town town)
        {
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                if (_town.Items.Count != 0)
                {
                    _town.Items.Clear();
                }
                _town.Items.Add(town);
                _address = _context.Address
                    .Where(x => x.TownId == town.Id)
                    .ToList();
                if (_address != null)
                {
                    _addresses.ItemsSource = _address;
                }
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddressEdit());
        }


        private void TownInfo(object sender, SelectionChangedEventArgs e)
        {
            if (_town.SelectedItem is Town selectedTown)
            {
                NavigationService.Navigate(new TownEdit(selectedTown));
            }
        }

        private void AddressInfo(object sender, SelectionChangedEventArgs e)
        {
            if (_addresses.SelectedItem is Address selectedAddress)
            {
                NavigationService.Navigate(new AddressEdit(selectedAddress));
            }
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            Data(_selectedTown);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Sherch(object sender, TextChangedEventArgs e)
        {
            List<Address> data = _address
                .Where(x => x.Street.ToLower().Contains(textSearch.Text.ToLower()))
                .ToList();
            if (data != null)
            {
                DataSort(data);
            }
        }

        private void DataSort(List<Address> data)
        {
            _addresses.ItemsSource = null;
            _addresses.ItemsSource = data;
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            int addressId = (int)(sender as Button).Content;
            if (addressId != 0)
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
                {
                    Address data = _context.Address.Where(x => x.Id == addressId).FirstOrDefault();
                    _context.Entry(data).State = EntityState.Deleted;
                    _context.SaveChanges();
                    Data(_selectedTown);
                }
            }
        }
    }
}
