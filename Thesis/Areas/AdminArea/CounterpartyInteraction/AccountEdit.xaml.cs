using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Data.Model;
using TicketBooking;

namespace Thesis.Areas.AdminArea.CounterpartyInteraction
{
    /// <summary>
    /// Логика взаимодействия для AccountEdit.xaml
    /// </summary>
    public partial class AccountEdit : Page
    {
        private readonly ApplicationUser _user = new ApplicationUser();
        private int _counterpartyId;

        public AccountEdit(int counterpartyId)
        {
            InitializeComponent();
            _counterpartyId = counterpartyId;
        }

        public AccountEdit(ApplicationUser user)
        {
            InitializeComponent();
            if (user != null)
            {
                _counterpartyId = (int)user.CounterpartyId;
                _user = user;
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDbContent _context = new ApplicationDbContent())
                {
                    int? roleId = (_role.SelectedItem as UserRole).Id;
                    _user.Counterparty = _context.Counterparty
                                .Where(x => x.Id == _counterpartyId)
                                .FirstOrDefault();
                    _user.UserRole = _context.UserRole
                        .Where(x => x.Id == roleId)
                        .FirstOrDefault();

                    if (_user.Id == 0)
                    {
                        _context.ApplicationUser.Add(_user);
                    }
                    else
                    {
                        if (roleId != null)
                        {
                            ApplicationUser data = _context.ApplicationUser
                                .Where(f => f.Id == _user.Id)
                                .Include(x => x.Counterparty)
                                .Include(x => x.UserRole)
                                .FirstOrDefault();

                            if (data != null)
                            {
                                data.Email = _user.Email;
                                data.UserPassword = Crypto.Hash(_user.UserPassword);
                                data.Counterparty = _user.Counterparty;
                                data.UserRole = _user.UserRole;
                            }
                            _context.Entry(data).State = EntityState.Modified;
                        }
                    }
                    _context.SaveChanges();
                    NavigationService.GoBack();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка!");
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Data(ApplicationUser user)
        {
            DataContext = user;
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());

                _role.ItemsSource = _context.UserRole.ToList();
                if (user != null)
                {
                    _role.SelectedItem = _context.UserRole
                        .Where(x => x.Id == _user.UserRoleId)
                        .FirstOrDefault();
                }
            }
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(_user);
            }
        }
    }
}
