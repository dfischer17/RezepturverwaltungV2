using Database;
using Database.Entities;
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
using Viemodel;

namespace Program.Dialogs.Order
{
    /// <summary>
    /// Interaction logic for MissingResourcesDialog.xaml
    /// </summary>
    public partial class MissingResourcesDialog : Window
    {
        private readonly MyDbContext db;

        public MissingResourcesDialog()
        {
            InitializeComponent();
        }
        public MissingResourcesDialog(MyDbContext db, Dictionary<Resource,double> missingResourceDictionary)
        {
            InitializeComponent();
            this.db = db;
            datagridResources.ItemsSource = DatagridMissingResources(missingResourceDictionary);
        }
        public static List<DatagridMissingResource> DatagridMissingResources(Dictionary<Resource, double> missingResourceDictionary)
        {
            var missingResources = new List<DatagridMissingResource>();

            foreach (KeyValuePair<Resource, double> entry in missingResourceDictionary)
            {
                var missingResource = new DatagridMissingResource
                {
                    Id = entry.Key.Id,
                    Name = entry.Key.Name,
                    Einheit = entry.Key.Unit,
                    Fehlend= entry.Value,
                };
                missingResources.Add(missingResource);
            }
            return missingResources;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
