using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Thesis.Areas.AdminArea.CounterpartyInteraction;
using Thesis.Areas.AdminArea.MaterialInteraction;
using Thesis.Areas.AdminArea.ProductInteraction;
using Thesis.Areas.AdminArea.RoleInteraction;
using Thesis.Areas.AdminArea.TownInteraction;
using Thesis.Areas.AdminArea.UserOrderInteraction;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea
{
    /// <summary>
    /// Логика взаимодействия для MainManagerWindow.xaml
    /// </summary>
    public partial class MainManagerWindow : Window
    {
        private readonly string pathToFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        public MainManagerWindow()
        {
            InitializeComponent();
            ImageDelete();
            dataView.Content = new UserOrders();
        }

        private void TownTable(object sender, RoutedEventArgs e)
        {
            dataView.Content = new Towns();
        }

        private void ProductTable(object sender, RoutedEventArgs e)
        {
            dataView.Content = new Products();
        }

        private void UserOrderTable(object sender, RoutedEventArgs e)
        {
            dataView.Content = new UserOrders();
        }

        private void UserTable(object sender, RoutedEventArgs e)
        {
            dataView.Content = new Counterparties();
        }

        private void RoleTable(object sender, RoutedEventArgs e)
        {
            dataView.Content = new Roles();
        }

        private void MaterialTable(object sender, RoutedEventArgs e)
        {
            dataView.Content = new Materials();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ImageDelete()
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                IEnumerable<string> imageInFolder = Directory
                    .GetFiles(pathToFile + "\\Data\\Image")
                    .Select(fn => Path.GetFileName(fn));
                if (imageInFolder != null)
                {
                    List<string> images = imageInFolder.ToList();

                    foreach (Product items in _context.Product.ToList())
                    {
                        images.Remove(items.ProductImage);
                    }
                    foreach (string image in images)
                    {
                        File.Delete(pathToFile + "\\Data\\Image\\" + image);
                    }
                }
            }
        }
    }
}
