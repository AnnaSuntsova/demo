﻿using System;
using System.Linq;
using System.Data.Entity;
using EFModel;
using NUnit.Framework;

namespace EFModelTests
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        [Test]
        public void Orders_with_products_of_concrete_category()
        {
            var db = new NorthwindDB();
            const int selectedCategoryId = 4;
            var query = db.Orders.Include(o => o.Order_Details.Select(od => od.Product)).Include(o => o.Customer)
                .Where(o => o.Order_Details.Any(od => od.Product.CategoryID == selectedCategoryId))
                .Select(o => new
                {
                    o.Customer.ContactName,
                    Order_Details = o.Order_Details.Select(od => new
                    {
                        od.Product.ProductName,
                        od.OrderID,
                        od.Discount,
                        od.Quantity,
                        od.UnitPrice,
                        od.ProductID
                    })
                });
            var result = query.ToList();

            foreach (var row in result)
            {
                Console.WriteLine($"Customer: {row.ContactName} Products: {string.Join(", ", row.Order_Details.Select(od => od.ProductName))}");
            }
        }
    }
}
