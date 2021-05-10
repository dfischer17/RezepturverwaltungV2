using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Status Status { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        private string orderDateFormatted;

        public string OrderDateFormatted
        {
            get { return OrderDate.ToString("dd.MM.yyyy"); }
            set { orderDateFormatted = value; }
        }

        private string delieverDateFormatted;

        public string DelieverDateFormatted
        {
            get { return DeliveryDate.ToString("dd.MM.yyyy"); }
            set { delieverDateFormatted = value; }
        }

    }
}
