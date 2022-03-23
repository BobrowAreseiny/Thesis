using System.Windows;
using System.Data.Entity;
using System.Windows.Controls;
using Thesis.Data.Model;
using System.Linq;
using System;

namespace Thesis.Areas.AdminArea.CounterpartyInteraction
{
    /// <summary>
    /// Логика взаимодействия для CounterpartyEdit.xaml
    /// </summary>
    public partial class CounterpartyEdit : Page
    {
        private Counterparty _counterparty = new Counterparty();

        public CounterpartyEdit()
        {
            InitializeComponent();
            DataContext = _counterparty;
        }

        public CounterpartyEdit(Counterparty counterparty)
        {
            InitializeComponent();
            if (counterparty != null)
            {
                _counterparty = counterparty;
            }
            DataContext = _counterparty;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
                {
                    _counterparty.Address = _context.Address
                         .Where(x => x.Id == ((Address)_street.SelectedItem).Id)
                         .FirstOrDefault();
                    if (_counterparty.Id == 0)
                    {
                        _context.Counterparty.Add(_counterparty);
                    }
                    else
                    {
                        Counterparty selectedCounterparty = _context.Counterparty
                            .Include(x => x.Address)
                            .Where(f => f.Id == _counterparty.Id)
                            .FirstOrDefault();
                        if (selectedCounterparty != null)
                        {
                            selectedCounterparty.Name = _counterparty.Name;
                            selectedCounterparty.Telephone = _counterparty.Telephone;
                            selectedCounterparty.ContactPerson = _counterparty.ContactPerson;
                            selectedCounterparty.Address = _counterparty.Address;
                        }
                        _context.Entry(selectedCounterparty).State = EntityState.Modified;
                    }
                    _context.SaveChanges();

                    NavigationService.GoBack();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка!");
            }
        }

        private void Data(int? counterpartyId)
        {
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                Counterparty counterparty = _context.Counterparty
                    .Where(x => x.Id == counterpartyId)
                    .FirstOrDefault();
                _street.ItemsSource = _context.Address
                    .ToList();
                if (counterparty != null)
                {
                    _street.SelectedItem = _context.Address
                        .Where(x => x.Id == counterparty.AddressId)
                        .FirstOrDefault();
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(_counterparty.Id);
            }
        }
    }
}
