using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thesis.Data.Model;

namespace Thesis.Areas.AdminArea.MaterialInteraction
{
    /// <summary>
    /// Логика взаимодействия для MaterialEdit.xaml
    /// </summary>
    public partial class MaterialEdit : Page
    {
        private readonly Material _material = new Material();

        public MaterialEdit()
        {
            InitializeComponent();
            DataContext = _material;
        }
        public MaterialEdit(Material material)
        {
            InitializeComponent();
            try
            {
                if (material != null)
                {
                    _material = material;
                }
                DataContext = _material;
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
                if (_material.Id == 0)
                {
                    _context.Material.Add(_material);
                }
                else
                {
                    Material data = _context.Material
                        .Where(f => f.Id == _material.Id)
                        .FirstOrDefault();

                    if (data != null)
                    {
                        data.MaterialName = _material.MaterialName;
                    }
                    _context.Entry(data).State = EntityState.Modified;
                }
                _context.SaveChanges();
                NavigationService.GoBack();
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
