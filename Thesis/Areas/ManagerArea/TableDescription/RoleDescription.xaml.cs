using System.Linq;
using System.Windows;
using Thesis.Areas.ManagerArea.Table;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.TableDescription
{
    /// <summary>
    /// Логика взаимодействия для RoleDescription.xaml
    /// </summary>
    public partial class RoleDescription : Window
    {
        public RoleDescription(int? userRoleId)
        {
            InitializeComponent();
            Data((int)userRoleId);
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            //new AddOrderConstruction(null).Show();
            //Close();
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            //List<OrderData> data = Order();
            // new AddOrderConstruction((sender as Button).DataContext as OrderConstruction, data).Show();
            //Close();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {

        }

        private void Data(int userRoleId)
        {
            UserRole roles = new UserRole();

            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                roles = _context.UserRole.Where(x => x.Id == userRoleId).FirstOrDefault();
            }
            if (roles != null)
            {

                // Поменять
                _rolesList.Content = new RoleChangerTablePage(this, roles.Id);
                item.Items.Add(roles);
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
