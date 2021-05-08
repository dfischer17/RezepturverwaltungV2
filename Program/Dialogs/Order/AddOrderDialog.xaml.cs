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
    /// Interaction logic for AddOrderDialog.xaml
    /// </summary>
    public partial class AddOrderDialog : Window
    {
        public AddOrderDialog()
        {
            InitializeComponent();
        }

        public DateTime GetOrderDate()
        {
            return (DateTime)orderDatePicker.SelectedDate;
        }

        public DateTime GetDeliverDate()
        {
            return (DateTime)deliverDatePicker.SelectedDate;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {            
            this.DialogResult = true;
        }
    }
}
