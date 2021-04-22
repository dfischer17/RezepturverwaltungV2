using System.Collections.Generic;

namespace Database.Entities
{
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
        public double Netprice { get; set; }
        public double Taxrate { get; set; }
        public List<RecipeDetail> RecipeDetails { get; set; }

        private double CalculateGrossPrice()
        {
            return Netprice * (Taxrate + 1);
        }

        private double PricePerUnit(bool gross)
        {
            if (gross) return Amount * Netprice;
            return Amount * Netprice * (Taxrate + 1);
        }
    }
}