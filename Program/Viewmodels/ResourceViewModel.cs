using Database;
using Database.Entities;
using MVVM.Tools;
using Program.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Viemodel
{
    public class ResourceViewModel : ObservableObject
    {
        private MyDbContext db;

        public ResourceViewModel()
        {
        }

        public ResourceViewModel(MyDbContext db)
        {
            this.db = db;
            Resources = db.Resources.AsObservableCollection();
        }

        private ObservableCollection<Resource> resources;
        private Resource selectedResource;
        private string descriptionTxtBox = "";
        private string amountTxtBox = "";
        private string unit = "";
        private string netpriceTxtBox = "";
        private string taxrateTxtBox = "";


        /*Properties*/
        public ObservableCollection<Resource> Resources
        {
            get { return resources; }
            set
            {
                resources = value;
                RaisePropertyChangedEvent(nameof(Resources));
            }
        }
       
        public Resource SelectedResource
        {
            get { return selectedResource; }
            set { selectedResource = value; }
        }


        public string DescriptionTxtBox
        {
            get { return descriptionTxtBox; }
            set
            {
                descriptionTxtBox = value;
                RaisePropertyChangedEvent(nameof(DescriptionTxtBox));
            }
        }

        public string AmountTxtBox
        {
            get { return amountTxtBox; }
            set
            {
                amountTxtBox = value;
                RaisePropertyChangedEvent(nameof(AmountTxtBox));
            }
        }


        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                RaisePropertyChangedEvent(nameof(Unit));
            }
        }

        public string NetpriceTxtBox
        {
            get { return netpriceTxtBox; }
            set
            {
                netpriceTxtBox = value;
                RaisePropertyChangedEvent(nameof(NetpriceTxtBox));
            }
        }

        public string TaxrateTxtBox
        {
            get { return taxrateTxtBox; }
            set
            {
                taxrateTxtBox = value;
                RaisePropertyChangedEvent(nameof(TaxrateTxtBox));
            }
        }

        // Commands
        public ICommand OpenAddResourceDialogCommand => new RelayCommand<string>(
            OpenAddResourceDialog,
            x => x == x
            );

        public ICommand OpenEditResourceDialogCommand => new RelayCommand<string>(
            OpenEditResourceDialog,
            x => SelectedResource != null
            );

        public ICommand DeleteSelectedResourceCommand => new RelayCommand<string>(
            DeleteSelecedResource,
            x => SelectedResource != null
            );


        // Helper
        private void OpenAddResourceDialog(string obj)
        {
            var addResourceDialog = new AddResourceDialog(db);
            if (addResourceDialog.ShowDialog() == true)
            {
                addResourceDialog.AddResource();
                Resources = db.Resources.AsObservableCollection();              
            }
        }

        private void OpenEditResourceDialog(string obj)
        {
            var editResourceDialog = new EditResourceDialog(db, SelectedResource);
            if (editResourceDialog.ShowDialog() == true)
            {
                editResourceDialog.EditResource();
                Resources = db.Resources.AsObservableCollection();
            }
        }

        private void DeleteSelecedResource(string obj)
        {
            var deleteResource = db.Resources.Single(x => x.Id == SelectedResource.Id);
            db.Resources.Remove(deleteResource);
            db.SaveChanges();
            Resources = db.Resources.AsObservableCollection();
        }
    }
}
