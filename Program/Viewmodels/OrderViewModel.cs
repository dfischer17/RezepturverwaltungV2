using Database;
using Database.Entities;
using MVVM.Tools;
using Program.Dialogs.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Viemodel
{
    public class OrderViewModel : ObservableObject
    {
        private readonly MyDbContext db;
        private readonly ResourceViewModel resourceViewModel;

        public OrderViewModel()
        {

        }
        public OrderViewModel(MyDbContext db, ResourceViewModel resourceViewModel)
        {
            this.db = db;
            this.resourceViewModel = resourceViewModel;
            Customers = db.Customers.AsObservableCollection();
        }

        //Properties

        private ObservableCollection<Customer> customers;

        public ObservableCollection<Customer> Customers
        {
            get => customers;
            set
            {
                customers = value;
                RaisePropertyChangedEvent(nameof(Customers));
            }
        }
        private Customer selectedCustomer;

        public Customer SelectedCustomer
        {
            get => selectedCustomer;
            set
            {
                selectedCustomer = value;
                Orders = db.Orders.Where(x => x.CustomerId == SelectedCustomer.Id).AsObservableCollection();
                Recipes = new ObservableCollection<Recipe>();
                RaisePropertyChangedEvent(nameof(SelectedCustomer));
            }
        }

        private ObservableCollection<Order> orders;

        public ObservableCollection<Order> Orders
        {
            get => orders;
            set
            {
                orders = value;
                RaisePropertyChangedEvent(nameof(Orders));
            }
        }
        private Order selectedOrder;

        public Order SelectedOrder
        {
            get => selectedOrder;
            set
            {
                selectedOrder = value;
                if (selectedOrder != null)
                {
                    Recipes = db.OrderDetails.Where(x => x.OrderId == SelectedOrder.Id).Select(x => x.Recipe).AsObservableCollection();
                    SelectedOrder.OrderDetails = db.OrderDetails.Where(x => x.OrderId == SelectedOrder.Id).ToList();
                    MissingResources = CalcMissingResources(Recipes);
                }

                RaisePropertyChangedEvent(nameof(SelectedOrder));
            }
        }
        private ObservableCollection<Recipe> recipes;

        public ObservableCollection<Recipe> Recipes
        {
            get => recipes;
            set
            {
                recipes = value;
                RaisePropertyChangedEvent(nameof(Recipes));
            }
        }

        private Recipe selectedRecipe;

        public Recipe SelectedRecipe
        {
            get { return selectedRecipe; }
            set
            {
                selectedRecipe = value;
                RaisePropertyChangedEvent(nameof(SelectedRecipe));
            }
        }
        private Dictionary<Resource,double> missingResources = new ();
                
        public Dictionary<Resource,double> MissingResources
        {
            get => missingResources; 
            set 
            {
                missingResources = value;
                RaisePropertyChangedEvent(nameof(MissingResources));
            }
        }


        //Commands
        public ICommand AddOrderCommand => new RelayCommand<string>(
           AddOrder,
           x => SelectedCustomer != null);

        public ICommand EditOrderCommand => new RelayCommand<string>(
           EditOrder,
           x => SelectedOrder != null);

        public ICommand DeleteOrderCommand => new RelayCommand<string>(
           DeleteSelectedOrder,
           x => SelectedCustomer != null && SelectedOrder != null);

        public ICommand OpenAddRecipeToOrderDialogCommand => new RelayCommand<string>(
           OpenAddRecipeToOrderDialog,
           x => SelectedOrder != null);

        public ICommand DeleteSelectedRecipeCommand => new RelayCommand<string>(
          DeleteSelectedRecipe,
          x => SelectedRecipe != null);

        public ICommand OpenBillDialogCommand => new RelayCommand<string>(
          OpenBillDialog,
          x => SelectedOrder != null);

        public ICommand ReorderMissingResourcesCommand => new RelayCommand<string>(
            ReorderMissingResources,
            x => MissingResources.Count > 0 && SelectedOrder != null );

        public ICommand CompleteOrderCommand => new RelayCommand<string>(
            CompleteOrder,
            x => MissingResources.Count == 0 && SelectedOrder != null && SelectedOrder.Status != Status.Done);

        private static Dictionary<Resource, double> CalcMissingResources(ObservableCollection<Recipe> recipes)
        {
            Dictionary<Resource, double> resources = new Dictionary<Resource, double>();

            foreach (var recipe in recipes)
            {
                foreach(var resource in recipe.RecipeDetails.ToList())
                {
                    double amountToReorder = resource.Resource.UnitsInStock - resource.Quantity;
                    if (amountToReorder < 0 && !resources.ContainsKey(resource.Resource)) // Zu wenig Rohstoff vorhanden
                    {
                        resources.Add(resource.Resource, Math.Abs(amountToReorder));
                    }
                    else if (amountToReorder < 0 && resources.ContainsKey(resource.Resource)) // Rohstoff schon bei fehlenden Rohstoffen
                    {
                        resources[resource.Resource] += Math.Abs(amountToReorder); 
                    }
                }
            }
            return resources;
        }

        /*/Helper*/
        private void AddOrder(string obj)
        {
            var addOrderDialog = new AddOrderDialog();

            if (addOrderDialog.ShowDialog() == true)
            {
                var order = new Order
                {
                    CustomerId = SelectedCustomer.Id,
                    DeliveryDate = addOrderDialog.GetDeliverDate(),
                    OrderDate = addOrderDialog.GetOrderDate(),
                };
                db.Orders.Add(order);
                db.SaveChanges();
                Orders = db.Orders.Where(x => x.CustomerId == SelectedCustomer.Id).AsObservableCollection();
            }
        }

        private void EditOrder(String obj)
        {
            var editOrderDialog = new EditOrderDialog(db, selectedOrder);
            if (editOrderDialog.ShowDialog() == true)
            {
                editOrderDialog.EditOrder();
                Orders = db.Orders.Where(x => x.CustomerId == SelectedCustomer.Id).AsObservableCollection();
            }
        }

        private void DeleteSelectedOrder(string obj)
        {
            var deleteOrder = db.Orders.Single(x => x.Id == SelectedOrder.Id);
            db.Orders.Remove(deleteOrder);
            db.SaveChanges();
            Orders = db.Orders.Where(x => x.CustomerId == SelectedCustomer.Id).AsObservableCollection();
            Recipes = new();
            MissingResources = new();
        }

        private void DeleteSelectedRecipe(string obj)
        {
            var recipe = SelectedRecipe as Recipe;

            var deleteOrderDetail = SelectedOrder.OrderDetails.Single(x => x.RecipeId == recipe.Id);
            db.OrderDetails.Remove(deleteOrderDetail);
            db.SaveChanges();
            Recipes = db.OrderDetails.Where(x => x.OrderId == SelectedOrder.Id).Select(x => x.Recipe).AsObservableCollection();
        }

        private void OpenAddRecipeToOrderDialog(String obj)
        {
            var addRecipeToOrderDialog = new AddRecipeToOrderDialog(db, selectedOrder);
            if (addRecipeToOrderDialog.ShowDialog() == true)
            {
                addRecipeToOrderDialog.AddRecipeToOrder();
                Recipes = db.OrderDetails.Where(x => x.OrderId == SelectedOrder.Id).Select(x => x.Recipe).AsObservableCollection();
            }
        }
        private void CompleteOrder(string obj)
        {
            var currentOrder = SelectedOrder;
            foreach(var recipe in currentOrder.OrderDetails.Select(x => x.Recipe))
            {
                foreach(var resource in recipe.RecipeDetails.Select(x => x.Resource))
                {
                    var updatingResource = db.Resources.Single(x => x.Id == resource.Id);
                    var recipeQuantity = recipe.RecipeDetails.Single(x => x.ResourceId == resource.Id);
                    updatingResource.UnitsInStock -= recipeQuantity.Quantity;
                    db.SaveChanges();
                    SelectedOrder.Status = Status.Done;
                }
            }
            LoadViewModelData();
            resourceViewModel.Resources = db.Resources.AsObservableCollection();
        }
        private void ReorderMissingResources(string obj)
        {
            var resourcesDialog = new MissingResourcesDialog(db, MissingResources);
            resourcesDialog.ShowDialog();
            LoadViewModelData();
            MissingResources = CalcMissingResources(Recipes);
            resourceViewModel.Resources = db.Resources.AsObservableCollection();
        }
        private void OpenBillDialog(string obj)
        {
            BillDialog billDialog = new BillDialog(db, SelectedOrder);
            billDialog.ShowDialog();
        }
        public void LoadViewModelData()
        {
            Customers = db.Customers.AsObservableCollection();
            Orders = db.Orders.Where(x => x.CustomerId == SelectedCustomer.Id).AsObservableCollection();
            Recipes = db.OrderDetails.Where(x => x.OrderId == SelectedOrder.Id).Select(x => x.Recipe).AsObservableCollection();
            SelectedOrder.OrderDetails = db.OrderDetails.Where(x => x.OrderId == SelectedOrder.Id).ToList();
        }
    }
}
