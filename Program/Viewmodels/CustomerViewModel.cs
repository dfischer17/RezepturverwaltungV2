using Database;
using Database.Entities;
using MVVM.Tools;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        /*Commands*/
        public ICommand AddCustomerCommand => new RelayCommand<string>(
            AddCustomer,
            x => LastnameTxt.Trim().Length > 0
            );

        /*/Helper*/
        private void AddCustomer(string obj)
        {
            var customer = new Customer
            {
                Lastname = LastnameTxt,
                Firstname = FirstnameTxt,
                Phonenumber = long.Parse(Phonenumber),
                Email = EmailTxt,
            };

            db.Customers.Add(customer);
            db.SaveChanges();
            Customers = db.Customers.AsObservableCollection();
        }
    }
}
