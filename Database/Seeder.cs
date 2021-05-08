using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public static class DbSeederExtension
    {
        public static void TestSeed(this ModelBuilder modelBuilder)
        {
            //Test Data
            Console.WriteLine("TestSeed");

            //4 Resources
            modelBuilder.Entity<Resource>().HasData(new Resource
            {
                Id = 1,
                Name = "Aloe Vera 10-fach",
                Amount = 10,
                Netprice = 1.20,
                Taxrate = 0.2,
                Unit = "ml",
            });
            modelBuilder.Entity<Resource>().HasData(new Resource
            {
                Id = 2,
                Name = "Aloe Vera Gel",
                Amount = 15,
                Netprice = 4.8,
                Taxrate = 0.2,
                Unit = "ml",
            });
            modelBuilder.Entity<Resource>().HasData(new Resource
            {
                Id = 3,
                Name = "Alpha-Bisabolol",
                Amount = 6,
                Netprice = 4.8,
                Taxrate = 0.2,
                Unit = "ml",
            });
            modelBuilder.Entity<Resource>().HasData(new Resource
            {
                Id = 4,
                Name = "Bienenwachs weiß kbA",
                Amount = 9,
                Netprice = 7.0,
                Taxrate = 0.15,
                Unit = "g",
            });

            //Recipe
            modelBuilder.Entity<Recipe>().HasData(new Recipe
            {
                Id = 1,
                Name = "Centella 30g",
                Amount = 30,
                Costprice = 20.0,
                Retailprice = 25.0,
                Unit = "mg",
            });
            modelBuilder.Entity<Recipe>().HasData(new Recipe
            {
                Id = 2,
                Name = "Haarshampoo",
                Amount = 200,
                Costprice = 15.0,
                Retailprice = 20,
                Unit = "ml",
            });
            modelBuilder.Entity<Recipe>().HasData(new Recipe
            {
                Id = 3,
                Name = "Centella 30g",
                Amount = 30,
                Costprice = 20.0,
                Retailprice = 25,
                Unit = "mg",
            });

            //RecipeDetail
            modelBuilder.Entity<RecipeDetail>().HasData(new RecipeDetail
            {
                Id = 1,
                Quantity = 3,
                RecipeId = 1,
                ResourceId = 1,
            });
            modelBuilder.Entity<RecipeDetail>().HasData(new RecipeDetail
            {
                Id = 2,
                Quantity = 1,
                RecipeId = 1,
                ResourceId = 2,
            });


            //Customer
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                Id = 1,
                Firstname = "Daniel",
                Lastname = "Fischer",
                Email = "banane23@gmail.com",
                Phonenumber = 004306805147882,
                //Order
            });
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                Id = 2,
                Firstname = "Kroiß",
                Lastname = "Matthias",
                Email = "kroißM@hotmail.com",
                Phonenumber = 004306801534212,

            });
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                Id = 3,
                Firstname = "Pepe",
                Lastname = "Fröhler",
                Email = "banane24@gmx.at",
                Phonenumber = 004306801135683,

            });

            ////Product
            //modelBuilder.Entity<Product>().HasData(new Product
            //{
            //    Id = 1,
            //    Name = "Centella 30g",
            //    Retailprice = 25.0,

            //});

            //Order
            modelBuilder.Entity<Order>().HasData(new Order
            {
                Id = 1,
                CustomerId = 1,
                OrderDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(1),
                Status = Status.Open,

            }); 
            modelBuilder.Entity<Order>().HasData(new Order
            {
                Id = 2,
                CustomerId = 2,
                OrderDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(1),
                Status = Status.Open,
            });
            modelBuilder.Entity<Order>().HasData(new Order
            {
                Id = 3,
                CustomerId = 1,
                OrderDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(1),
                Status = Status.Done,
            });

            //OrderDetail
            modelBuilder.Entity<OrderDetail>().HasData(new OrderDetail
            {
                Id = 1,
                OrderId = 1,
                RecipeId= 1,
                Quantity = 5,
            });
            modelBuilder.Entity<OrderDetail>().HasData(new OrderDetail
            {
                Id = 2,
                OrderId = 1,
                RecipeId = 2,
                Quantity = 3,
            });
            modelBuilder.Entity<OrderDetail>().HasData(new OrderDetail
            {
                Id = 3,
                OrderId = 2,
                RecipeId = 3,
                Quantity = 1,
            });

        }
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //Implement
        }
    }
}
