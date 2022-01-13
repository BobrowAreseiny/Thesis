using Microsoft.Win32;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Thesis.Data.Model;

namespace Thesis.Areas.ManagerArea.ProductInteraction
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
                    _context.SaveChanges();
                    NavigationService.GoBack();
                }
                else
                {
                    _context.Product.Add(_product);
                }
            }
        }

        private void Data(int? Id) 
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (Id != null)
                {
                    _product = _context.Product.Where(x => x.Id == Id).FirstOrDefault();
                }
                DataContext = _product;
                ImageSourse(_product.ProductImage);
            }
        }
        private void ImageSourse(string Image) 
        {
            _image.Source = BitmapFrame.Create(new Uri(Path.GetFullPath(pathToFile + "\\Data\\Image\\" + Image)));
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
            File.Copy(path, newPath, true);
            ImageSourse(imageName);
        }
    }
}

