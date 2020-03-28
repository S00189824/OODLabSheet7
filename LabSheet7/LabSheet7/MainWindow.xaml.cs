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

namespace LabSheet7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NORTHWNDEntities db = new NORTHWNDEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void Q1Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from c in db.Customers
                        group c by c.Country into g
                        orderby g.Count() descending
                        select new
                        {
                            Country = g.Key,
                            Count = g.Count()
                        };

            Q1GdDisplay.ItemsSource = query.ToList();
        }

        private void Q2Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from c in db.Customers
                        where c.Country == "Italy"
                        orderby c.CompanyName
                        //select c;
                        select new
                        {
                            c.CompanyName,
                            c.Phone
                        };

            Q2GdDisplay.ItemsSource = query.ToList().Distinct();
        }

        private void Q3Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from p in db.Products
                        where (p.UnitsInStock - p.UnitsOnOrder > 0)
                        select new
                        {
                            product = p.ProductName,
                            available = p.UnitsInStock - p.UnitsOnOrder
                        };

            Q3GdDisplay.ItemsSource = query.ToList();
        }

        private void Q4Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from od in db.Order_Details
                        orderby od.Product.ProductName
                        where od.Discount > 0
                        select new
                        {
                            Product = od.Product.ProductName,
                            DiscountGiven = od.Discount,
                            OrderID = od.OrderID
                        };

            Q4GdDisplay.ItemsSource = query.ToList();
        }

        private void Q5Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from o in db.Orders
                        select o.Freight;


            var query2 = db.Orders.Sum(o => o.Freight);//this is an alternative way to do it

            Q5TblkCount.Text = string.Format("total value of freight for all order is {0:C}",query.Sum());
        }

        private void Q6Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from p in db.Products
                        orderby p.Category.CategoryName, p.UnitPrice descending
                        select new
                        {
                            p.CategoryID,
                            p.Category.CategoryName,
                            p.ProductName,
                            p.UnitPrice
                        };

            var results = query.ToList();
            Q6GdDisplay.ItemsSource = results;
        }

        private void Q7Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from o in db.Orders
                        group o by o.CustomerID into g
                        orderby g.Count() descending
                        select new
                        {
                            CustomerID = g.Key,
                            numberoforders = g.Count()
                        };

            //var combined = from c in db.Customers
            //               join p in query on c.CustomerID equals p.CustomerID
            //               orderby p.numberoforders descending
            //               select new
            //               {
            //                   p.CustomerID,
            //                   c.CompanyName,
            //                   p.numberoforders
            //               };

            //Q7GdDisplay.ItemsSource = combined.ToList().Take(10);
            Q7GdDisplay.ItemsSource = query.ToList().Take(10);
        }

        private void Q8Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from o in db.Orders
                        group o by o.CustomerID into g
                        join c in db.Customers on g.Key equals c.CustomerID
                        select new
                        {
                            CustomerID = c.CustomerID,
                            CompanyName = c.CompanyName,
                            numberOfOrders = c.Orders.Count()
                        };

            Q8GdDisplay.ItemsSource = query.ToList().Take(10);
        }

        private void Q9Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from c in db.Customers
                        where c.Orders.Count == 0
                        orderby c.Orders.Count
                        select new
                        {
                            CompanyID = c.CustomerID,
                            CompanyName = c.CompanyName,
                            NumberOfOrders = c.Orders.Count
                        };

            //var query1 = from o in db.Orders
            //             select o.CustomerID;

            //var query = from c in db.Customers
            //            where !query1.Contains(c.CustomerID)
            //            select new
            //            {
            //                c.CustomerID,
            //                c.CompanyName,
            //                Count = c.Orders.Count()
            //            };

            Q9GdDisplay.ItemsSource = query.ToList();
        }
    }
}
