using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Areas.ManagerArea.TableDescription;
using Thesis.Areas.ManagerArea.TableEditor;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.Table
{
    /// <summary>
    /// Логика взаимодействия для AddressTablePage.xaml
    /// </summary>
    public partial class AddressTablePage : Page
    {
        private readonly Window _window;

        public AddressTablePage(TownDescription window)
        {
            InitializeComponent();
            _window = window;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _addresses.ItemsSource = _context.Address.ToList();
            }
        }

        private void SelectedItem(object sender, SelectionChangedEventArgs e)
        {
            Address selectedAddress = (Address)_addresses.SelectedItem;
            if (selectedAddress != null)
            {
                new AddAddress(selectedAddress).Show();
                _window.Close();
            }
        }
    }
}
