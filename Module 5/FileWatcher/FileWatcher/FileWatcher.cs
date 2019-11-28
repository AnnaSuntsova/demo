using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using FileWatcher.Resources;
using System.Configuration;
using System.Globalization;
using System.Threading;
using resources = FileWatcher.Resources.Resource;

namespace FileWatcher
{
    class FileWatcher
    {
        static void Main(string[] args)
        {
            var configSection = (UserConfigSection)ConfigurationManager.GetSection("customSection");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(configSection.InterfaceCulture);
            RunWatching();
        }

        private class UserConfigSection : ConfigurationSection
        {
            [ConfigurationProperty("interfCulture")]
            public string InterfaceCulture => (string) base["interfCulture"];

            [ConfigurationProperty("observFolders")]
            public List<FileInfo> ObservingFolders => (List<FileInfo>)base["observFolders"];

            [ConfigurationProperty("rulesForFiles")]
            public List<FileInfo> RulesForFiles => (List<FileInfo>)base["rulesForFiles"];
        }        

        private static void RunWatching()
        {
            var systemWatcher = new FileSystemWatcher
            {
                Path = @"D:\Info",
                NotifyFilter = NotifyFilters.LastAccess,
                Filter = "*.*",
                EnableRaisingEvents = true
            };
            systemWatcher.Created += SystemWatcher_Created;
        }
        
        private static void SystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            string newPath;
            var configSection = (UserConfigSection)ConfigurationManager.GetSection("simpleSection");            
            Console.WriteLine(resources.FileCreated);            

            //поиск правила
            foreach (var rule in configSection.RulesForFiles)
            {
                    
            }

            if (false)
            {
                Console.Write(resources.RuleNotFound);
                newPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(e.Name));
            }
            else
            {
                Console.WriteLine(resources.RuleFound);
                newPath= Path.Combine(Path.GetTempPath(), Path.GetFileName(e.Name));
                File.Move(e.Name, newPath);
                Console.WriteLine(resources.FileTransferToDestination);
            }            
        }
    }
}
