using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class RecipeDetail
    {
        public int Id { get; set; }
        public double Quantity { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int ResourceId { get; set; }
        public Resource Resource { get; set; }
    }
}
