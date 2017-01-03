using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDataBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WpfDataBindingContext _dbContext = new WpfDataBindingContext();
        MainWindowViewModel _viewModel = new MainWindowViewModel();
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var firstCustomer = _dbContext.Customers.FirstOrDefault();
            _viewModel.Customer = firstCustomer;

            var orderDates = _dbContext.Orders.Where(o => o.Customer.Id == firstCustomer.Id).Select(o => o.OrderDate).ToList();
            _viewModel.OrderDates = orderDates;
            DataContext = _viewModel;
        }

        void OnOrderSelected(object sender, SelectionChangedEventArgs args)
        {
            var selectedOrder = _dbContext.Orders.Include("OrderItems").Where(o => o.OrderDate == (DateTime)OrdersList.SelectedItem && o.CustomerId == (Guid)CustomerIdLabel.Content).FirstOrDefault();
            _viewModel.OrderItems = selectedOrder.OrderItems.ToList();
            DataContext = _viewModel;
            OrderItemsDataGrid.GetBindingExpression(DataGrid.ItemsSourceProperty).UpdateTarget();
        }
        private void OnSave(object sender, RoutedEventArgs e)
        {
            _dbContext.SaveChanges();
        }
    }
}
