using Database;
using Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Program.Dialogs
{
    /// <summary>
    /// Interaction logic for AddResourceToRecipeDialog.xaml
    /// </summary>
    public partial class AddResourceToRecipeDialog : Window
    {
        private readonly MyDbContext db;
        private readonly Recipe selectedRecipe;

        public AddResourceToRecipeDialog(MyDbContext db, Recipe selectedRecipe)
        {
            InitializeComponent();
            this.db = db;
            resourceGrid.ItemsSource = db.Resources.ToList();
            this.selectedRecipe = selectedRecipe;
        }

        public void AddResourceToRecipe()
        {
            var selectedResource = resourceGrid.SelectedItem as Resource;
            var recipeDetail = new RecipeDetail
            {
                RecipeId = selectedRecipe.Id,
                ResourceId = selectedResource.Id,
                Quantity = Double.Parse(amountTxtbox.Text.Replace('.', ',')),
            };
            db.RecipeDetails.Add(recipeDetail);
            db.SaveChanges();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (amountTxtbox.Text.Trim() != "")
            {
                this.DialogResult = true;
            }            
        }
    }
}
