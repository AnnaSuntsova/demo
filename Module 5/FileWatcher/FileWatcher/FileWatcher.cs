using System;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FileWatcher
{
    class FileWatcher
    {
        static UserConfigSection configSection = (UserConfigSection)ConfigurationManager.GetSection("customSection");
        static void Main(string[] args)
        {
            RunWatching();
            while (true) { }
        }
        public static void RunWatching()
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
                systemWatchers.Created += WatcherActions.SystemWatcher_Created;
            }
        }
    }
}
