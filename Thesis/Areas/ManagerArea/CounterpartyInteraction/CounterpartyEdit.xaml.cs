using System.Windows;
using System.Data.Entity;
using System.Windows.Controls;
using Thesis.Data.Model;
using System.Linq;

namespace Thesis.Areas.ManagerArea.CounterpartyInteraction
{
    /// <summary>
    /// Логика взаимодействия для CounterpartyEdit.xaml
    /// </summary>
    public partial class CounterpartyEdit : Page
    {
        private readonly Counterparty _counterparty;

        public CounterpartyEdit()
        {
            InitializeComponent();
        }

        public CounterpartyEdit(Counterparty counterparty)
        {
            InitializeComponent();
            if (counterparty != null)
            {
                _counterparty = counterparty;
            }
            Data(_counterparty.Id);
            DataContext = _counterparty;            
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (_counterparty.Id == 0)
                {
                    _context.Counterparty.Add(_counterparty);
                }

                Counterparty selectedCounterparty = _context.Counterparty
                    .Include(x => x.Address)
                    .Where(f => f.Id == _counterparty.Id)
                    .FirstOrDefault();
                _counterparty.Address = _context.Address
                     .Where(x => x.Id == ((Address)_street.SelectedItem).Id)
                     .FirstOrDefault();

                if (selectedCounterparty != null)
                {
                    selectedCounterparty.Name = _counterparty.Name;
                    selectedCounterparty.Telephone = _counterparty.Telephone;
                    selectedCounterparty.ContactPerson = _counterparty.ContactPerson;
                    selectedCounterparty.Address = _counterparty.Address;
                }
                _context.Entry(selectedCounterparty).State = EntityState.Modified;
                _context.SaveChanges();

                NavigationService.GoBack();
            }
        }

        private void Data(int counterpartyId)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                Counterparty counterparty = _context.Counterparty
                    .Where(x => x.Id == counterpartyId)
                    .FirstOrDefault();
                _street.ItemsSource = _context.Address
                    .ToList();
                _street.SelectedItem = _context.Address
                    .Where(x => x.Id == counterparty.AddressId)
                    .FirstOrDefault();
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
