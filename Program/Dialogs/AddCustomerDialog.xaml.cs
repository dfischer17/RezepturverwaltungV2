using Database;
using Database.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Program.Dialogs
{
    /// <summary>
    /// Interaction logic for AddCustomerDialog.xaml
    /// </summary>
    public partial class AddCustomerDialog : Window
    {
        private readonly MyDbContext db;

        public AddCustomerDialog()
        {
            InitializeComponent();
        }

        public AddCustomerDialog(MyDbContext db)
        {
            InitializeComponent();
            this.db = db;
        }

        public void AddCustomer()
        {
            var customer = new Customer
            {
                Lastname = lastnameTxtbox.Text,
                Firstname = firstnameTxtbox.Text,
                Phonenumber = long.Parse(phoneNbrTxtbox.Text),
                Email = emailTxtbox.Text,
            };

            db.Customers.Add(customer);
            db.SaveChanges();

            Debug.WriteLine("AddCustomer");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
