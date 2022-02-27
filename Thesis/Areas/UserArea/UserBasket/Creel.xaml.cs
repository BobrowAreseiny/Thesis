using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Areas.UserArea.ProductsWindows;
using Thesis.Data.Model;

namespace Thesis.Areas.UserArea.UserBasket
{
    /// <summary>
    /// Логика взаимодействия для Creel.xaml
    /// </summary>
    public partial class Creel : Window
    {
        private readonly List<Basket> _basket;
        private readonly List<CreelData> creel = new List<CreelData>();
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
            new Purchase(_account, _basket).Show();
            Close();
        }

        private void ProductCatalog(object sender, RoutedEventArgs e)
        {
            new ProductsCatalog(_account, _basket).Show();
            Close();
        }

        private void Data()
        {
            creel.Clear();
            if (_basket != null)
            {
                foreach (Basket item in _basket)
                {
                    using (ApplicationDbContent _context = new ApplicationDbContent())
                    {
                        Product product = _context.Product
                            .Where(x => x.Id == item.Size.ProductId)
                            .FirstOrDefault();

                        if (product != null)
                        {
                            creel.Add(new CreelData(item, product));
                        }
                    }
                }
                if (creel != null)
                {
                    _creel.ItemsSource = null;
                    _creel.ItemsSource = creel.OrderBy(x => x.Name);
                }
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            int? productSizeId = (sender as Button).Content as int?;
            if (productSizeId != null)
            {
                Basket selecteditem = _basket.Where(x => x.Size.Id == productSizeId).FirstOrDefault();
                if (selecteditem != null)
                {
                    _basket.Remove(selecteditem);
                    Data();
                }
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            int? productSizeId = (sender as Button).Content as int?;
            if (productSizeId != null)
            {
                Basket selecteditem = _basket
                    .Where(x => x.Size.Id == productSizeId)
                    .FirstOrDefault();
                if (selecteditem != null)
                {
                    new AddToBasket(selecteditem, _basket).ShowDialog();
                    Data();
                }
            }
        }      
    }
}
