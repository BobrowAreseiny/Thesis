using System.Linq;
using System.Windows;
using Thesis.Data.Model;
using System.Data.Entity;
using System.Windows.Controls;
using Thesis.Areas.ManagerArea.TableEditor;

namespace Thesis.Areas.ManagerArea.TableDescription
{
    /// <summary>
    /// Логика взаимодействия для UserDescription.xaml
    /// </summary>
    public partial class UserDescription : Window
    {
        public UserDescription(int userId)
        {
            InitializeComponent();
            Data(userId);
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            //new EditOrderConstruction(null, _data).Show();
            Close();
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            //new EditOrderConstruction((sender as Button).DataContext as OrderConstruction, _data).Show();
            Close();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {

        }

        private void Data(int userId)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                ApplicationUser user = _context.ApplicationUser
                    .Include(x => x.Counterparty)
                    .Include(x => x.UserRole)
                    .Where(x => x.Id == userId)
                    .FirstOrDefault();
                _users.Items.Add(user);
            }
        }

        private void UserInformation(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is ApplicationUser user)
            {
                new AddUser(user).Show();
                Close();
            }
        }

        private void UserRoleInformation(object sender, RoutedEventArgs e)
        {
            int? userRoleId = ((sender as Button).DataContext as ApplicationUser).UserRoleId;
            if (userRoleId != null)
            {
                new RoleDescription(userRoleId).Show();
                Close();
            }
        }

        private void CounterpartyInformation(object sender, RoutedEventArgs e)
        {
            int? CounterpartyId = ((sender as Button).DataContext as ApplicationUser).CounterpartyId;
            if (CounterpartyId != null)
            {
                //new CounterpartyDescription((int)CounterpartyId).Show();
                Close();
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OrderCatalog(object sender, RoutedEventArgs e)
        {
            new MainManagerWindow().Show();
            Close();
        }
    }
}
