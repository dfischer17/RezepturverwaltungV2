using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Utility
{
   public class DataGridOrderpositions
    {
        public string RecipeName { get; set; }
        public int Quantity { get; set; }
        public double RetailPrice { get; set; }


        public DataGridOrderpositions(string recipeName, int quantity, double retailPrice, string retailPriceOutput)
        {
            RecipeName = recipeName;
            Quantity = quantity;
            RetailPrice = retailPrice;
            RetailPriceOutput = retailPriceOutput;
        }
        private string retailPriceOutput;

        public string RetailPriceOutput
        {
            get => retailPriceOutput;
            set 
            {
                retailPriceOutput = $"{RetailPrice}€";
            }
        }

    }
}
