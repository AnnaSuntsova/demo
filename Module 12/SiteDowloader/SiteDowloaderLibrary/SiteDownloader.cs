using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SiteDownloaderLibrary
{
    public class SiteDownloader
    {
        private Logger _logger;
        private SiteSaver _siteSaver;
        private bool _tracingMode;
        private Validator _fileValidator;

        private const string HtmlTypeContent = "text/html";


        public SiteDownloader(Logger logger, SiteSaver siteSaver, Validator  fileValidator, bool tracingMode)
        {
            _logger = logger;
            _siteSaver = siteSaver;
            _tracingMode = tracingMode;
            _fileValidator = fileValidator;
        }

        public async Task<Stream> DownloadAsync(Uri uri)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var responseMessage = await client.GetAsync(uri);

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        return null;
                    }

                    var content = await responseMessage.Content.ReadAsStreamAsync();
                    string fileName;
                    if (responseMessage.Content.Headers.ContentType.MediaType.Equals(HtmlTypeContent))
                    {
                        fileName = GetHtmlFileName(content);
                    }
                    else
                    {
                        fileName = GetFileName(uri);
                    }                        

                    if (fileName != null)
                    {
                        _siteSaver.Save(content, fileName);
                    }

                    content.Seek(0, SeekOrigin.Begin);
                    return content;
                }
                catch (Exception e)
                {
                    _logger.Log(e.Message, _tracingMode);
                }
            }

            return null;
        }

        private string GetHtmlFileName(Stream content)
        {
            var document = new HtmlDocument();

            try
            {
                document.Load(content, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var titles = document.DocumentNode.Descendants("title").ToArray();
            if (titles.Any())
            {
                return $"{titles.First().InnerText}.html";
            }
            else
            {
                return null;
            }
        }

        private string GetFileName(Uri uri)
        {
            return _fileValidator.IsValid(uri) ? uri.Segments.Last() : null;
        }  
    }
}
