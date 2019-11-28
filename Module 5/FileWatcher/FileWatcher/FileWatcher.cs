using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using FileWatcher.Resources;

namespace FileWatcher
{
    class FileWatcher
    {
        static void Main(string[] args)
        {
            RunWatching();
        }

        private static void RunWatching()
        {
            var systemWatcher=new FileSystemWatcher();
            systemWatcher.Path = @"D:\Info";
            systemWatcher.NotifyFilter = NotifyFilters.LastAccess;
            systemWatcher.Filter = "*.*";
            systemWatcher.Created += SystemWatcher_Created;
            systemWatcher.EnableRaisingEvents = true;
        }

        private static void SystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            //var rm= new ResourceManager ()
            //Console.WriteLine(ResourceEn.FileCreated);
            var newPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(e.Name));

            //поиск правила
            if (false) Console.Write(ResourceEn.RuleNotFound);
            else
            {
                Console.WriteLine(ResourceEn.RuleFound);
                File.Move(e.Name, newPath);
                Console.WriteLine(ResourceEn.FileTransferToDestination);
            }
        }
    }
}
