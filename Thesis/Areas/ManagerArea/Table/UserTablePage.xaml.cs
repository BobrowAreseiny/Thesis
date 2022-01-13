using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Areas.ManagerArea.TableDescription;
using Thesis.Areas.ManagerArea.TableEditor;
using Thesis.Data.Model;
using System.Data.Entity;

namespace Thesis.Areas.ManagerArea.Table
{
    /// <summary>
    /// Логика взаимодействия для UserTablePagexaml.xaml
    /// </summary>
    public partial class UserTablePage : Page
    {
        private readonly Window _window;

        public UserTablePage(MainManagerWindow window)
        {
            InitializeComponent();
            _window = window;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _users.ItemsSource = _context.Counterparty
                    .Include(x => x.Address)
                    .Include(x => x.UserOrder)
                    .Include(x => x.ApplicationUser)
                    .ToList();
            }
        }

        public UserTablePage(Window window, int roleId)
        {
            InitializeComponent();
            _window = window;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _users.ItemsSource = _context.Counterparty
                    .Where(x => x.Id == roleId)
                    .ToList();
            }
        }

        private void UserOrderDescription(object sender, SelectionChangedEventArgs e)
        {
            if (_users.SelectedItem is Counterparty user)
            {
                new CounterpartyDescription(user).Show();
            }
            //    int? CounterpartyId = ((sender as Button).DataContext as ApplicationUser).CounterpartyId;
            //if (CounterpartyId != null)
            //{
            //    NavigationService.Navigate(new CounterpartyDescription((int)CounterpartyId));
            //}
        }
    }
}
