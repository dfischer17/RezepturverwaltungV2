using Database;
using Database.Entities;
using MVVM.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            set {
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


        /*Commands*/
        public ICommand AddResourceCommand => new RelayCommand<string>(
            AddResource,
            x => DescriptionTxtBox.Trim().Length > 0
            );

        /*/Helper*/
        private void AddResource(string obj)
        {            
            var resource = new Resource
            {
                Name = DescriptionTxtBox,
                Amount = double.Parse(AmountTxtBox),
                Unit = Unit,
                Netprice = double.Parse(NetpriceTxtBox),
                Taxrate = double.Parse(TaxrateTxtBox),
            };

            db.Resources.Add(resource);
            db.SaveChanges();
            Resources = db.Resources.AsObservableCollection();
        }
    }
}
