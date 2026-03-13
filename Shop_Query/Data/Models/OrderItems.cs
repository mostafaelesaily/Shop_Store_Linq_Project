using System;
using System.Collections.Generic;
using System.Text;

namespace Shop_Query.Data.Models
{
    public class OrderItems
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

    }
}
