using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NorthWindProducts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public enum StockLevel { Low, Normal, Overstocked };//Enum for stock level

    public partial class MainWindow : Window
    {
        NORTHWNDEntities db = new NORTHWNDEntities();//Reference to Database

        #region Startup
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region StockLevel
            //Populate Stock Level Listbox
            lbxStock.ItemsSource = Enum.GetNames(typeof(StockLevel));
            #endregion

            #region Suppliers
            //Populate the Suppliers Listbox using Anonymous Type
            var query1 = from s in db.Suppliers
                         orderby s.CompanyName
                         select new
                         {
                             SupplierName = s.CompanyName,
                             SupplierID = s.SupplierID,
                             Country = s.Country
                         };

            lbxSuppliers.ItemsSource = query1.ToList();
            #endregion
            
            #region Country
            //Populate Country List
            var query2 = query1
                .OrderBy(s => s.Country)
                .Select(s => s.Country);

            var countries = query2.ToList();

            lbxCountries.ItemsSource = countries.Distinct();
            #endregion
        }
        #endregion

        #region Selection
        //Stock
        private void lbxStock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Get stock levels selected
            var query = db.Products
                .Where(p => p.UnitsInStock < 50)
                .OrderBy(p => p.ProductName)
                .Select(p => p.ProductName);

            string selected = lbxStock.SelectedItem as string;

            switch (selected)
            {
                case "Low":
                    //Do Nothing as query sorted from above
                    break;
                case "Normal":
                    query = db.Products
                        .Where(p => p.UnitsInStock >= 50 
                            && p.UnitsInStock <= 100)
                        .OrderBy(p => p.ProductName)
                        .Select(p => p.ProductName);
                    break;
                case "Overstocked":
                    query = db.Products
                        .Where(p => p.UnitsInStock > 100)
                        .OrderBy(p => p.ProductName)
                        .Select(p => p.ProductName);
                    break;
            }

            //Update Product Listbox
            lbxProducts.ItemsSource = query.ToList();
        }
        //Suppliers
        private void lbxSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Using the selected value path
            int supplierID = Convert.ToInt32(lbxSuppliers.SelectedValue);

            var query = db.Products
                .Where(p => p.SupplierID == supplierID)
                .OrderBy(p => p.ProductName)
                .Select(p => p.ProductName);

            //Update Product Listbox
            lbxProducts.ItemsSource = query.ToList();
        }
        //Countries
        private void lbxCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string country = lbxCountries.SelectedValue as string;

            var query = db.Products
                .Where(p => p.Supplier.Country == country)
                .OrderBy(p => p.ProductName)
                .Select(p => p.ProductName);

            //Update Product Listbox
            lbxProducts.ItemsSource = query.ToList();
        }
        #endregion
    }
}
