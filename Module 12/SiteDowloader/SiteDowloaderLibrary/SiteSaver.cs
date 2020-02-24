using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDownloaderLibrary
{
    public class SiteSaver
    {
        private string _rootDirectory;
        private Logger _logger;
        private bool _tracingMode;

        public SiteSaver(string rootDirectory, Logger logger, bool tracingMode)
        {
            _rootDirectory = rootDirectory;
            _logger = logger;
            _tracingMode = tracingMode;
        }

        public void Save(Stream contentStream, string fileName)
        {
            var filePath = Path.Combine(_rootDirectory, GetValidPathName(fileName));
            if (!File.Exists(filePath))
            {
                SaveToFile(filePath, contentStream);
                _logger.Log($"Save content to: {filePath}",  _tracingMode);
            }
        }

        private string GetValidPathName(string fileName)
        {
            return string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
            //return string.Concat(fileName.Where(s => Path.GetInvalidFileNameChars().All(invalidChar => invalidChar != s)));
        }

        private void SaveToFile(string path, Stream content)
        {
            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                content.Seek(0, SeekOrigin.Begin);
                content.CopyTo(fileStream);
            }
        }
    }
}
