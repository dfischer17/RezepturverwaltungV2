using Database;
using System.Windows;

namespace Viemodel
{
    public class MainViewModel
    {
        private readonly MyDbContext db;

        public MainViewModel()
        {

        }

        public MainViewModel(MyDbContext db)
        {
            this.db = db;        

            //View Models
            customerViewModel = new CustomerViewModel(db);
            resourceViewModel = new ResourceViewModel(db);
            orderViewModel = new OrderViewModel(db);

            recipeViewModel = new RecipeViewModel(db);
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
