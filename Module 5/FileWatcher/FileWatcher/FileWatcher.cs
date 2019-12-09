using System;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using resources = FileWatcher.Resources.Resource;

namespace FileWatcher
{
    class FileWatcher
    {
        static UserConfigSection configSection = (UserConfigSection)ConfigurationManager.GetSection("customSection");
        static void Main(string[] args)
        {
            var watcherAct = new WatcherActions();
            watcherAct.OnFileCreated += WatcherAct_OnFileCreated;
            watcherAct.OnRuleFound += WatcherAct_OnRuleFound;
            watcherAct.OnRuleNotFound += WatcherAct_OnRuleNotFound;
            watcherAct.OnFileMovedToDefault += WatcherAct_OnFileMovedToDefault;
            watcherAct.OnFileMovedToDestinstion += WatcherAct_OnFileMovedToDestinstion;
            watcherAct.RunWatching();
            while (true) { }
        }

        private static void WatcherAct_OnFileMovedToDestinstion(object sender, EventArgs e)
        {
            Console.WriteLine(resources.FileMoveToDestination);
        }

        private static void WatcherAct_OnFileMovedToDefault(object sender, EventArgs e)
        {
            Console.WriteLine(resources.FileMoveToDefaultFolder);
        }

        private static void WatcherAct_OnRuleNotFound(object sender, EventArgs e)
        {
            Console.WriteLine(resources.RuleNotFound);
        }

        private static void WatcherAct_OnRuleFound(object sender, EventArgs e)
        {
            Console.WriteLine(resources.RuleFound);
        }

        private static void WatcherAct_OnFileCreated(object sender, EventArgs e)
        {
            Console.WriteLine(resources.FileCreated);
        }
    }
}
