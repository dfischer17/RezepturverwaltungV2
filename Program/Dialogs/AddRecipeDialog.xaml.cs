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
    /// Interaction logic for AddRecipeDialog.xaml
    /// </summary>
    public partial class AddRecipeDialog : Window
    {
        private readonly MyDbContext db;

        public AddRecipeDialog()
        {
            InitializeComponent();
        }

        public AddRecipeDialog(MyDbContext db)
        {
            InitializeComponent();
            this.db = db;
        }

        public void AddRecipe()
        {
            var recipe = new Recipe
            {
                Name = descTxtbox.Text,
                Amount = int.Parse(amountTxtbox.Text),
                Costprice = double.Parse(prodPrice.Text),
                Retailprice = double.Parse(retailPrice.Text),
                Unit = unitTxtbox.Text,
            };

            db.Recipes.Add(recipe);
            db.SaveChanges();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
