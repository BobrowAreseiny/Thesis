using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.RoleInteraction
{
    /// <summary>
    /// Логика взаимодействия для RoleEdit.xaml
    /// </summary>
    public partial class RoleEdit : Page
    {   
        private readonly UserRole _selectedRole = null;

        public RoleEdit()
        {
            InitializeComponent();
        }

        public RoleEdit(UserRole role)
        {
            InitializeComponent();
            try
            {
                if (role != null)
                {
                    _selectedRole = role;
                }
                DataContext = _selectedRole;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                if (_selectedRole.Id == 0)
                {
                    _context.UserRole.Add(_selectedRole);
                }
                UserRole role = _context.UserRole.FirstOrDefault(f => f.Id == _selectedRole.Id);
                if (role != null)
                {
                    role.RoleName = _selectedRole.RoleName;
                }
                _context.SaveChanges();
                NavigationService.GoBack();
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
