using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.TownInteraction
{
    /// <summary>
    /// Логика взаимодействия для Towns.xaml
    /// </summary>
    public partial class Towns : Page
    {
        private List<Town> _town;
        public Towns()
        {
            InitializeComponent();
        }

        private void Data()
        {
            using (ApplicationDbContent _context = new ApplicationDbContent())
            {
                _town = _context.Town.ToList();
                if (_town != null)
                {
                    _towns.ItemsSource = _town;
                }
            }
        }

        private void TownDescription(object sender, SelectionChangedEventArgs e)
        {
            if (_towns.SelectedItem is Town selectedTown)
            {
                NavigationService.Navigate(new TownDescription(selectedTown));
            }
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            Data();
        }

        private void AddTown(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TownEdit());
        }

        private void NameSort(object sender, RoutedEventArgs e)
        {
            List<Town> data = _town;
            if (_name.IsChecked == true)
            {
                DataSort(data.OrderBy(x => x.TownName).ToList());
            }
            else
            {
                _towns.ItemsSource = null;
                _towns.ItemsSource = _town;
            }
        }

        private void Sherch(object sender, TextChangedEventArgs e)
        {
            _name.IsChecked = false;
            List<Town> data = _town
                .Where(x => x.TownName.ToLower().Contains(textSearch.Text.ToLower()))
                .ToList();
            if (data != null)
            {
                DataSort(data);
            }
        }

        private void DataSort(List<Town> data)
        {
            _towns.ItemsSource = null;
            _towns.ItemsSource = data;
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Точно удалить данные?", "Внимание",
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if ((sender as Button) != null)
                {
                    int sizeId = (int)(sender as Button).Content;
                    using (ApplicationDbContent _context = new ApplicationDbContent())
                    {
                        Town data = _context.Town.Where(x => x.Id == sizeId).FirstOrDefault();
                        _context.Entry(data).State = EntityState.Deleted;
                        _context.SaveChanges();
                        Data();
                    }
                }
            }
        }
    }
}
