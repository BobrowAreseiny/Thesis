using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Areas.AdminArea;
using Thesis.Areas.ManagerArea.Table;
using Thesis.Areas.ManagerArea.TableEditor;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.TableDescription
{
    public partial class TownDescription : Window
    {
        public TownDescription(int townId)
        {
            InitializeComponent();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Town town = _context.Town
                    .Where(x => x.Id == townId)
                    .FirstOrDefault();

                _towns.Items.Add(town);
            }
            Data();
        }

        public TownDescription()
        {
            InitializeComponent();
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                Town town = _context.Town
                     .FirstOrDefault();
                _towns.Items.Add(town);
            }
            Data();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            new AddTown().Show();
        }
        
        private void Data()
        {
            _addresses.Content = new AddressTablePage(this);
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
            new AddAddress((sender as Button).DataContext as Address).Show();
            Close();
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

        private void TownInfo(object sender, SelectionChangedEventArgs e)
        {
            Town selectedTown = (Town)_towns.SelectedItem;
            if (selectedTown != null)
            {
                new AddTown(selectedTown).Show();
                Close();
            }
        }
    }
}