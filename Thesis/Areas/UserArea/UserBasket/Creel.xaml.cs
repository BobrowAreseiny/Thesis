using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Thesis.Data.Model;

namespace Thesis.Areas.UserArea.UserBasket
{
    /// <summary>
    /// Логика взаимодействия для Creel.xaml
    /// </summary>
    public partial class Creel : Window
    {
        private readonly List<Basket> _basket;
        private readonly ApplicationUser _account;

        public Creel(ApplicationUser user, List<Basket> basket)
        {
            InitializeComponent();
            _basket = basket;
            _account = user;
            Data();
        }

        private void Buy(object sender, RoutedEventArgs e)
        {

        }

        private void ProductCatalog(object sender, RoutedEventArgs e)
        {
            new MainWindow(_account, _basket).Show();
            Close();
        }

        private void Data()
        {
            List<CreelData> creel = new List<CreelData>();
            Product product = null;
            foreach (Basket item in _basket)
            {
                using (ApplicationDbContext _context = new ApplicationDbContext())
                {
                    product = _context.Product
                        .Where(x => x.Id == item.Size.ProductId)
                        .FirstOrDefault();
                }
                if (product != null)
                {
                    creel.Add(new CreelData(item, product));
                }
            }
            if (creel != null)
            {
                _creel.ItemsSource = creel;
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {

        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
