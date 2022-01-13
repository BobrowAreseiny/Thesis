using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.RoleInteraction
{
    /// <summary>
    /// Логика взаимодействия для Roles.xaml
    /// </summary>
    public partial class Roles : Page
    {
        public Roles()
        {
            InitializeComponent();
        }

        private void Data()
        {
            List<UserRole> roles = new List<UserRole>();

            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                roles = _context.UserRole.ToList();
            }
            if (roles != null)
            {
                _roles.ItemsSource = roles;
            }
        }

        private void RoleDescription(object sender, SelectionChangedEventArgs e)
        {
            if (_roles.SelectedItem is UserRole selectedRole)
            {
                NavigationService.Navigate(new RoleDescription(selectedRole));
            }
        }

        private void UpdateData(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Data();
        }
    }
}
