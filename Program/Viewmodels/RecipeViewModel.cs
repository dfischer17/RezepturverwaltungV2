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
        private readonly Window addResourceToRecipeWindow;

        public RecipeViewModel(MyDbContext db, Window addResourceToRecipeWindow)
        {
            this.db = db;
            this.addResourceToRecipeWindow = addResourceToRecipeWindow;
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
                    StaticValues.selectedRecipe = selectedRecipe.Id; // TODO notwendig?
                    RecipeResources = db.RecipeDetails.Where(x => x.RecipeId == selectedRecipe.Id).Select(x => x.Resource).AsObservableCollection();
                    RaisePropertyChangedEvent(nameof(selectedRecipe));
                }                
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

        public ICommand OpenAddResourceToRecipeWindowCommand => new RelayCommand<string>(
            OpenAddResourceToRecipeWindow,
            x => x == x
            );

        public ICommand OpenEditRecipeDialogCommand => new RelayCommand<string>(
            OpenEditRecipeDialog,
            x => SelectedRecipe != null
            );

        public ICommand DeleteSelectedRecipeCommand => new RelayCommand<string>(
            DeleteSelecedRecipe,
            x => SelectedRecipe != null
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
        }

        private void OpenAddResourceToRecipeWindow(string obj)
        {
            addResourceToRecipeWindow.Show();
        }
    }
}
