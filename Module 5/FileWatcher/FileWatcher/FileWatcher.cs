using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
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
        static int numOfFiles = 0;

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-EN");
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

        static void MoveToNewPath (string nameOfFile, string destin, bool addDate, bool addNumber)
        {
            numOfFiles++;
            var tempName = Path.GetFileName(nameOfFile);
            var newPath = Path.Combine(Path.GetTempPath(), destin);
            if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);
            if (addDate) tempName = string.Concat(Path.GetFileNameWithoutExtension(nameOfFile),"_", DateTimeOffset.Now.Date.ToShortDateString(), Path.GetExtension(nameOfFile));
            if (addNumber) tempName = string.Concat(Path.GetFileNameWithoutExtension(nameOfFile),"_", numOfFiles.ToString(), Path.GetExtension(nameOfFile));
            newPath = Path.Combine(newPath, tempName);
            if (File.Exists(newPath)) File.Delete(newPath);
            File.Move(Path.GetFullPath(nameOfFile), newPath);
        }

        private static void SystemWatcher_Created(object sender, FileSystemEventArgs e)
        {            
            var isRuleFound = false;
            Console.WriteLine(resources.FileCreated);
            foreach (RuleElement rule in configSection.RulesForFiles)
            {
                if (Regex.IsMatch(e.Name, rule.Rule, RegexOptions.IgnoreCase))
                {
                    Console.WriteLine(resources.RuleFound);
                    MoveToNewPath(e.FullPath, rule.Destination, rule.AddDate, rule.AddNumber);
                    Console.WriteLine(resources.FileMoveToDestination);
                    isRuleFound = true;
                    break;
                }               
            }
            if (!isRuleFound)
            {
                Console.WriteLine(resources.RuleNotFound);
                MoveToNewPath(e.FullPath, "TempPath", false, false);
                Console.WriteLine(resources.FileMoveToDefaultFolder);
            }
        }
    }
}
