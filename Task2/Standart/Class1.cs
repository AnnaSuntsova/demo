using System;

namespace Standart
{
    public class Class1
    {
        public static string Hello(string name)
        {
            return $"{DateTime.Now.ToString("HH:mm:ss")} Hello, {name}!";
        }
    }
}
