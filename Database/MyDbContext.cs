using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        public MyDbContext() { }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Resource> Resources { get; set; } 
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<RecipeDetail> RecipeDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = @"server=(LocalDB)\mssqllocaldb;attachdbfilename=C:\mytemp\Recipe.mdf;database=Recipes;integrated security=True";
                //var connectionString = @"server=(LocalDB)\mssqllocaldb;attachdbfilename=C:\Users\pefr2\OneDrive\Desktop\Databases\Recipe.mdf;database=Recipes;integrated security=True";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Seed Testdaten
            //modelBuilder.TestSeed();

            //Seed
            modelBuilder.Seed();
        }

    }
}
