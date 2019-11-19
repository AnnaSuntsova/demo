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
            const decimal totalVolume = 10000;
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
            foreach (var cl in clients)
            {
                ObjectDumper.Write(cl.CompanyName+" "+cl.TotalOrd);
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
                ObjectDumper.Write($"CustomerName: {item.Customer.CompanyName} " + $"List of suppliers: {string.Join(", ", item.Suppliers.Select(s => s.SupplierName))}");
            }

            //var result = dataSource.Customers.GroupJoin(dataSource.Suppliers,
            //    c => new { c.City, c.Country },
            //    s => new { s.City, s.Country },
            //    (c, s) => new { Customer = c, Suppliers = s });

            //ObjectDumper.Write("With  grouping:\n");
            //foreach (var c in result)
            //{
            //    ObjectDumper.Write($"CustomerId: {c.Customer.CustomerID} " +
            //                       $"List of suppliers: {string.Join(", ", c.Suppliers.Select(s => s.SupplierName))}");
            //}
        }

    }
}
