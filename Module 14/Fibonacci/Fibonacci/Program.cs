using System;

namespace Fibonacci
{
    class Program
    {
        static void Main(string[] args)
        {   
            Console.WriteLine("Variant 1. Memory cache");
            var calc = new NumberCalculation(new FibonacciMemoryCache());
            for (int index = 1; index<=10; index++)
                Console.Write($"{ calc.Calculate(index)} ");


            Console.WriteLine("\nVariant 2. Redis cache");
            calc = new NumberCalculation(new FibonacciRedisCache("localhost"));
            for (int index = 1; index <= 10; index++)
                Console.Write($"{ calc.Calculate(index)} ");

            Console.ReadKey();
        }
    }
}
