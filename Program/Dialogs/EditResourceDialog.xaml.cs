using Database;
using Database.Entities;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Program.Dialogs
{
    /// <summary>
    /// Interaction logic for EditResourceDialog.xaml
    /// </summary>
    public partial class EditResourceDialog : Window
    {
        private MyDbContext db;
        private readonly Resource selectedResource;

        public EditResourceDialog()
        {
            InitializeComponent();
        }

        public EditResourceDialog(MyDbContext db, Resource selectedResource)
        {
            InitializeComponent();
            this.db = db;
            this.selectedResource = selectedResource;

            // Previous resource values before editing
            descTxtbox.Text = selectedResource.Name;
            netpriceTxtbox.Text = selectedResource.Netprice.ToString();
            taxrateTxtbox.Text = selectedResource.Taxrate.ToString();
            unitTxtbox.Text = selectedResource.Unit;
            amountTxtbox.Text = selectedResource.Amount.ToString();
        }

        public void EditResource()
        {
            var editResource = db.Resources.Single(x => x.Id == selectedResource.Id);
            editResource.Name = descTxtbox.Text;
            editResource.Netprice = double.Parse(netpriceTxtbox.Text);
            editResource.Taxrate = double.Parse(taxrateTxtbox.Text);
            editResource.Unit = unitTxtbox.Text;
            editResource.Amount = double.Parse(amountTxtbox.Text);

            db.SaveChanges();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
