using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data.Model;
using System.Windows;

namespace Thesis.Areas.ManagerArea.CounterpartyInteraction
{
    /// <summary>
    /// Логика взаимодействия для Counterparties.xaml
    /// </summary>
    public partial class Counterparties : Page
    {
        private readonly int? roleId = null;
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
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _counetparties.ItemsSource = roleId != null
                    ? _context.Counterparty.Where(x => x.Id == roleId).ToList()
                    : _context.Counterparty.ToList();
            }
        }

        private void CounterpartyDescription(object sender, SelectionChangedEventArgs e)
        {
            if (_counetparties.SelectedItem is Counterparty user)
            {
                NavigationService.Navigate(new CounterpartyDescription(user));
            }
        }

        private void UpdateData(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(roleId);
            }
        }
    }
}
