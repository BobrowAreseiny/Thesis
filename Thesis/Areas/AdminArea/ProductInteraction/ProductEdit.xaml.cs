using Microsoft.Win32;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.ProductInteraction
{
    /// <summary>
    /// Логика взаимодействия для ProductEdit.xaml
    /// </summary>
    public partial class ProductEdit : Page
    {

        private readonly string pathToFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private Product _product = new Product();

        public ProductEdit()
        {
            InitializeComponent();
            Data(null);
        }

        public ProductEdit(Product selectedProduct)
        {
            InitializeComponent();
            try
            {
                if (selectedProduct != null)
                {
                    _product = selectedProduct;
                }
                Data(selectedProduct.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                _product.Sex = _context.Sex
                        .Where(x => x.Id == ((Sex)_sex.SelectedItem).Id)
                        .FirstOrDefault();
                _product.Material = _context.Material
                    .Where(x => x.Id == ((Material)_material.SelectedItem).Id)
                    .FirstOrDefault();
                _product.Season = _context.Season
                    .Where(x => x.Id == ((Season)_season.SelectedItem).Id)
                    .FirstOrDefault();
                if (_product.Id != 0)
                {
                    Product data = _context.Product
                        .Where(x => x.Id == _product.Id)
                        .FirstOrDefault();
                    if (data != null)
                    {
                        data.Name = _product.Name;
                        data.Price = _product.Price;
                        data.ProductCode = _product.ProductCode;
                        data.ProductImage = _product.ProductImage;
                        data.Description = _product.Description;
                        data.CountOnStorage = _product.CountOnStorage;
                    }
                    _context.Entry(data).State = EntityState.Modified;
                }
                else
                {
                    _context.Product.Add(_product);
                }
                _context.SaveChanges();
                NavigationService.GoBack();
            }
        }

        private void Data(int? Id)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {

                _material.ItemsSource = _context.Material
                    .ToList();
                _season.ItemsSource = _context.Season
                    .ToList();
                _sex.ItemsSource = _context.Sex
                    .ToList();
                if (Id != null)
                {
                    _product = _context.Product
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();
                }
                if (_product != null)
                {
                    _material.SelectedItem = _context.Material
                        .Where(x => x.Id == _product.MaterialId)
                        .FirstOrDefault();
                    _season.SelectedItem = _context.Season
                        .Where(x => x.Id == _product.SeasonId)
                        .FirstOrDefault();
                    _sex.SelectedItem = _context.Sex
                        .Where(x => x.Id == _product.SexId)
                        .FirstOrDefault();
                }
                DataContext = _product;
                ImageSourse(_product.ProductImage);
            }
        }
        private void ImageSourse(string Image)
        {
            try
            {
                _image.Source = BitmapFrame.Create(new Uri(Path.GetFullPath(pathToFile + "\\Data\\Image\\" + Image)));
            }
            catch
            {
                _image.Source = BitmapFrame.Create(new Uri(Path.GetFullPath(pathToFile + "\\Data\\ImageDefault\\noimage.png")));
            }
            _product.ProductImage = Image;
        }

        private void Exit(object sender, RoutedEventArgs e) => NavigationService.GoBack();

        private void ImageSelector(object sender, RoutedEventArgs e)
        {
            string path = string.Empty;
            string imageName = string.Empty;
            string newPath = pathToFile + "\\Data\\Image\\";

            OpenFileDialog OPF = new OpenFileDialog()
            {
                Filter = "Файлы png|*.png|Файлы jpg|*.jpg"
            };

            if (OPF.ShowDialog() == true)
            {
                path = OPF.FileName;
                newPath += OPF.SafeFileName;
                imageName = OPF.SafeFileName;
            }
            if (path != string.Empty)
            {
                File.Copy(path, newPath, true);
                ImageSourse(imageName);
            }
        }
    }
}

