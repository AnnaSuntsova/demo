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
            string startPosition;

            Console.WriteLine("Введите начальный каталог:");
            startPosition = Console.ReadLine();
            if (!Directory.Exists(startPosition))
            {
                Console.WriteLine("Задан неверный каталог!");
            }
            else
            {
                FileSystemVisitor.FileSystemVisitor systemVisitor = new FileSystemVisitor.FileSystemVisitor(startPosition, CheckFiles);

                systemVisitor.OnStart += SystemVisitor_OnStart;
                systemVisitor.OnFinish += SystemVisitor_OnFinish;
                systemVisitor.OnFileFinded += SystemVisitor_OnFileFinded;
                systemVisitor.OnDirectoryFinded += SystemVisitor_OnDirectoryFinded;
                systemVisitor.OnFilteredDirectoryFinded += SystemVisitor_OnFilteredDirectoryFinded; ;
                systemVisitor.OnFilteredFileFinded += SystemVisitor_OnFilteredFileFinded; ;
                systemVisitor.Searching();
            }
            Console.ReadLine();
        }

        private static void SystemVisitor_OnFilteredFileFinded(string itemName, FileSystemVisitor.FileSystemVisitor.CharachteristicsOfItems arg)
        {
            arg.cancelSearch = false;
            if (itemName.IndexOf("s")==-1) arg.excludeItem = true;
            else Console.WriteLine($"FilteredFile found: " + itemName);
        }

        private static void SystemVisitor_OnFilteredDirectoryFinded(string itemName, FileSystemVisitor.FileSystemVisitor.CharachteristicsOfItems arg)
        {
            arg.cancelSearch = true;
            if (itemName.IndexOf("12") == -1) arg.excludeItem = true;
            else Console.WriteLine($"FilteredDirectory found: " + itemName);
        }

        private static void SystemVisitor_OnDirectoryFinded(object sender, string e)
        {
            Console.WriteLine($"Directory found: " + e);
        }

        private static bool CheckFiles(string itemName)
        {
            if (itemName.IndexOf(".txt")!=-1) return true;
            else return false;
        }

        private static void SystemVisitor_OnFileFinded(object sender, string e)
        {
            Console.WriteLine($"File found: " + e);
        }

        private static void SystemVisitor_OnFinish(object sender, EventArgs e)
        {
            Console.WriteLine("Finish of searching");
        }

        private static void SystemVisitor_OnStart(object sender, EventArgs e)
        {
            Console.WriteLine("Start of searching");
        }

        private static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
