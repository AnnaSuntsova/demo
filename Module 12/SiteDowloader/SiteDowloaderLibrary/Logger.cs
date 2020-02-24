using System;

namespace SiteDownloaderLibrary
{
    public class Logger
    {
        public void Log(string message, bool tracingMode)
        {
            if (tracingMode)
            {
                Console.WriteLine(message);
            }
        }
    }
}
