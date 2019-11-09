using System;

namespace Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your name: ");
            var name=Console.ReadLine();
            Console.WriteLine($"Hello, {name}!");
        }
    }
}
