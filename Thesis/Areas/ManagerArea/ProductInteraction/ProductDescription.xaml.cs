﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.ProductInteraction
{
    /// <summary>
    /// Логика взаимодействия для ProductDescription.xaml
    /// </summary>
    public partial class ProductDescription : Page
    {
        private readonly int _productId;
        public ProductDescription(int productId)
        {
            InitializeComponent();
            _productId = productId;
        }

        private void Data(int productId)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                if (_product.Items.Count != 0)
                {
                    _product.Items.Clear();
                }
                _productSize.ItemsSource = _context.ProductSize
                    .Where(x => x.ProductId == productId)
                    .ToList();
                Product product = _context.Product
                    .Where(x => x.Id == productId)
                    .FirstOrDefault();
                _product.Items.Add(product);
            }
        }

        private void SelectedItem(object sender, SelectionChangedEventArgs e)
        {
            if (_productSize.SelectedItem is ProductSize productSize)
            {
                NavigationService.Navigate(new ProductSizeEdit(productSize));
            }
        }
        
        private void ProductEdit(object sender, SelectionChangedEventArgs e)
        {
            if (_product.SelectedItem is Product product)
            {
                NavigationService.Navigate(new ProductEdit(product));
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
           
        }

        private void Edit(object sender, RoutedEventArgs e)
        {

        }

        private void Delete(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateData(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Data(_productId);
            }
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
