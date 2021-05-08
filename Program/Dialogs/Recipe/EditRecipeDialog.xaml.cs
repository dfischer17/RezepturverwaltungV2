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
    /// Interaction logic for EditRecipeDialog.xaml
    /// </summary>
    public partial class EditRecipeDialog : Window
    {
        private readonly MyDbContext db;
        private readonly Recipe selectedRecipe;

        public EditRecipeDialog()
        {
            InitializeComponent();
        }

        public EditRecipeDialog(MyDbContext db, Recipe selectedRecipe)
        {
            InitializeComponent();
            this.db = db;
            this.selectedRecipe = selectedRecipe;

            // Previous recipe values before editing
            descTxtbox.Text = selectedRecipe.Name;
            amountTxtbox.Text = selectedRecipe.Amount.ToString();
            prodPrice.Text = selectedRecipe.Costprice.ToString();
            retailPrice.Text = selectedRecipe.Retailprice.ToString();
            unitTxtbox.Text = selectedRecipe.Unit;
        }

        public void EditRecipe()
        {
            var editRecipe = db.Recipes.Single(x => x.Id == selectedRecipe.Id);

            editRecipe.Name = descTxtbox.Text;
            editRecipe.Amount = int.Parse(amountTxtbox.Text);
            editRecipe.Costprice = double.Parse(prodPrice.Text);
            editRecipe.Retailprice = double.Parse(retailPrice.Text);
            editRecipe.Unit = unitTxtbox.Text;

            db.SaveChanges();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
