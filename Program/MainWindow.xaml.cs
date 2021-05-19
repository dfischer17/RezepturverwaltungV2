using Database;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Viemodel;

namespace Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new MyDbContext();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                
                var mainViewModel = new MainViewModel(db);
                this.DataContext = mainViewModel;

                CalcCostprice(db);
                //db.Dispose();
            }
            catch (Exception ex)
            {
                Title = ex.Message;
            }
            
        }
        public void CalcCostprice(MyDbContext db)
        {
            foreach(var recipe in db.Recipes.ToList())
            {
                var costprice = 0.0;
                var currRecipeDetails = db.RecipeDetails.Where(x => x.RecipeId == recipe.Id).ToList();
                foreach(var recipeDetail in currRecipeDetails)
                {
                    var resource = recipeDetail.Resource;
                    costprice += resource.Netprice;
                }
                recipe.Costprice = Math.Round(costprice,2);
                db.SaveChanges();
            }
        }
    }
}
