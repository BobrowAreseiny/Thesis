using System.Windows;
using System.Windows.Controls;
using Thesis.Areas.ManagerArea.TableDescription;
using Thesis.Data.Model;
using System.Data.Entity;
using System.Linq;
using Thesis.Areas.ManagerArea.TableEditor;

namespace Thesis.Areas.ManagerArea.Table
{
    /// <summary>
    /// Логика взаимодействия для RoleTablePage.xaml
    /// </summary>
    public partial class RoleTablePage : Page
    {
        private readonly Window _window;
      
        public RoleTablePage(MainManagerWindow window)
        {
            InitializeComponent();
            _window = window;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _roles.ItemsSource = _context.UserRole
                    .ToList();
            }
        }

        public RoleTablePage(RoleDescription window, int roleId)
        {
            InitializeComponent();
            _window = window;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _roles.ItemsSource = _context.UserRole
                    .Where(x => x.Id == roleId)
                    .ToList();
            }
        }

        private void RoleDescription(object sender, SelectionChangedEventArgs e)
        {
            UserRole selectedRole = (UserRole)_roles.SelectedItem;
            if (selectedRole != null)
            {
                new RoleDescription(selectedRole.Id).Show();
                _window.Close();              
            } 
        }
    }
}
