using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
