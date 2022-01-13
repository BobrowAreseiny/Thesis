using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Thesis.Areas.ManagerArea.TableDescription;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.TableEditor
{
    /// <summary>
    /// Логика взаимодействия для AddCounterparty.xaml
    /// </summary>
    public partial class AddCounterparty : Window
    {
        private readonly Counterparty _counterparty;

        public AddCounterparty()
        {
            InitializeComponent();
        }

        public AddCounterparty(Counterparty counterparty)
        {
            InitializeComponent();
            if (counterparty != null)
            {
                _counterparty = counterparty;
            }
            DataContext = _counterparty;
            Data(_counterparty);
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

                ApplicationUser user = _context.ApplicationUser
                    .Where(x => x.CounterpartyId == _counterparty.Id)
                    .FirstOrDefault();

                if (user != null)
                {
                   // new CounterpartyDescription(user.Id).Show();
                    Close();
                }
            }
        } 

        private void Data(Counterparty counterparty)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _street.ItemsSource = _context.Address
                    .ToList();
                _street.SelectedItem = _context.Address
                    .Where(x => x.Id == counterparty.AddressId)
                    .FirstOrDefault();
            }
        }


        private void Exit(object sender, RoutedEventArgs e)
        {
           // new CounterpartyDescription(_counterparty.Id).Show();
            Close();
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
