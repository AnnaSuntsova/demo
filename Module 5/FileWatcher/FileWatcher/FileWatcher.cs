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
using System.Text.RegularExpressions;

namespace FileWatcher
{
    class FileWatcher
    {
        static UserConfigSection configSection = (UserConfigSection)ConfigurationManager.GetSection("customSection");

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(configSection.Culture);
            RunWatching();
            while (true) { }
        }       

        private static void RunWatching()
        {
            var numOfWatch = configSection.FolderItems.Count;
            var num = 0;
            foreach (FolderElement folder in configSection.FolderItems)
            {
                var tempPath = Path.Combine(Path.GetTempPath(), folder.Name);
                if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
                FileSystemWatcher systemWatchers = new FileSystemWatcher();
                systemWatchers.Path = tempPath;
                systemWatchers.EnableRaisingEvents = true;
                systemWatchers.Created += SystemWatcher_Created;
                num++;
            }
        }

        private static void SystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            string newPath;
            var isRuleFound = false;
            Console.WriteLine(resources.FileCreated);

            foreach (RuleElement rule in configSection.RulesForFiles)
            {
                if (Regex.IsMatch(e.Name, rule.Rule, RegexOptions.IgnoreCase))
                {
                    Console.WriteLine(resources.RuleFound);
                    newPath = Path.Combine(Path.GetTempPath(), rule.Destination);
                    if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);                                       
                    File.Move(e.Name, newPath);
                    Console.WriteLine(resources.FileMoveToDestination);
                    isRuleFound = true;
                }               
            }
            if (!isRuleFound)
            {
                Console.WriteLine(resources.RuleNotFound);
                newPath = Path.Combine(Path.GetTempPath(), "TempPath");
                if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);
                var newFileName = Path.Combine(newPath, e.Name);
                if (File.Exists(newFileName)) File.Delete(newFileName);
                File.Move(e.FullPath, newFileName);
                Console.WriteLine(resources.FileMoveToDefaultFolder);
            }
        }
    }
}
