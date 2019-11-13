using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleAppForFileSystemVisitor
{
    class Program
    {
        static void Main(string[] args)
        {
            string startPosition, searchDir, searchFile;
            Console.WriteLine("Введите начальный каталог:");
            startPosition = Console.ReadLine();
            if (!Directory.Exists(startPosition))
            {
                Console.WriteLine("Задан неверный каталог!");
            }
            else
            {
                Console.WriteLine("Введите условие поиска директорий (* - поиск без ограничений):");
                searchDir = Console.ReadLine();

                Console.WriteLine("Введите условие поиска файлов (* - поиск без ограничений):");
                searchFile = Console.ReadLine();

                Console.WriteLine("Прервать поиск, если файлы/директории найдены? (+/-)");
                string interruptSearch = Console.ReadLine();

                Console.WriteLine("Исключить файлы/папки из конечного списка? (+/-)");
                string ignoreItems = Console.ReadLine();

                if ((searchDir=="*")&&(searchFile=="*"))
                {
                    interruptSearch = "-";
                    ignoreItems = "-";
                }
                                             
                FileSystemVisitor.FileSystemVisitor systemVisitor = new FileSystemVisitor.FileSystemVisitor(/*searchDir, searchFile, interruptSearch, ignoreItems*/);
                //systemVisitor.Notify += DisplayMessage;
                systemVisitor.Searching(startPosition);                
            }
            Console.ReadLine();
        }
        private static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
