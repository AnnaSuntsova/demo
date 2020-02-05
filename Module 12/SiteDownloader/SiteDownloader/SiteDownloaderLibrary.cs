using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text;
using System.IO;
using SiteDownloader.Interfaces;

namespace SiteDownloader
{
    public class SiteDownloaderLibrary
    {
        public int MaxDeepLevel { get; set; }
        private List<Uri> _visitedUrls;
        //private ILogger _logger;
        //private IContentSaver _contentSaver;
        //private List<IConstraint> _fileConstraints;
        private List<IConstraint> _urlConstraints;
        private const string HtmlDocumentMediaType = "text/html";


        public SiteDownloaderLibrary(int maxDeep)
        {
            MaxDeepLevel = maxDeep;
            _visitedUrls = new List<Uri>();
        }

        public void LoadFromUrl (string url)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                ScanUrl(httpClient, httpClient.BaseAddress, 0);
            }
        }

        private void ScanUrl(HttpClient httpClient, Uri uri, int level)
        {
            if (level > MaxDeepLevel || _visitedUrls.Contains(uri))
            {
                return;
            }
            _visitedUrls.Add(uri);
            HttpResponseMessage head = httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri)).Result;

            if (!head.IsSuccessStatusCode)
            {
                return;
            }

            if (head.Content.Headers.ContentType?.MediaType == HtmlDocumentMediaType)
            {
                ProcessHtmlDocument(httpClient, uri, level);
            }
            else
            {
                ProcessFile(httpClient, uri);
            }
        }

        private void ProcessFile(HttpClient httpClient, Uri uri)
        {
            _logger.Log($"File founded: {uri}");
            if (!IsAcceptableUri(uri, _fileConstraints))
            {
                return;
            }

            var response = httpClient.GetAsync(uri).Result;
            _logger.Log($"File loaded: {uri}");
            _contentSaver.SaveFile(uri, response.Content.ReadAsStreamAsync().Result);
        }

        private void ProcessHtmlDocument(HttpClient httpClient, Uri uri, int level)
        {
            //_logger.Log($"Url founded: {uri}");
            if (!IsAcceptableUri(uri, _urlConstraints))
            {
                return;
            }

            var response = httpClient.GetAsync(uri).Result;
            var document = new HtmlDocument();
            document.Load(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8);
            //_logger.Log($"Html loaded: {uri}");
            _contentSaver.SaveHtmlDocument(uri, GetDocumentFileName(document), GetDocumentStream(document));

            var attributesWithLinks = document.DocumentNode.Descendants().SelectMany(d => d.Attributes.Where(IsAttributeWithLink));
            foreach (var attributesWithLink in attributesWithLinks)
            {
                ScanUrl(httpClient, new Uri(httpClient.BaseAddress, attributesWithLink.Value), level + 1);
            }            
        }

        private bool IsAttributeWithLink(HtmlAttribute attribute)
        {
            return attribute.Name == "src" || attribute.Name == "href";
        }

        private bool IsAcceptableUri(Uri uri, IEnumerable<IConstraint> constraints)
        {
            return constraints.All(c => c.IsAcceptable(uri));
        }


        private string GetDocumentFileName(HtmlDocument document)
        {
            return document.DocumentNode.Descendants("title").FirstOrDefault()?.InnerText + ".html";
        }

        private Stream GetDocumentStream(HtmlDocument document)
        {
            MemoryStream memoryStream = new MemoryStream();
            document.Save(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

    }
}