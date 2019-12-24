using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var path="";
            using (var fsFileStream = File.Open(path, FileMode.Open))
            {
                var array = new byte[fsFileStream.Length];
                fsFileStream.Read(array, 0, array.Length);
                var textFromFile = Encoding.Default.GetString(array);
                
                var action = new Actions();
                action.CheckValidation();
                Console.ReadKey();

                var action1 = new Actions();
                action1.CheckValidation();
                Console.ReadKey();
            }
        }
    }
}
