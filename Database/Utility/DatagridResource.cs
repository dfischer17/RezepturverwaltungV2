using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Utility
{
    public class DatagridResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double UnitsInOrder { get; set; }
        public string Unit { get; set; }
        public double Netprice { get; set; }
        public double Taxrate { get; set; }        
    }
}
