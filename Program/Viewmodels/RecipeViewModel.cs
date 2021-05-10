using Database;
using Database.Entities;
using MVVM.Tools;
using Program.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Viemodel
{
    public class RecipeViewModel: ObservableObject
    {
        private readonly MyDbContext db;

        public RecipeViewModel(MyDbContext db)
        {
            this.db = db;            
            Recipes = db.Recipes.AsObservableCollection();
        }

        //Properties
        private ObservableCollection<Recipe> recipes;
        private Recipe selectedRecipe;
        private string name = "";
        private string costprice = "";
        private string amount = "";
        private string unit = "";

        public ObservableCollection<Recipe> Recipes
        {
            get => recipes; 
            set 
            {
                recipes = value;
                RaisePropertyChangedEvent(nameof(Recipes));
            }
        }

        public string NameTxtBox
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChangedEvent(nameof(NameTxtBox));
            }
        }
        public string CostpriceTxtBox
        {
            get => costprice;
            set
            {
                costprice = value;
                RaisePropertyChangedEvent(nameof(CostpriceTxtBox));
            }
        }
        public string AmountTxtBox
        {
            get => amount;
            set
            {
                amount = value;
                RaisePropertyChangedEvent(nameof(AmountTxtBox));
            }
        }

        public string UnitTxtBox
        {
            get => unit;
            set
            {
                unit = value;
                RaisePropertyChangedEvent(nameof(UnitTxtBox));
            }
        }
                
        public Recipe SelectedRecipe
        {
            get { return selectedRecipe; }
            set
            {
                selectedRecipe = value;

                if (selectedRecipe != null)
                {
                    RecipeResources = db.RecipeDetails.Where(x => x.RecipeId == selectedRecipe.Id).Select(x => x.Resource).AsObservableCollection();
                    RaisePropertyChangedEvent(nameof(selectedRecipe));
                }                
            }
        }

        private Resource selectedResource;

        public Resource SelectedResource
        {
            get { return selectedResource; }
            set
            {
                selectedResource = value;
                RaisePropertyChangedEvent(nameof(SelectedResource));
            }
        }



        private ObservableCollection<Resource> recipeResources;

        public ObservableCollection<Resource> RecipeResources
        {
            get => recipeResources;
            set
            {
                recipeResources = value;
                RaisePropertyChangedEvent(nameof(RecipeResources));
            }
        }

        /*Commands*/      
        public ICommand OpenAddRecipeDialogCommand => new RelayCommand<string>(
            OpenAddRecipeDialog,
            x => x == x
            );

        public ICommand OpenAddResourceToRecipeDialogCommand => new RelayCommand<string>(
             OpenAddResourceToRecipeDialog,
             x => SelectedRecipe != null
             );
        
        public ICommand OpenEditRecipeDialogCommand => new RelayCommand<string>(
            OpenEditRecipeDialog,
            x => SelectedRecipe != null
            );

        public ICommand DeleteSelectedRecipeCommand => new RelayCommand<string>(
            DeleteSelecedRecipe,
            x => SelectedRecipe != null
            );

        public ICommand DeleteSelectedResourceCommand => new RelayCommand<string>(
            DeleteSelectedResource,
            x => SelectedRecipe != null && SelectedResource != null
            );

        // Helper
        private void OpenAddRecipeDialog(string obj)
        {
            var addRecipeDialog = new AddRecipeDialog(db);
            if (addRecipeDialog.ShowDialog() == true)
            {
                addRecipeDialog.AddRecipe();
                Recipes = db.Recipes.AsObservableCollection();
            }
        }

        private void OpenEditRecipeDialog(string obj)
        {
            var editRecipeDialog = new EditRecipeDialog(db, SelectedRecipe);
            if (editRecipeDialog.ShowDialog() == true)
            {
                editRecipeDialog.EditRecipe();
                Recipes = db.Recipes.AsObservableCollection();
            }
        }

        private void DeleteSelecedRecipe(string obj)
        {
            var deleteRecipe = db.Recipes.Single(x => x.Id == SelectedRecipe.Id);
            db.Recipes.Remove(deleteRecipe);
            db.SaveChanges();
            Recipes = db.Recipes.AsObservableCollection();
            RecipeResources = new();
        }

        private void DeleteSelectedResource(string obj)
        {            
            var deleteResource = SelectedResource;

            var deleteRecipeDetail = db.RecipeDetails.Single(x => x.ResourceId == deleteResource.Id);
            db.RecipeDetails.Remove(deleteRecipeDetail);
            db.SaveChanges();

            RecipeResources = db.RecipeDetails.Where(x => x.RecipeId == SelectedRecipe.Id).Select(x => x.Resource).AsObservableCollection();            
        }

        private void OpenAddResourceToRecipeDialog(string obj)
        {
            var addResourceToRecipeDialog = new AddResourceToRecipeDialog(db, SelectedRecipe);
            if (addResourceToRecipeDialog.ShowDialog() == true)
            {
                addResourceToRecipeDialog.AddResourceToRecipe();
                Recipes = db.Recipes.AsObservableCollection();
                RecipeResources = db.RecipeDetails.Where(x => x.RecipeId == SelectedRecipe.Id).Select(x => x.Resource).AsObservableCollection();
            }
        }
    }
}
