using System.Collections.Generic;

namespace Database.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public long Phonenumber { get; set; }
        public string Email { get; set; }
        public List<Order> Orders { get; set; }
        public override string ToString()
        {
            return $"{Lastname} {Firstname}";
        }
    }
}