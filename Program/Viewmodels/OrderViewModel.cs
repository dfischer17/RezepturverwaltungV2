using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
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
        private bool orderedAllResources = false;
        public OrderViewModel()
        {

        }
        public OrderViewModel(MyDbContext db, ResourceViewModel resourceViewModel)
        {
            this.db = db;
            this.resourceViewModel = resourceViewModel;
            Customers = db.Customers.AsObservableCollection();
            AllMissingResources = CalcAllMissingResources();
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
                //Recipes = new ObservableCollection<Recipe>();
                OrderDetails = new ObservableCollection<OrderDetail>();
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
                    //Recipes = db.OrderDetails.Where(x => x.OrderId == SelectedOrder.Id).Select(x => x.Recipe).AsObservableCollection();
                    OrderDetails = db.OrderDetails.Include(x => x.Recipe).Where(x => x.OrderId == SelectedOrder.Id).AsObservableCollection();
                    SelectedOrder.OrderDetails = db.OrderDetails.Where(x => x.OrderId == SelectedOrder.Id).ToList();
                    MissingResources = CalcMissingResources(SelectedOrder.OrderDetails.Select(x => x.Recipe).AsObservableCollection());
                }

                RaisePropertyChangedEvent(nameof(SelectedOrder));
            }
        }

        private ObservableCollection<OrderDetail> orderDetails;

        public ObservableCollection<OrderDetail> OrderDetails
        {
            get => orderDetails; 
            set 
            {
                orderDetails = value;
                RaisePropertyChangedEvent(nameof(OrderDetails));
            }
        }


        private OrderDetail selectedOrderDetail;

        public OrderDetail SelectedOrderDetail
        {
            get => selectedOrderDetail; 
            set 
            {
                selectedOrderDetail = value;
                RaisePropertyChangedEvent((nameof(SelectedOrderDetail)));
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

        private Dictionary<Resource, double> allMissingResource;

        public Dictionary<Resource, double> AllMissingResources
        {
            get => allMissingResource; 
            set 
            { 
                allMissingResource = value;
                RaisePropertyChangedEvent(nameof(AllMissingResources));
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
           x => SelectedOrder != null && SelectedOrder.Status != Status.Done);

        public ICommand DeleteSelectedRecipeCommand => new RelayCommand<string>(
          DeleteSelectedRecipe,
          x => SelectedOrderDetail != null && SelectedOrder.Status != Status.Done);

        public ICommand OpenBillDialogCommand => new RelayCommand<string>(
          OpenBillDialog,
          x => SelectedOrder != null);

        public ICommand ReorderMissingResourcesCommand => new RelayCommand<string>(
            ReorderMissingResources,
            x => MissingResources.Count > 0 && SelectedOrder != null && SelectedOrder.Status != Status.Done);

        public ICommand CompleteOrderCommand => new RelayCommand<string>(
            CompleteOrder,
            x => MissingResources.Count == 0 && SelectedOrder != null && SelectedOrder.Status != Status.Done);

        public ICommand ReorderAllMissingResourcesCommand => new RelayCommand<string>(
            ReorderAllMissingResources,
            x => AllMissingResources.Count > 0);

        public ICommand CompleteAllOrdersCommand => new RelayCommand<string>(
            CompleteAllOrders,
            x => AllMissingResources.Count == 0 && orderedAllResources == false);

        private void CompleteAllOrders(string obj)
        {
            foreach (var order in db.Orders.ToList())
            {
                order.OrderDetails = db.OrderDetails.Where(x => x.OrderId == order.Id).ToList();
                foreach (var recipe in order.OrderDetails.Select(x => x.Recipe))
                {
                    if (order.Status != Status.Done)
                    {
                        foreach (var resource in recipe.RecipeDetails.Select(x => x.Resource))
                        {
                            var updatingResource = db.Resources.Single(x => x.Id == resource.Id);
                            var recipeQuantity = recipe.RecipeDetails.Single(x => x.ResourceId == resource.Id);
                            updatingResource.UnitsInStock -= recipeQuantity.Quantity;
                            db.SaveChanges();
                            order.Status = Status.Done;
                        }
                    }
                }
            }
            AllMissingResources = CalcAllMissingResources();
            orderedAllResources = true;
            resourceViewModel.Resources = db.Resources.AsObservableCollection();
        }

        private void ReorderAllMissingResources(string obj)
        {
            var resourcesDialog = new MissingResourcesDialog(db, AllMissingResources);
            resourcesDialog.ShowDialog();
            AllMissingResources = CalcAllMissingResources();
            resourceViewModel.Resources = db.Resources.AsObservableCollection();
        }

        private Dictionary<Resource, double> CalcMissingResources(ObservableCollection<Recipe> recipes)
        {
            Dictionary<Resource, double> resources = new Dictionary<Resource, double>();

            foreach (var recipe in recipes)
            {
                foreach(var resource in recipe.RecipeDetails.ToList())
                {
                    if (SelectedOrder.Status != Status.Done)
                    {
                        var quantityOfRecipe = SelectedOrder.OrderDetails.Single(x => x.RecipeId == recipe.Id).Quantity;
                        double amountToReorder = resource.Resource.UnitsInStock - resource.Quantity * quantityOfRecipe;
                        if (amountToReorder < 0 && !resources.ContainsKey(resource.Resource)) // Zu wenig Rohstoff vorhanden
                        {
                            resources.Add(resource.Resource, Math.Abs(amountToReorder));
                        }
                        else if (amountToReorder < 0 && resources.ContainsKey(resource.Resource)) // Rohstoff schon bei fehlenden Rohstoffen
                        {
                            resources[resource.Resource] += Math.Abs(resource.Quantity * quantityOfRecipe);
                        }
                    }
                }
            }
            return resources;
        }

        private Dictionary<Resource, double> CalcAllMissingResources()
        {
            Dictionary<Resource, double> resources = new Dictionary<Resource, double>();

            var recipes = new ObservableCollection<Recipe>();

            foreach(var order in db.Orders.Include(x => x.OrderDetails).Where(x => x.Status != Status.Done).AsObservableCollection())
            {
                if (order.Status != Status.Done)
                {
                    foreach (var recipe in db.OrderDetails.Where(x => x.OrderId == order.Id).Select(x => x.Recipe).AsObservableCollection())
                    {
                        recipe.RecipeDetails = db.RecipeDetails.Where(x => x.RecipeId == recipe.Id).ToList();
                        foreach (var resource in recipe.RecipeDetails.ToList())
                        {
                            var quantityOfRecipe = order.OrderDetails.Single(x => x.RecipeId == recipe.Id).Quantity;
                            double amountToReorder = resource.Resource.UnitsInStock - resource.Quantity * quantityOfRecipe;
                            if (amountToReorder < 0 && !resources.ContainsKey(resource.Resource)) // Zu wenig Rohstoff vorhanden
                            {
                                resources.Add(resource.Resource, Math.Abs(amountToReorder));
                            }
                            else if (amountToReorder < 0 && resources.ContainsKey(resource.Resource)) // Rohstoff schon bei fehlenden Rohstoffen
                            {
                                resources[resource.Resource] += Math.Abs(resource.Quantity * quantityOfRecipe);
                            }
                        }

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
            AllMissingResources = CalcAllMissingResources();
        }

        private void EditOrder(String obj)
        {
            var editOrderDialog = new EditOrderDialog(db, selectedOrder);
            if (editOrderDialog.ShowDialog() == true)
            {
                editOrderDialog.EditOrder();
                Orders = db.Orders.Where(x => x.CustomerId == SelectedCustomer.Id).AsObservableCollection();
            }
            AllMissingResources = CalcAllMissingResources();
        }

        private void DeleteSelectedOrder(string obj)
        {
            var deleteOrder = db.Orders.Single(x => x.Id == SelectedOrder.Id);
            db.Orders.Remove(deleteOrder);
            db.SaveChanges();
            Orders = db.Orders.Where(x => x.CustomerId == SelectedCustomer.Id).AsObservableCollection();
            //Recipes = new();
            OrderDetails = new();
            MissingResources = new();
            AllMissingResources = CalcAllMissingResources();
        }

        private void DeleteSelectedRecipe(string obj)
        {
            var orderDetail = SelectedOrderDetail as OrderDetail;
            db.OrderDetails.Remove(orderDetail);
            db.SaveChanges();
            //Recipes = db.OrderDetails.Where(x => x.OrderId == SelectedOrder.Id).Select(x => x.Recipe).AsObservableCollection();
            OrderDetails = db.OrderDetails.Include(x => x.Recipe).Where(x => x.OrderId == SelectedOrder.Id).AsObservableCollection();
            MissingResources = CalcMissingResources(SelectedOrder.OrderDetails.Select(x => x.Recipe).AsObservableCollection());
            AllMissingResources = CalcAllMissingResources();
        }

        private void OpenAddRecipeToOrderDialog(String obj)
        {
            var addRecipeToOrderDialog = new AddRecipeToOrderDialog(db, selectedOrder);
            if (addRecipeToOrderDialog.ShowDialog() == true)
            {
                addRecipeToOrderDialog.AddRecipeToOrder();
                OrderDetails = db.OrderDetails.Include(x => x.Recipe).Where(x => x.OrderId == SelectedOrder.Id).AsObservableCollection();
               // Recipes = db.OrderDetails.Where(x => x.OrderId == SelectedOrder.Id).Select(x => x.Recipe).AsObservableCollection();
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
            MissingResources = CalcMissingResources(SelectedOrder.OrderDetails.Select(x => x.Recipe).AsObservableCollection());
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
            OrderDetails = db.OrderDetails.Include(x => x.Recipe).Where(x => x.OrderId == SelectedOrder.Id).AsObservableCollection();
            //Recipes = db.OrderDetails.Where(x => x.OrderId == SelectedOrder.Id).Select(x => x.Recipe).AsObservableCollection();
            SelectedOrder.OrderDetails = db.OrderDetails.Where(x => x.OrderId == SelectedOrder.Id).ToList();
        }

    }
}
