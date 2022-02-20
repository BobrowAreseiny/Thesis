using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data.Model;
using System.Windows;
using System.Collections.Generic;
using System;

namespace Thesis.Areas.AdminArea.CounterpartyInteraction
{
    /// <summary>
    /// Логика взаимодействия для Counterparties.xaml
    /// </summary>
    public partial class Counterparties : Page
    {
        private readonly int? roleId = null;
        private List<Counterparty> _data = new List<Counterparty>();

        public Counterparties()
        {
            InitializeComponent();
        }

        public Counterparties(int Id)
        {
            InitializeComponent();
            roleId = Id;
        }

        private void Data(int? roleId)
        {
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _data = roleId != null
                    ? _context.Counterparty.Where(x => x.Id == roleId).ToList()
                    : _context.Counterparty.ToList();
                if (_data != null)
                {
                    _counetparties.ItemsSource = null;
                    _counetparties.ItemsSource = _data;
                }
            }
        }

        private void CounterpartyDescription(object sender, SelectionChangedEventArgs e)
        {
            if (_counetparties.SelectedItem is Counterparty user)
            {
                NavigationService.Navigate(new CounterpartyDescription(user));
            }
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(roleId);
            }
        }

        private void NameSort(object sender, RoutedEventArgs e)
        {
            List<Counterparty> data = _data;
            if (_name.IsChecked == true)
            {
                data = data.OrderBy(x => x.Name).ToList();
                _counetparties.ItemsSource = null;
                _counetparties.ItemsSource = data;
            }
            else
            {
                _counetparties.ItemsSource = null;
                _counetparties.ItemsSource = _data;
            }
        }

        private void AddCounterparty(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CounterpartyEdit());
        }

        private void DeleteMaterial(object sender, RoutedEventArgs e)
        {
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                int counterpartyId = (int)((Button)sender).Content;
                Counterparty selectedData = _context.Counterparty.Where(x => x.Id == counterpartyId).FirstOrDefault();
                _context.Counterparty.Remove(selectedData);
                _context.SaveChanges();
            }
            Data(roleId);
        }

        private void Sherch(object sender, TextChangedEventArgs e)
        {
            _name.IsChecked = false;
            List<Counterparty> data = _data
                .Where(x => x.Name.ToLower().Contains(textSearch.Text.ToLower()))
                .ToList();
            if (data != null)
            {
                _counetparties.ItemsSource = null;
                _counetparties.ItemsSource = data;
            }
        }
    }
}
