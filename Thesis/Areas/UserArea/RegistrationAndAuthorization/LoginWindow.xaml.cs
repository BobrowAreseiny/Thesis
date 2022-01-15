using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Thesis.Data.Model;
using TicketBooking;

namespace Thesis.Areas.UserArea.RegistrationAndAuthorization
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly List<Basket> _basket;

        public LoginWindow(List<Basket> basket)
        {
            InitializeComponent();
            _basket = basket;
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            string passwordHash = Crypto.Hash(password.Password);
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                ApplicationUser user = _context.ApplicationUser
                    .Where(x => x.Email == email.Text && x.UserPassword == passwordHash)
                    .FirstOrDefault();

                if (user != null)
                {
                    new MainWindow(user, _basket).Show();
                    Close();
                }
                else
                {
                    email.BorderBrush = Brushes.Red;
                    password.BorderBrush = Brushes.Red;
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            new CreateAccountWindow(_basket).Show();
            Close();
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
