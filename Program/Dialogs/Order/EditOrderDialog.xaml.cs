using Database;
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
    /// Interaction logic for EditOrderDialog.xaml
    /// </summary>
    public partial class EditOrderDialog : Window
    {
        private readonly MyDbContext db;
        private readonly Database.Entities.Order selectedOrder;

        public EditOrderDialog()
        {
            InitializeComponent();
        }

        public EditOrderDialog(MyDbContext db, Database.Entities.Order selectedOrder)
        {
            InitializeComponent();
            this.db = db;
            this.selectedOrder = selectedOrder;

            // Previous order values before editing
            orderDatePicker.SelectedDate = selectedOrder.OrderDate;
            deliverDatePicker.SelectedDate = selectedOrder.OrderDate;
        }

        public void EditOrder()
        {
            var editOrder = db.Orders.Single(x => x.Id == selectedOrder.Id);

            editOrder.DeliveryDate = (DateTime)deliverDatePicker.SelectedDate;
            editOrder.OrderDate = (DateTime)orderDatePicker.SelectedDate;

            db.SaveChanges();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
