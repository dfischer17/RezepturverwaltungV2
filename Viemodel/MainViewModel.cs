using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Viemodel
{
    public class MainViewModel
    {
        private readonly MyDbContext db;
        private readonly Window addResourceToRecipeWindow;

        public MainViewModel()
        {

        }
        public MainViewModel(MyDbContext db, Window addResourceToRecipeWindow)
        {         
            this.db = db;
            this.addResourceToRecipeWindow = addResourceToRecipeWindow;

            //View Models
            customerViewModel = new CustomerViewModel(db);
            resourceViewModel = new ResourceViewModel(db);
            orderViewModel = new OrderViewModel(db);

            recipeViewModel = new RecipeViewModel(db, addResourceToRecipeWindow);
        }
        private OrderViewModel orderViewModel;

        public OrderViewModel OrderViewModel
        {
            get => orderViewModel;
        }

        private CustomerViewModel customerViewModel;
        public CustomerViewModel CustomerViewModel
        {
            get => customerViewModel;
        }

        private ResourceViewModel resourceViewModel;
        public ResourceViewModel ResourceViewModel
        {
            get => resourceViewModel;
        }

        private RecipeViewModel recipeViewModel;

        public RecipeViewModel RecipeViewModel
        {
            get => recipeViewModel;
        }

        
    }
}
