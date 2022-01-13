using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.TownInteraction
{
    /// <summary>
    /// Логика взаимодействия для TownEdit.xaml
    /// </summary>
    public partial class TownEdit : Page
    {
        private readonly Town _selectedTown = null;

        public TownEdit()
        {
            InitializeComponent();
        }

        public TownEdit(Town town)
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
                NavigationService.GoBack();
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
