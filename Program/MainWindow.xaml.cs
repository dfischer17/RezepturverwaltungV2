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
        private readonly MyDbContext db;

        public MainWindow(MyDbContext db)
        {
            InitializeComponent();
            this.db = db;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var addResourceToRecipeWindow = new AddResourceToRecipeWindow(db);
                var mainViewModel = new MainViewModel(db, addResourceToRecipeWindow);
                this.DataContext = mainViewModel;

                int nr = db.Resources.Count();
                Title = $"{nr} Resources";

                //db.Dispose();
            }
            catch (Exception ex)
            {
                Title = ex.Message;
            }
        }
    }
}
