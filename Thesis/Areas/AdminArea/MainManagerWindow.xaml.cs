using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
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
        private DispatcherTimer timer;
        private double leftPanelWidth;
        private bool hidden;

        public MainManagerWindow()
        {
            InitializeComponent();
            ImageDelete();
            dataView.Content = new UserOrders();
            ImageSetter();
            //Timer();
        }

        private void Timer()
        {
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 0)
            };
            timer.Tick += Timer_Tick;
            leftPanelWidth = leftPanel.Width;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (hidden)
            {
                leftPanel.Width += 1;
                if (leftPanel.Width >= leftPanelWidth)
                {
                    timer.Stop();
                    hidden = false;
                }
            }
            else
            {
                leftPanel.Width -= 1;
                if (leftPanel.Width <= 50)
                {
                    timer.Stop();
                    hidden = true;
                }
            }
        }

        private void ImageSetter()
        {
            try
            {
                imageOrder.Source = BitmapFrame.Create(new Uri(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\ImageDefault\order.png"));
                imageUser.Source = BitmapFrame.Create(new Uri(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\ImageDefault\user.png"));
                imageProduct.Source = BitmapFrame.Create(new Uri(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\ImageDefault\product.png"));
                imageActors.Source = BitmapFrame.Create(new Uri(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\ImageDefault\comedy.png"));
                imageMaterial.Source = BitmapFrame.Create(new Uri(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\ImageDefault\cloth.png"));
                imageCity.Source = BitmapFrame.Create(new Uri(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\ImageDefault\city.png"));
                Три_полоски.ImageSource = BitmapFrame.Create(new Uri(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Data\ImageDefault\menu.png"));
            }
            catch { }
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
            using (ApplicationDbContent _context = new ApplicationDbContent())
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

        private void TimerStarter(object sender, RoutedEventArgs e)
        {
            //timer.Start();
        }
    }
}
