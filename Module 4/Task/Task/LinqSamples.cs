// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{

		private DataSource dataSource = new DataSource();

		[Category("Restriction Operators")]
		[Title("Where - Task 1")]
		[Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]
		public void Linq1()
		{
			int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

			var lowNums =
				from num in numbers
				where num < 5
				select num;

			Console.WriteLine("Numbers < 5:");
			foreach (var x in lowNums)
			{
				Console.WriteLine(x);
			}
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 2")]
		[Description("This sample returns all presented in market products")]

		public void Linq2()
		{
			var products =
				from p in dataSource.Products
				where p.UnitsInStock > 0
				select p;

			foreach (var p in products)
			{
				ObjectDumper.Write(p);
			}
		}

        [Category("Task")] //доделать!
        [Title("Linq001")]
        [Description("This sample returns all customers, whose total turnover is greater then some value")]

        public void Linq001()
        {
            decimal totalVolume = 10000;
            var clients =
                from cl in dataSource.Customers
                from ord in cl.Orders
                group ord by cl.CompanyName into ordGroup
                where ordGroup.Sum(x=>x.Total)> totalVolume
                select new
                {
                    CompanyName = ordGroup.Key,
                    TotalOrd=ordGroup.Sum(x=>x.Total)
                };
            ObjectDumper.Write($"Total volume is {totalVolume}");
            foreach (var cl in clients)
            {
                ObjectDumper.Write(cl.CompanyName+" "+cl.TotalOrd);
            }

            totalVolume = 100;
            ObjectDumper.Write($"Total volume is {totalVolume}");
            foreach (var cl in clients)
            {
                ObjectDumper.Write(cl.CompanyName + " " + cl.TotalOrd);
            }

        }

        [Category("Task")]
        [Title("Linq021")]
        [Description("This sample returns correspondence list of customers and suppliers locations")]

        public void Linq021()
        {
            var list =
                from cust in dataSource.Customers
                join sup in dataSource.Suppliers on new {cust.City, cust.Country} equals new {sup.City, sup.Country} 
                select new {cust.CompanyName, sup.SupplierName, cust.City, cust.Country};

            foreach (var item in list)
            {
                ObjectDumper.Write(item);
            }
        }

        [Category("Task")]
        [Title("Linq022")]
        [Description("This sample returns correspondence list of customers and suppliers locations")]

        public void Linq022()
        {
            var list = dataSource.Customers.GroupJoin(
                dataSource.Suppliers,
                c => new { c.City, c.Country },
                s => new { s.City, s.Country },
                (c, s) => new
                {
                    Customer = c,
                    Suppliers = s
                });

            foreach (var item in list)
            {
                ObjectDumper.Write($"CustomerName: {item.Customer.CompanyName} " + $"Supplier: {string.Join(", ", item.Suppliers.Select(s => s.SupplierName))}");
            }
        }

        [Category("Task")]
        [Title("Linq004")]
        [Description("This sample returns correspondence list of customers and suppliers locations")]

        public void Linq004()
        {
            var list = dataSource.Customers.Where(c=>c.Orders.Any())
                .Select(
                c => new
                {
                    CustomerID = c.CustomerID,
                    FirstOrder = c.Orders.OrderBy(t => t.OrderDate).First()
                }
                );      
            
            foreach (var item in list)
            {
                ObjectDumper.Write($"Customer: {item.CustomerID}    First order: Month = {item.FirstOrder.OrderDate.Month} Year = {item.FirstOrder.OrderDate.Year}");
            }
        }

        [Category("Task")]
        [Title("Linq005")]
        [Description("This sample returns correspondence list of customers and suppliers locations")]

        public void Linq005()
        {
            var list = dataSource.Customers.Where(c => c.Orders.Any())
                .Select(
                c => new
                {
                    TotalOrders=c.Orders.Sum(t=>t.Total),
                    CustomerID = c.CustomerID,
                    FirstOrder = c.Orders.OrderBy(t => t.OrderDate).First()
                }
                ).OrderByDescending(c=>c.FirstOrder.OrderDate.Year)
                .ThenByDescending(c => c.FirstOrder.OrderDate.Month)
                .ThenByDescending(c => c.TotalOrders)
                .ThenByDescending(c => c.CustomerID);

            foreach (var item in list)
            {
                ObjectDumper.Write($"First order: Year = {item.FirstOrder.OrderDate.Year} Month = {item.FirstOrder.OrderDate.Month} TotalOrders = {item.TotalOrders} Customer: {item.CustomerID}");
            }
        }

        [Category("Task")]
        [Title("Linq006")]
        [Description("This sample returns correspondence list of customers and suppliers locations")]

        public void Linq006()
        {
            char[] arrayOfNum = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            var list = dataSource.Customers.Where
                (
                    c =>(c.PostalCode!=null) && (c.PostalCode.IndexOfAny(arrayOfNum) ==-1)
                    ||(c.Region==null)||(c.Phone.StartsWith("("))
                )
                .Select(
                    c => new
                    {
                        CustomerName = c.CustomerID,
                    }                    
                );

            foreach (var item in list)
            {
               ObjectDumper.Write(item.CustomerName);
            }
        }

        [Category("Task")]
        [Title("Linq007")]
        [Description("This sample returns correspondence list of customers and suppliers locations")]

        public void Linq007()
        {
            var list = from prod in dataSource.Products
                       group prod by prod.Category into catGroup
                       from inStockGroup in
                       (
                           from prod in catGroup
                           group prod by prod.UnitsInStock into inStockGroup
                           from priceGroup in
                           (
                                from prod in inStockGroup
                                group prod by prod.UnitPrice
                           )
                           group inStockGroup by catGroup.Key
                        )
                       group catGroup by catGroup.Key;

            foreach (var item in list)
            {
                ObjectDumper.Write($"Category: {item.Key}");
                foreach (var a in item)
                {
                    ObjectDumper.Write($"\tUnitsInStock: {a.Key}");
                    foreach (var b in a)
                        ObjectDumper.Write($"\t\tUnitPrice: {b.UnitPrice}");
                }

            }
        }

        [Category("Task")]
        [Title("Linq009")]
        [Description("This sample returns correspondence list of customers and suppliers locations")]

        public void Linq009()
        {
            var list = dataSource.Customers.GroupBy
                (
                    c => c.City
                )
                .Select(c => new
                {
                    City = c.Key,
                    Profitable = c.Average(p=>p.Orders.Sum(d=>d.Total)),
                    Intensity=c.Average(p=>p.Orders.Length)
                }                  
                );

            foreach (var item in list)
            {
                ObjectDumper.Write($"City: {item.City}\t Profitable = {item.Profitable}\t Intensity={item.Intensity}");
            }
        }

        [Category("Task")]
        [Title("Linq0010")]
        [Description("This sample returns correspondence list of customers and suppliers locations")]

        public void Linq0010()
        {
            var list = dataSource.Customers.Select(c => new
            {
                Customer = c.CustomerID,
                ByMonth = c.Orders.GroupBy(d=>d.OrderDate.Month).Select(b=> new { Month=b.Key, OrdCount=b.Count()}),
                ByYear = c.Orders.GroupBy(d => d.OrderDate.Year).Select(b => new { Year = b.Key, OrdCount = b.Count() }),
                ByYearAndMonth = c.Orders.GroupBy(d => new { d.OrderDate.Year, d.OrderDate.Month })
                .Select(b => new { Year=b.Key.Year, Month = b.Key.Month, OrdCount = b.Count() }),
            }

            );

            foreach (var item in list)
            {
                ObjectDumper.Write($"Customer: {item.Customer}");
                ObjectDumper.Write("\tMonths statistic:\n");
                foreach (var m in item.ByMonth)
                {
                    ObjectDumper.Write($"\t\tMonth: {m.Month} Orders count: {m.OrdCount}");
                }
                ObjectDumper.Write("\tYears statistic:\n");
                foreach (var y in item.ByYear)
                {
                    ObjectDumper.Write($"\t\tYear: {y.Year} Orders count: {y.OrdCount}");
                }
                ObjectDumper.Write("\tYear and month statistic:\n");
                foreach (var ym in item.ByYearAndMonth)
                {
                    ObjectDumper.Write($"\t\tYear: {ym.Year} Month: {ym.Month} Orders count: {ym.OrdCount}");
                }
            }
        }




    }
}
