using System.Collections.Generic;

namespace Database.Entities
{
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double UnitsInStock { get; set; }
        public string Unit { get; set; }
        public double Netprice { get; set; }
        public double Taxrate { get; set; }
        public List<RecipeDetail> RecipeDetails { get; set; }

        private string netpriceOutputFormat;

        public string NetpriceOutputFormat
        {
            get { return $"{Netprice}€"; }
            set { netpriceOutputFormat = value; }
        }
        private string taxrateOutputFormat;

        public string TaxrateOutputFormat
        {
            get { return $"{Taxrate}%"; }
            set { taxrateOutputFormat = value; }
        }


        private double CalculateGrossPrice()
        {
            return Netprice * (Taxrate + 1);
        }

        private double PricePerUnit(bool gross)
        {
            if (gross) return UnitsInStock * Netprice;
            return UnitsInStock * Netprice * (Taxrate + 1);
        }
    }
}