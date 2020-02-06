using System;

namespace Fibonacci
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter number position:");
            int index;
            if (int.TryParse(Console.ReadLine(), out int result))
                index = result;
            else
            {
                throw new ArgumentOutOfRangeException("Position must be an integer");
            }
            if (index < 1)
                Console.WriteLine("Position must be a positive integer");

            Console.WriteLine("Variant 1. Memory cache");
            var calc = new NumberCalculation(new FibonacciMemoryCache());
            Console.WriteLine($"Result is {calc.Calculate(index)}");


            Console.WriteLine("Variant 2. Redis cache");
            calc = new NumberCalculation(new FibonacciRedisCache("localhost"));
            Console.WriteLine($"Result is {calc.Calculate(index)}");

            Console.ReadKey();
        }
    }
}
