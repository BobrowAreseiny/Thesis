using System;
using System.IO;
using System.Windows;
using Thesis.Areas.MarketingArea.UsersOrders;

namespace Thesis.Areas.MarketingArea
{
    /// <summary>
    /// Логика взаимодействия для MainMarketingArea.xaml
    /// </summary>
    public partial class MainMarketingWindow : Window
    {
        public MainMarketingWindow()
        {
            InitializeComponent();
            dataView.Content = new UserOrders();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathToFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\help.chm";
                System.Diagnostics.Process.Start(pathToFile);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Ошибка!");
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
