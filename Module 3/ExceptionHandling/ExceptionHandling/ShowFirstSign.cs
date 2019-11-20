using System;

namespace ExceptionHandling
{
    class ShowFirstSign
    {
        private static void Main()
        {
            while (true)
            {
                char sign;
                Console.Write("Enter a line: ");
                var strOfConsole = Console.ReadLine();

                var firstSign= new GetFirstSign.GetFirstSign();
                try
                {
                    sign = firstSign.GetSign(strOfConsole);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Your line is null or empty!");
                    continue;
                }

                Console.WriteLine($"First sign is: {sign}");
            }
        }
    }
}
