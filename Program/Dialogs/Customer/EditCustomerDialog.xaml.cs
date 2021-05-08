using Database;
using Database.Entities;
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

namespace Program.Dialogs
{
    /// <summary>
    /// Interaction logic for EditCustomerDialog.xaml
    /// </summary>
    public partial class EditCustomerDialog : Window
    {
        private readonly MyDbContext db;
        private readonly Customer selectedCustomer;

        public EditCustomerDialog()
        {
            InitializeComponent();
        }

        public EditCustomerDialog(MyDbContext db, Customer selectedCustomer)
        {
            InitializeComponent();
            this.db = db;
            this.selectedCustomer = selectedCustomer;

            // Previous resource values before editing
            lastnameTxtbox.Text = selectedCustomer.Lastname;
            firstnameTxtbox.Text = selectedCustomer.Firstname;
            phoneNbrTxtbox.Text = selectedCustomer.Phonenumber.ToString();
            emailTxtbox.Text = selectedCustomer.Email;
        }

        public void EditCustomer()
        {
            var editCustomer = db.Customers.Single(x => x.Id == selectedCustomer.Id);

            editCustomer.Lastname = lastnameTxtbox.Text;
            editCustomer.Firstname = firstnameTxtbox.Text;
            editCustomer.Phonenumber = long.Parse(phoneNbrTxtbox.Text);
            editCustomer.Email = emailTxtbox.Text;

            db.SaveChanges();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
