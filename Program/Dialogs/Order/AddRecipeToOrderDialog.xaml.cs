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

namespace Program.Dialogs.Order
{
    /// <summary>
    /// Interaction logic for AddRecipeToOrderDialog.xaml
    /// </summary>
    public partial class AddRecipeToOrderDialog : Window
    {
        private readonly MyDbContext db;
        private readonly Database.Entities.Order selectedOrder;

        public AddRecipeToOrderDialog()
        {
            InitializeComponent();
        }

        public AddRecipeToOrderDialog(MyDbContext db, Database.Entities.Order selectedOrder)
        {
            InitializeComponent();

            this.db = db;
            this.selectedOrder = selectedOrder;

            availableRecipeGrid.ItemsSource = db.Recipes.ToList();
        }

        public void AddRecipeToOrder()
        {
            var selectedRecipe = availableRecipeGrid.SelectedItem as Recipe;
            var orderDetail = new OrderDetail
            {
                OrderId = selectedOrder.Id,
                RecipeId = selectedRecipe.Id,
                Quantity = int.Parse(amountTxtbox.Text),
            };
            db.OrderDetails.Add(orderDetail);
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
