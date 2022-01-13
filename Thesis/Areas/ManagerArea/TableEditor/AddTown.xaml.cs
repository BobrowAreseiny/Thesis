using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Thesis.Areas.ManagerArea.TableDescription;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea
{
    /// <summary>
    /// Логика взаимодействия для AddTown.xaml
    /// </summary>
    public partial class AddTown : Window
    {
        private readonly Town _selectedTown = new Town();

        public AddTown()
        {
            InitializeComponent();
            DataContext = _selectedTown;
        }

        public AddTown(Town town)
        {
            InitializeComponent();
            try
            {
                if (town != null)
                {
                    _selectedTown = town;
                }
                DataContext = _selectedTown;
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
                if (_selectedTown.Id == 0)
                {
                    _context.Town.Add(_selectedTown);
                }
                Town town = _context.Town.FirstOrDefault(f => f.Id == _selectedTown.Id);
                if (town != null)
                {
                    town.TownName = _selectedTown.TownName;
                }
                _context.SaveChanges(); 
                new TownDescription(town.Id).Show();
                Close();
            }

        }


        private void Exit(object sender, RoutedEventArgs e)
        {
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
