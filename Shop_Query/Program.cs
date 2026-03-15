
using Shop_Query.Data;
using Shop_Query.Data.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var db = new AppDbContext();


        // Get all products
        var products = db.Products.ToList();

        foreach (var product in products)
        {
            Console.WriteLine($"Product: {product.Name} Price: {product.Price}");
        }



        // Where + FirstOrDefault
        var product1 = db.Products
            .Where(x => x.Id == 1)
            .FirstOrDefault();

        Console.WriteLine($"Product Found: {product1?.Name}");



        // OrderBy
        var orderitems = db.Orders
            .Where(x => x.PaymentMethod == PaymentMethod.BankTransfer)
            .OrderBy(x => x.OrderDate)
            .ToList();

        foreach (var orderitem in orderitems)
        {
            Console.WriteLine($"Payment: {orderitem.PaymentMethod} Date: {orderitem.OrderDate}");
        }



        // OrderBy + ThenBy
        var orderitems2 = db.Orders
            .Where(x => x.Status == OrderStatus.Pending)
            .OrderBy(x => x.OrderDate)
            .ThenBy(x => x.CustomerId)
            .ToList();

        foreach (var orderitem in orderitems2)
        {
            Console.WriteLine($"Status: {orderitem.Status} Date: {orderitem.OrderDate}");
        }



        // Any
        var anyProduct = db.Products.Any(p => p.Price > 100);

        Console.WriteLine($"Any product price > 100 : {anyProduct}");



        // All
        var allProducts = db.Products.All(p => p.Price > 100);

        Console.WriteLine($"All products price > 100 : {allProducts}");

        // Skip And Take 

        var SkipAndTakeResult =
            db.Products.Skip(10).Take(10)
            .ToList();

        foreach(var product in SkipAndTakeResult)
        {
            Console.WriteLine($"Product Name is {product.Name} And price {product.Price}");
        }


        // Distinct
        var distinctProducts = db.Products
            .Select(p => p.Name)
            .Distinct()
            .ToList();

        foreach (var productName in distinctProducts)
        {
            Console.WriteLine($"Product Name: {productName}");
        }



        // Aggregate (Sum)
        var totalPrice = db.Products.Sum(p => p.Price);

        Console.WriteLine($"Total Price of all products: {totalPrice}");



        // Join (Orders + Customers)
        var orderwithcus =
            db.Orders
            .Join
            (
                db.Customers,
                order => order.CustomerId,
                cus => cus.Id,
                (order, cus) => new
                {
                    OrderId = order.Id,
                    CustomerId = cus.Id,
                    Status = order.Status,
                    Date = order.OrderDate
                }
            ).ToList();

        foreach (var o in orderwithcus)
        {
            Console.WriteLine($"OrderId: {o.OrderId} CustomerId: {o.CustomerId} Status: {o.Status} Date: {o.Date}");
        }



        // Join more than two tables
        var orderDetails =
            db.Orders
            .Join
            (
                db.OrderItems,
                o => o.Id,
                oi => oi.OrderId,
                (o, oi) => new { o, oi }
            )
            .Join
            (
                db.Products,
                x => x.oi.ProductId,
                p => p.Id,
                (x, p) => new
                {
                    OrderId = x.o.Id,
                    CustomerId = x.o.CustomerId,
                    ProductName = p.Name,
                    OrderDate = x.o.OrderDate,
                    Status = x.o.Status
                }
            ).ToList();

        foreach (var o in orderDetails)
        {
            Console.WriteLine($"OrderId: {o.OrderId} Product: {o.ProductName} Status: {o.Status} Date: {o.OrderDate}");
        }



        // Join + GroupBy + Average
        var avgOrdersPerProduct =
            db.Products
            .Join
            (
                db.OrderItems,
                p => p.Id,
                oi => oi.ProductId,
                (p, oi) => new { p, oi }
            )
            .GroupBy(x => new { x.p.Id, x.p.Name })
            .Select(g => new
            {
                ProductId = g.Key.Id,
                ProductName = g.Key.Name,
                AvgQuantity = g.Average(x => x.oi.Quantity)
            }).ToList();

        foreach (var product in avgOrdersPerProduct)
        {
            Console.WriteLine($"Product: {product.ProductName} Avg Quantity: {product.AvgQuantity}");
        }



        // Join + Join + GroupBy + Aggregation
        var result =
            db.Products
            .Join
            (
                db.Categories,
                p => p.CategoryId,
                c => c.Id,
                (p, c) => new { p, c }
            )
            .Join
            (
                db.OrderItems,
                pc => pc.p.Id,
                oi => oi.ProductId,
                (pc, oi) => new { pc, oi }
            )
            .GroupBy(x => new { x.pc.p.Id, x.pc.p.Name })
            .Select(g => new
            {
                ProductId = g.Key.Id,
                ProductName = g.Key.Name,
                OrdersCount = g.Count(),
                TotalQuantity = g.Sum(x => x.oi.Quantity)
            })
            .ToList();

        foreach (var item in result)
        {
            Console.WriteLine($"Product: {item.ProductName} Orders: {item.OrdersCount} Quantity: {item.TotalQuantity}");
        }

    }
}

