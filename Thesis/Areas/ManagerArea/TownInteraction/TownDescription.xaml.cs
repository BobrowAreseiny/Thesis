using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.TownInteraction
{
    /// <summary>
    /// Логика взаимодействия для TownDescription.xaml
    /// </summary>
    public partial class TownDescription : Page
    {
        private readonly Town _selectedTown;
        public TownDescription()
        {
            InitializeComponent();
        }

        public TownDescription(Town town)
        {
            InitializeComponent();
            _selectedTown = town;
        }

        private void Data(Town town)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                if (_town.Items.Count != 0)
                {
                    _town.Items.Clear();
                }
                _town.Items.Add(town);
                _addresses.ItemsSource = _context.Address
                    .Where(x => x.TownId == town.Id)
                    .ToList();
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {

        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            Town selectedTown = (sender as Button).DataContext as Town;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Town town = _context.Town.Where(x => x.Id == selectedTown.Id).FirstOrDefault();
                _context.Town.Remove(town);
                _context.SaveChanges();
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {

        }



        private void TownInfo(object sender, SelectionChangedEventArgs e)
        {
            if (_town.SelectedItem is Town selectedTown)
            {
                NavigationService.Navigate(new TownEdit(selectedTown));
            }
        }

        private void AddressInfo(object sender, SelectionChangedEventArgs e)
        {
            if (_town.SelectedItem is Town selectedTown)
            {
                //NavigationService.Navigate(new AddressEdit(selectedTown));// Допилить изменение адреса
            }
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            Data(_selectedTown);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
