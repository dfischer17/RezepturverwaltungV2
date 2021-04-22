using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viemodel;

namespace Program
{
    public class Startup
    {
        internal void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            string connectionString = configuration.GetConnectionString("RecipeDb");
            services.AddDbContext<MyDbContext>(x => x.UseSqlServer(connectionString));
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<ResourceViewModel>();
            services.AddSingleton<RecipeViewModel>();
            services.AddSingleton<OrderViewModel>();
            services.AddSingleton<CustomerViewModel>();
            services.AddSingleton<AddResourceToRecipeViewModel>();
            services.AddTransient<AddResourceToRecipeWindow>();
        }
    }
}
