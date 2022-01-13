using System.Linq;
using System.Windows.Controls;
using Thesis.Areas.ManagerArea.TableDescription;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.Table
{
    /// <summary>
    /// Логика взаимодействия для TownTable.xaml
    /// </summary>
    public partial class TownTablePage : Page
    {
        private readonly MainManagerWindow _window;
        public TownTablePage(MainManagerWindow window)
        {
            InitializeComponent();
            _window = window;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _towns.ItemsSource = _context.Town.ToList();
            }
        }

        private void TownDescription(object sender, SelectionChangedEventArgs e)
        {
            Town selectedTown = (Town)_towns.SelectedItem;
            if (selectedTown != null)
            {
                new TownDescription(selectedTown.Id).Show();
                _window.Close();
            }
        }
    }
}
