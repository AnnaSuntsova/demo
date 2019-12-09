using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileWatcher
{
    public class WatcherActions
    {
        public event EventHandler OnFileCreated;
        public event EventHandler OnRuleFound;
        public event EventHandler OnRuleNotFound;
        public event EventHandler OnFileMovedToDestinstion;
        public event EventHandler OnFileMovedToDefault;

        static UserConfigSection configSection = (UserConfigSection)ConfigurationManager.GetSection("customSection");
        static int _numOfFiles;
        private static string _defFolder=configSection.defaultFolder;

        public void RunWatching()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(configSection.Culture);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(configSection.Culture);
            foreach (FolderElement folder in configSection.FolderItems)
            {
                var tempPath = Path.Combine(Path.GetTempPath(), folder.Name);
                if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
                var systemWatchers = new FileSystemWatcher
                {
                    Path = tempPath,
                    EnableRaisingEvents = true
                };
                systemWatchers.Created += SystemWatcher_Created;
            }
        }

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

        public void SystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            var isRuleFound = false;
            OnFileCreated?.Invoke(this, null);
            foreach (RuleElement rule in configSection.RulesForFiles)
            {
                if (Regex.IsMatch(e.Name, rule.Rule, RegexOptions.IgnoreCase))
                {
                    OnRuleFound?.Invoke(this, null);
                    MoveToNewPath(e.FullPath, rule.Destination, rule.AddDate, rule.AddNumber);
                    OnFileMovedToDestinstion?.Invoke(this, null);
                    isRuleFound = true;
                    break;
                }
            }
            if (!isRuleFound)
            {
                OnRuleNotFound?.Invoke(this, null);
                MoveToNewPath(e.FullPath, _defFolder, false, false);
                OnFileMovedToDefault?.Invoke(this, null);
            }
        }
    }
}
