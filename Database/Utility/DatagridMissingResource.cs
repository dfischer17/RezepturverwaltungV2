using Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Utility
{
    public class DatagridMissingResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Fehlend { get; set; }
        public int Gefaeß { get; set; }
        public string Einheit { get; set; }
    }
}
