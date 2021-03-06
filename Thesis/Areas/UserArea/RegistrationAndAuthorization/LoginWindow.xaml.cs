using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Thesis.Areas.ManagerArea;
using Thesis.Areas.MarketingArea;
using Thesis.Areas.UserArea.ProductsWindows;
using Thesis.Data.Model;
using TicketBooking;

namespace Thesis.Areas.UserArea.RegistrationAndAuthorization
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            string passwordHash = Crypto.Hash(password.Password);
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                ApplicationUser user = _context.ApplicationUser
                    .Where(x => x.Email == email.Text && x.UserPassword == passwordHash)
                    .FirstOrDefault();

                if (user != null)
                {
                    if (user.UserRole.RoleName == "Администратор")
                    {
                        new MainManagerWindow().Show();
                        Close();
                    }
                    else if (user.UserRole.RoleName == "Работник")
                    {
                        new MainMarketingWindow().Show();
                        Close();
                    }
                    else if (user.UserRole.RoleName == "Пользователь")
                    {
                        new ProductsCatalog(user).Show();
                        Close();
                    }
                }
                else
                {
                    email.BorderBrush = Brushes.Red;
                    password.BorderBrush = Brushes.Red;
                    password.Clear();
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            new CreateAccountWindow().Show();
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
