using Database;
using Database.Entities;
using MVVM.Tools;
using Program.Dialogs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Viemodel
{
    public class CustomerViewModel : ObservableObject
    {
        private MyDbContext db;

        public CustomerViewModel(MyDbContext db)
        {
            this.db = db;
            Customers = db.Customers.AsObservableCollection();
        }

        public CustomerViewModel()
        {

        }

        private ObservableCollection<Customer> customers;
        private Customer selectedCustomer;
        private string lastnameTxt = "";
        private string firstnameTxt = "";
        private string phonenumber = "";
        private string email = "";

        /*Properties*/
        public ObservableCollection<Customer> Customers
        {
            get { return customers; }
            set
            {
                customers = value;
                RaisePropertyChangedEvent(nameof(Customers));
            }
        }

        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set { selectedCustomer = value; }
        }


        public string LastnameTxt
        {
            get { return lastnameTxt; }
            set
            {
                lastnameTxt = value;
                RaisePropertyChangedEvent(nameof(LastnameTxt));
            }
        }

        public string FirstnameTxt
        {
            get { return firstnameTxt; }
            set
            {
                firstnameTxt = value;
                RaisePropertyChangedEvent(nameof(FirstnameTxt));
            }
        }

        public string Phonenumber
        {
            get { return phonenumber; }
            set
            {
                phonenumber = value;
                RaisePropertyChangedEvent(nameof(Phonenumber));
            }
        }

        public string EmailTxt
        {
            get { return email; }
            set
            {
                email = value;
                RaisePropertyChangedEvent(nameof(EmailTxt));
            }
        }

        // Commands
        public ICommand OpenAddCustomerDialogCommand => new RelayCommand<string>(
            OpenAddCustomerDialog,
            x => x == x
            );

        public ICommand OpenEditCustomerDialogCommand => new RelayCommand<string>(
            OpenEditCustomerDialog,
            x => SelectedCustomer != null
            );

        public ICommand DeleteSelectedCustomerCommand => new RelayCommand<string>(
            DeleteSelecedCustomer,
            x => SelectedCustomer != null
            );


        // Helper
        private void OpenAddCustomerDialog(string obj)
        {
            var addCustomerDialog = new AddCustomerDialog(db);
            if (addCustomerDialog.ShowDialog() == true)
            {
                addCustomerDialog.AddCustomer();
                Customers = db.Customers.AsObservableCollection();
            }
        }

        private void OpenEditCustomerDialog(string obj)
        {
            var editCustomerDialog = new EditCustomerDialog(db, SelectedCustomer);
            if (editCustomerDialog.ShowDialog() == true)
            {
                editCustomerDialog.EditCustomer();
                Customers = db.Customers.AsObservableCollection();
            }
        }

        private void DeleteSelecedCustomer(string obj)
        {
            var deleteCustomer = db.Customers.Single(x => x.Id == SelectedCustomer.Id);
            db.Customers.Remove(deleteCustomer);
            db.SaveChanges();
            Customers = db.Customers.AsObservableCollection();
        }
    }
}
