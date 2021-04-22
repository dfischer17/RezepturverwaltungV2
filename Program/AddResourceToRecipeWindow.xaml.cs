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
using System.Windows.Shapes;
using Viemodel;

namespace Program
{
    /// <summary>
    /// Interaction logic for AddResourceToRecipeWindow.xaml
    /// </summary>
    public partial class AddResourceToRecipeWindow : Window
    {
        private readonly MyDbContext db;

        public AddResourceToRecipeWindow(MyDbContext db, AddResourceToRecipeViewModel addResourceToRecipeViewModel)
        {
            InitializeComponent();
            this.db = db;
            DataContext = addResourceToRecipeViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
