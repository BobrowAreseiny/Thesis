using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Areas.ManagerArea.TableDescription;
using Thesis.Areas.ManagerArea.TableEditor;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.Table
{
    /// <summary>
    /// Логика взаимодействия для RoleChangerTablePage.xaml
    /// </summary>
    public partial class RoleChangerTablePage : Page
    {
        public RoleChangerTablePage(RoleDescription window, int roleId)
        {
            InitializeComponent();
            _window = window;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _roles.ItemsSource = _context.ApplicationUser
                    .Where(x => x.UserRoleId == roleId)
                    .ToList();
            }
        }
        private readonly Window _window;

        public RoleChangerTablePage(MainManagerWindow window)
        {
            InitializeComponent();
            _window = window;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _roles.ItemsSource = _context.ApplicationUser.ToList();
            }
        }

        private void RoleDescription(object sender, RoutedEventArgs e)
        {
            if (_roles.SelectedItem is ApplicationUser user)
            {
                new AddUser(user).Show();
                if (_window != null)
                {
                    _window.Close();
                }
            }
        }    
    }
}
