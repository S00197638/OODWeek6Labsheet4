using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AdventureWorksOrders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Reference to Database
        AdventureLiteEntities db = new AdventureLiteEntities();

        #region Startup
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var query = db.SalesOrderHeaders
                .OrderBy(o => o.Customer.CompanyName)
                .Select(o => o.Customer.CompanyName);

            var customers = query.ToList();

            lbxCustomers.ItemsSource = customers.Distinct();
        }
        #endregion

        #region Selection
        //Customers
        private void lbxCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string customer = lbxCustomers.SelectedItem as string;

            if(customer != null)
            {
                var query = db.SalesOrderHeaders
                    .Where(o => o.Customer.CompanyName.Equals(customer))
                    .Select(o => o);

                lbxOrderSummary.ItemsSource = query.ToList();
            }
        }
        //Order Summary
        private void lbxOrderSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int orderID = Convert.ToInt32(lbxOrderSummary.SelectedValue);

            if(orderID > 0)
            {
                var query = db.SalesOrderDetails
                    .Where(od => od.SalesOrderID == orderID)
                    .Select(od => new
                    {
                        ProductName = od.Product.Name,
                        od.UnitPrice,
                        od.UnitPriceDiscount,
                        od.OrderQty,
                        od.LineTotal
                    });

                dgOrderDetails.ItemsSource = query.ToList();
            }
        }
        #endregion
    }
}
