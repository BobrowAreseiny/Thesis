using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Data.Model;
using System.Data.Entity;
using Thesis.Areas.AdminArea.CounterpartyInteraction;

namespace Thesis.Areas.AdminArea.RoleInteraction
{
    /// <summary>
    /// Логика взаимодействия для RoleDescription.xaml
    /// </summary>
    public partial class RoleDescription : Page
    {
        private readonly int roleId;

        public RoleDescription(UserRole role)
        {
            InitializeComponent();
            roleId = role.Id;
        }

        private void Add(object sender, RoutedEventArgs e)
        {

        }

        private void Edit(object sender, RoutedEventArgs e)
        {

        }

        private void Delete(object sender, RoutedEventArgs e)
        {

        }

        private void Data(int roleId)
        {
            UserRole roles = new UserRole();
            List<ApplicationUser> users = new List<ApplicationUser>();

            using (ApplicationDbContent _context = new ApplicationDbContent())
            { 
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                if(_role.Items.Count != 0)
                {
                    _role.Items.Clear();
                }
                roles = _context.UserRole
                    .Where(x => x.Id == roleId)
                    .FirstOrDefault();
                users = _context.ApplicationUser
                    .Where(x => x.UserRoleId == roleId)
                    .Include(x =>x.UserRole)
                    .Include(x =>x.Counterparty)
                    .ToList();
            }
            if (roles != null && users != null)
            {
                _usersList.ItemsSource = users;
                _role.Items.Add(roles);
            }
        }

        private void CounterpartyDescription(object sender, SelectionChangedEventArgs e)
        {
            if (_usersList.SelectedItem is ApplicationUser user)
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
                {
                    NavigationService.Navigate(new AccountEdit(user));
                }
            }
        }

        private void RoleEdit(object sender, SelectionChangedEventArgs e)
        {
            if (_role.SelectedItem is UserRole role)
            {
                NavigationService.Navigate(new RoleEdit(role));
            }
        }
        
        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        
        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(roleId);
            }
        }

    }
}
