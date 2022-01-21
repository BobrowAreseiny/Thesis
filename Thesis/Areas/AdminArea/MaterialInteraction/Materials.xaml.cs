using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thesis.Data.Model;
using System.Windows;
using System.Collections.Generic;
using System;

namespace Thesis.Areas.AdminArea.MaterialInteraction
{
    /// <summary>
    /// Логика взаимодействия для Materials.xaml
    /// </summary>
    public partial class Materials : Page
    {
        private List<Material> _data;

        public Materials()
        {
            InitializeComponent();
        }


        private void Data()
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _data = null;
                _data = _context.Material.ToList();
                if (_data != null)
                {
                    _materials.ItemsSource = _data;
                }
            }
        }

        private void MaterialEdit(object sender, SelectionChangedEventArgs e)
        {
            if (_materials.SelectedItem is Material material)
            {
                NavigationService.Navigate(new MaterialEdit(material));
            }
        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data();
            }
        }

        private void AddMaterial(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MaterialEdit());
        }

        private void NameSort(object sender, RoutedEventArgs e)
        {
            List<Material> data = _data;
            if (_name.IsChecked == true)
            {
                data = data.OrderBy(x => x.MaterialName).ToList();
                _materials.ItemsSource = null;
                _materials.ItemsSource = data;
            }
            else
            {
                _materials.ItemsSource = null;
                _materials.ItemsSource = _data;
            }
        }

        private void DeleteMaterial(object sender, RoutedEventArgs e)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                int materialId = (int)((Button)sender).Content;
                Material selectedData = _context.Material.Where(x => x.Id == materialId).FirstOrDefault();
                _context.Material.Remove(selectedData);
                _context.SaveChanges();
            }
            Data();
        }

        private void Sherch(object sender, TextChangedEventArgs e)
        {
            _name.IsChecked = false;
            List<Material> data = _data
                .Where(x => x.MaterialName.ToLower().Contains(textSearch.Text.ToLower()))
                .ToList();
            if (data != null)
            {
                _materials.ItemsSource = null;
                _materials.ItemsSource = data;
            }
        }
    }
}
