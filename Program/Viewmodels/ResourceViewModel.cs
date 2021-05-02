using Database;
using Database.Entities;
using MVVM.Tools;
using Program.Dialogs;
using System.Collections.ObjectModel;
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

        public ICommand OpenAddResourceDialogCommand => new RelayCommand<string>(
            OpenAddResourceDialog,
            x => x == x
            );

        private void OpenAddResourceDialog(string obj)
        {
            var addResourceDialog = new AddResourceDialog(db);
            if (addResourceDialog.ShowDialog() == true)
            {
                addResourceDialog.AddResource();
                Resources = db.Resources.AsObservableCollection();              
            }
        }
    }
}
