using Database;
using Database.Entities;
using MVVM.Tools;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Viemodel
{
    public class AddResourceToRecipeViewModel : ObservableObject
    {
        private MyDbContext db;

        public AddResourceToRecipeViewModel(MyDbContext db)
        {
            this.db = db;
            Resources = db.Resources.AsObservableCollection();
            Console.WriteLine("");
        }

        public AddResourceToRecipeViewModel()
        {
        }

        private ObservableCollection<Resource> resources;
        private Resource selectedResource;

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
        
        public Resource SelectedResource
        {
            get { return selectedResource; }
            set
            {
                selectedResource = value;
                RaisePropertyChangedEvent(nameof(SelectedResource));
            }
        }


        /*Commands*/
        public ICommand AddResourceToRecipeCommand => new RelayCommand<string>(
            AddResourceToRecipe,
            x => x==x
            );

        /*/Helper*/
        private void AddResourceToRecipe(string obj)
        {
            var recipeDetail = new RecipeDetail
            {
                RecipeId = StaticValues.selectedRecipe,
                ResourceId = SelectedResource.Id,
            };
            db.RecipeDetails.Add(recipeDetail);
            db.SaveChanges();            
        }
    }
}
