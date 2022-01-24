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
    }
}
