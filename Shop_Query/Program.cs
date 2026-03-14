<<<<<<< Updated upstream
﻿Console.WriteLine("Hello, World!");
=======
﻿
using Shop_Query.Data;
using Shop_Query.Data.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var db = new AppDbContext();
        // Query 1: Get all products
        var products = db.Products.ToList();
        foreach (var product in products)
        {
            Console.WriteLine($"name is {product.Name} And price is {product.Description}");
        }
        // Query 2: using where
        var product1 = db.Products.Where(x => x.Id == 1).FirstOrDefault();
        Console.WriteLine(product1);
        // Query 3: using orderby 
        var orderitems = db.Orders
                    .Where(x => x.PaymentMethod == PaymentMethod.BankTransfer)
                    .OrderBy(x => x.OrderDate)
                    .ToList();
        foreach (var orderitem in orderitems)
        {
            Console.WriteLine($"PaymentMethod is {orderitem.PaymentMethod} And Date : " +
                $" {orderitem.OrderDate} ");
        }
        // Query 4 : using thenby
        var orderitems2 = db.Orders.Where(x => x.Status == OrderStatus.Pending)
            .OrderBy(x => x.OrderDate)
            .ThenBy(x => x.CustomerId);
        foreach (var orderitem in orderitems2)
        {
            Console.WriteLine($"Status is {orderitem.Status} And Date {orderitem.OrderDate}");
        }
        // Query 5 :  using join ( Order - Customer )
        var orderwithcus =
            db.Orders
            .Join
            (
                db.Customers,
                order => order.CustomerId,
                cus => cus.Id,
                (order, cus) => new
                {
                    order_id = order.Id,
                    cus_id = cus.Id,
                    status = order.Status,
                    date = order.OrderDate
                }
            ).ToList();
        foreach (var o in orderwithcus)
        {
            Console.WriteLine($"OrderId : {o.order_id} ," +
                $" Cus id : {o.cus_id} , status :{o.status} And date {o.date} "

                );
        }
        // Query 6 :  using join more than 2 Tables 

        var orderDetails = db.Orders.Join
             (
              db.OrderItems,
              o => o.Id,
              oi => oi.OrderId,
              (o, oi) => new { o, oi }
             ).Join
             (
              db.Products,
              pi => pi.oi.ProductId,
              p => p.Id,
              (pi, p) => new
              {
                  orderid = pi.oi.OrderId,
                  Custid = pi.o.CustomerId,
                  orderdate = pi.o.OrderDate,
                  orderstatus = pi.o.Status
              }
             ).ToList();
        foreach (var o in orderDetails)
        {
            Console.WriteLine($"OrderId : {o.orderid} ," +
                $" Cus id : {o.Custid} , status :{o.orderstatus} And date {o.orderdate} "

                );
        }
        // Query 7 : using All :  
        var allProducts = db.Products.All(p => p.Price > 100);
        Console.WriteLine(allProducts);

        // Query 8 : using Any :
        var anyProduct = db.Products.Any(p => p.Price > 100);
        Console.WriteLine(anyProduct);

        // Query 9 : using Containes : 
        var containsProduct = db.Products.Contains(new Product { Id = 1 , Name = "Laptop" });
        Console.WriteLine(containsProduct);
    }
}
>>>>>>> Stashed changes
