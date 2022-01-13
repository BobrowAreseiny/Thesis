using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.TownInteraction
{
    /// <summary>
    /// Логика взаимодействия для Towns.xaml
    /// </summary>
    public partial class Towns : Page
    {
        public Towns()
        {
            InitializeComponent();
        }

        private void Data()
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _towns.ItemsSource = _context.Town.ToList();
            }
        }

        private void TownDescription(object sender, SelectionChangedEventArgs e)
        {
            if (_towns.SelectedItem is Town selectedTown)
            {
                NavigationService.Navigate(new TownDescription(selectedTown));
            }
        }

        private void UpdateData(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Data();
        }
    }
}
