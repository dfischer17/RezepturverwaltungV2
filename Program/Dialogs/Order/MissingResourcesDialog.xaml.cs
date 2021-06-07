using Database;
using Database.Entities;
using Database.Utility;
using MVVM.Tools;
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
        public MissingResourcesDialog(MyDbContext db, Dictionary<Resource, double> missingResourceDictionary)
        {
            InitializeComponent();
            this.db = db;
            List<DatagridMissingResource> missingResources = DatagridMissingResources(missingResourceDictionary);
            datagridResources.ItemsSource = missingResources;
            datagridResourcesDetail.ItemsSource = DatagridResourceDetail(missingResources);
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
                    Fehlend = entry.Value,
                    Gefaeß = entry.Key.Container,
                };
                missingResources.Add(missingResource);
            }
            return missingResources;
        }
        public static List<ResourceDetail> DatagridResourceDetail(List<DatagridMissingResource> missingResources)
        {
            var resourceDetails = new List<ResourceDetail>();
            int id = 0;
            foreach (var resource in missingResources)
            {
                id++;
                int quantity = CalcQuantity(resource);
                var resourceDetail = new ResourceDetail
                {
                    Id = id,
                    ResourceId = resource.Id,
                    Quantity = quantity,
                };
                resourceDetails.Add(resourceDetail);
            }
            return resourceDetails;
        }
        public static int CalcQuantity(DatagridMissingResource missingResource)
        {
            int quantity = 0;
            double checkIfHasDecimal = missingResource.Fehlend / missingResource.Gefaeß;

            if (checkIfHasDecimal == 1) quantity = (int)checkIfHasDecimal;
            else if (!(checkIfHasDecimal % 2 == 0) && checkIfHasDecimal >= 1)
            {
                quantity = (int)checkIfHasDecimal;
                quantity++;
            }
            else if (checkIfHasDecimal % 2 == 0) quantity = (int)checkIfHasDecimal;
            else
            {
                quantity = (int)checkIfHasDecimal;
                quantity++;
            }

            return quantity;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void UpdateStorage_Click(object sender, RoutedEventArgs e)
        {
            var updatedResources = datagridResourcesDetail.ItemsSource as List<ResourceDetail>;
            foreach(var resource in updatedResources)
            {
                var updatingResource = db.Resources.Single(x => x.Id == resource.ResourceId);
                updatingResource.UnitsInStock += resource.Quantity * updatingResource.Container;
                db.SaveChanges();
            }
            this.DialogResult = true;
        }
    }
}
