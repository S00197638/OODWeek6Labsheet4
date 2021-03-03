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
    }
}
