using Database;
using Database.Utility;
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
using System.Windows.Shapes;

namespace Program.Dialogs.Order
{
    /// <summary>
    /// Interaction logic for BillDialog.xaml
    /// </summary>
    public partial class BillDialog : Window
    {
        private readonly MyDbContext db;
        private readonly Database.Entities.Order selectedOrder;
        public BillDialog()
        {
            InitializeComponent();
        }
        public BillDialog(MyDbContext db, Database.Entities.Order selectedOrder)
        {
            InitializeComponent();
            this.db = db;
            this.selectedOrder = selectedOrder;
            BillingDate.Content = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss");
            Orderpositions.ItemsSource = db.OrderDetails.Where(x => x.OrderId == selectedOrder.Id).Select(x => new DataGridOrderpositions(x.Recipe.Name, x.Quantity, x.Recipe.Retailprice, x.Recipe.RetailpriceOutputFormat)).ToList();
            Total.Content = db.OrderDetails.Where(x => x.OrderId == selectedOrder.Id).Select(x => x.Quantity * x.Recipe.Retailprice).Sum().ToString() + "€";

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
