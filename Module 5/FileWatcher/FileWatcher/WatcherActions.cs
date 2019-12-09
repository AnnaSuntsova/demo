using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using resources = FileWatcher.Resources.Resource;
using System.Threading.Tasks;

namespace FileWatcher
{
    public class WatcherActions
    {
        static UserConfigSection configSection = (UserConfigSection)ConfigurationManager.GetSection("customSection");
        static int _numOfFiles;
        private static string _defFolder=configSection.defaultFolder;

        private static void MoveToNewPath(string nameOfFile, string destin, bool addDate, bool addNumber)
        {
            _numOfFiles++;
            var tempName = Path.GetFileName(nameOfFile);
            var newPath = Path.Combine(Path.GetTempPath(), destin);
            if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);
            if (addDate) tempName = string.Concat(Path.GetFileNameWithoutExtension(nameOfFile), "_", DateTimeOffset.Now.Date.ToShortDateString(), Path.GetExtension(nameOfFile));
            if (addNumber) tempName = string.Concat(Path.GetFileNameWithoutExtension(nameOfFile), "_", _numOfFiles.ToString(), Path.GetExtension(nameOfFile));
            newPath = Path.Combine(newPath, tempName);
            newPath = newPath.Replace("/", ".");
            if (File.Exists(newPath)) File.Delete(newPath);
            File.Move(Path.GetFullPath(nameOfFile), newPath);
        }

        public static void SystemWatcher_Created(object sender, FileSystemEventArgs e)
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
                MoveToNewPath(e.FullPath, _defFolder, false, false);
                Console.WriteLine(resources.FileMoveToDefaultFolder);
            }
        }
    }
}
