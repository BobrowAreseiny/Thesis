using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis.Data.Model;
using TicketBooking;

namespace Thesis.Areas.ManagerArea.CounterpartyInteraction
{
    /// <summary>
    /// Логика взаимодействия для AccountEdit.xaml
    /// </summary>
    public partial class AccountEdit : Page
    {
        private readonly ApplicationUser _user = new ApplicationUser();

        public AccountEdit()
        {
            InitializeComponent();
        }

        public AccountEdit(ApplicationUser user)
        {
            InitializeComponent();
            user.UserPassword = user.UserPassword;
            _user = user;
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
                    int? roleId = (_role.SelectedItem as UserRole).Id;
                    //int? counterpartyId = (_counterparty.SelectedItem as Counterparty).Id;
                    if (roleId != null /*&& counterpartyId != null*/)
                    {
                        //_user.Counterparty = _context.Counterparty
                        //    .Where(x => x.Id == counterpartyId)
                        //    .FirstOrDefault();
                        _user.UserRole = _context.UserRole
                            .Where(x => x.Id == roleId)
                            .FirstOrDefault();

                        ApplicationUser data = _context.ApplicationUser
                            .Where(f => f.Id == _user.Id)
                            .Include(x => x.Counterparty)
                            .Include(x => x.UserRole)
                            .FirstOrDefault();

                        if (data != null)
                        {
                            data.Email = _user.Email;
                            data.UserPassword = Crypto.Hash(_user.UserPassword);
                            //data.Counterparty = _user.Counterparty;
                            data.UserRole = _user.UserRole;
                        }
                        _context.Entry(data).State = EntityState.Modified;
                        _context.SaveChanges();
                        NavigationService.GoBack();
                    }
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Data(ApplicationUser user)
        {
            DataContext = user;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());

                //_counterparty.ItemsSource = _context.Counterparty.ToList();
                _role.ItemsSource = _context.UserRole.ToList();
                if (user != null)
                {
                    //_counterparty.SelectedItem = _context.Counterparty
                    //    .Where(x => x.Id == _user.CounterpartyId)
                    //    .FirstOrDefault();                    
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
