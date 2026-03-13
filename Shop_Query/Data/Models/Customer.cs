using System;
using System.Collections.Generic;
using System.Text;

namespace Shop_Query.Data.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }

        public string? Address { get; set; }

        public ICollection<Order>? orders { get; set; }
    }
}
