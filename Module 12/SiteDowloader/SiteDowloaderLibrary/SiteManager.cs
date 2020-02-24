using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDownloaderLibrary
{
    public class SiteManager
    {
        private SiteDownloader _siteDownloader;
        private Logger _logger;
        private bool _tracingMode;
        private Validator _validator;

        public SiteManager(
            SiteDownloader siteDownloader,
            Logger logger,
            Validator validator,
            bool tracingMode)
        {
            _siteDownloader = siteDownloader;
            _logger = logger;
            _validator = validator;
            _tracingMode = tracingMode;
        }

        public void Start(IEnumerable<Uri> uries, int deepLevel, int countLevel)
        {
            _logger.Log($"Level {countLevel}", _tracingMode);
            var links = Analyze(uries, countLevel).ToArray();

            if (!links.Any()) return;

            countLevel++;

            if (countLevel > deepLevel) return;

            Start(links, countLevel, deepLevel);
        }

        private IEnumerable<Uri> Analyze(IEnumerable<Uri> uries, int currentLevel)
        {
            var workLinks = new HashSet<Uri>();

            try
            {
                var content = Task.WhenAll(uries
                        .Select(url => _siteDownloader.DownloadAsync(url)
                        .ContinueWith(task => GetLinks(task.Result))
                    )).ContinueWith(task =>
                    {
                        foreach (var link in task.Result)
                        {
                            workLinks.UnionWith(link);
                        }

                        return workLinks;
                    });

                workLinks = content.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error source: {ex.Source}, Error message: {ex.Message}");
            }

            return workLinks;
        }

        private IEnumerable<Uri> GetLinks(Stream content)
        {
            if (content == null)
            {
                return new Uri[0];
            }

            var document = new HtmlDocument();

            try
            {
                document.Load(content, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return document.DocumentNode
                .Descendants()
                .SelectMany(d => d.Attributes.Where(IsValidLink))
                .Where(uri => _validator.IsValid(uri.Value))
                .Select(link => new Uri(link.Value));
        }

        private bool IsValidLink(HtmlAttribute attribute)
        {
            return attribute.Name == "src" || attribute.Name == "href";
        }        
    }
}
