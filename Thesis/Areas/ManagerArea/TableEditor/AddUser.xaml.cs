using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.TableEditor
{
    /// <summary>
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        private readonly ApplicationUser _user = new ApplicationUser();

        public AddUser()
        {
            InitializeComponent();
            DataContext = _user;
        }

        public AddUser(ApplicationUser _selectedUser)
        {
            InitializeComponent();

            try
            {
                if (_selectedUser != null)
                {
                    _user = _selectedUser;
                }
                DataContext = _user;
                Data(_user);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (_user.Id == 0)
                {
                    _context.ApplicationUser.Add(_user);
                }
                else
                {
                    _user.UserRole = _context.UserRole
                        .Where(x => x.Id == ((UserRole)_roles.SelectedItem).Id)
                        .FirstOrDefault();
                    _user.Counterparty = _context.Counterparty
                        .Where(x => x.Id == ((Counterparty)_counterparty.SelectedItem).Id)
                        .FirstOrDefault();

                    ApplicationUser data = _context.ApplicationUser
                        .Where(f => f.Id == _user.Id)
                        .Include(x => x.UserRole)
                        .Include(x => x.Counterparty)
                        .FirstOrDefault();

                    if (data != null)
                    {
                        data.UserPassword = _user.UserPassword;
                        data.Email = _user.Email;
                        data.UserRole = _user.UserRole;
                        data.Counterparty = _user.Counterparty;
                    }
                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
           // new CounterpartyDescription(_user.Id).Show();
            Close();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
          //  new CounterpartyDescription(_user.Id).Show();
            Close();
        }

        private void Data(ApplicationUser user)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _counterparty.ItemsSource = _context.Counterparty.ToList();
                _counterparty.SelectedItem = _context.Counterparty
                    .Where(x => x.Id == user.CounterpartyId)
                    .FirstOrDefault();

                _roles.ItemsSource = _context.UserRole.ToList();
                _roles.SelectedItem = _context.UserRole
                    .Where(x => x.Id == user.UserRoleId)
                    .FirstOrDefault();
            }
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
