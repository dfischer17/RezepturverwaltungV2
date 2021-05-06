using Database;
using Database.Entities;
using MVVM.Tools;
using System.Diagnostics;
using System.Windows;

namespace Program.Dialogs
{
    /// <summary>
    /// Interaction logic for AddResourceDialog.xaml
    /// </summary>
    public partial class AddResourceDialog : Window
    {
        private readonly MyDbContext db;

        public AddResourceDialog()
        {
            InitializeComponent();
        }

        public AddResourceDialog(MyDbContext db)
        {
            InitializeComponent();
            this.db = db;
        }

        public void AddResource()
        {
            var resource = new Resource
            {
                Name = descTxtbox.Text,
                UnitsinStock = double.Parse(amountTxtbox.Text),
                Unit = unitTxtbox.Text,
                Netprice = double.Parse(netpriceTxtbox.Text),
                Taxrate = double.Parse(taxrateTxtbox.Text),
            };

            db.Resources.Add(resource);
            db.SaveChanges();

            Debug.WriteLine("AddResource");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
