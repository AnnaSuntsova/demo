using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using FileWatcher.Resources;
using System.Configuration;

namespace FileWatcher
{
    class FileWatcher
    {
        static void Main(string[] args)
        {
            var configSection = (UserConfigSection)ConfigurationManager.GetSection("simpleSection");

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
            var systemWatcher=new FileSystemWatcher();
            systemWatcher.Path = @"D:\Info";
            systemWatcher.NotifyFilter = NotifyFilters.LastAccess;
            systemWatcher.Filter = "*.*";
            systemWatcher.Created += SystemWatcher_Created;
            systemWatcher.EnableRaisingEvents = true;
        }

        
        private static void SystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            var configSection = (UserConfigSection)ConfigurationManager.GetSection("simpleSection");
            //var rm= new ResourceManager ()
            //Console.WriteLine(ResourceEn.FileCreated);
            var newPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(e.Name));

            //поиск правила
            foreach (var rule in configSection.RulesForFiles)
            {
                    
            }

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
