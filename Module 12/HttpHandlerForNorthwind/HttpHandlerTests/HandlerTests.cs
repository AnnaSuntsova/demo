using HttpHandlerForNorthwind;
using HttpHandlerForNorthwind.Db;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace HttpHandlerTests
{
    [TestFixture]
    public class HandlerTests
    {
        private const string ExcelAcceptType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private const string TextXmlAcceptType = "text/xml";
        private const string ApplicationXmlAcceptType = "application/xml";
        private const string DefaultAcceptType = "custom/type";

        private Northwind _context;
        private ReportHandler _handler;
        private HttpClient _client;
        private UriBuilder _uriBuilder;
        private Converters _converter;

        public HandlerTests()
        {
            _context = new Northwind();
            var parser = new Parser();
            var converter = new Converters();
            _handler = new ReportHandler(parser, converter);
            var listenerThread = new Thread(_handler.Listen);
            listenerThread.Start();
            _client = new HttpClient();
            _uriBuilder = new UriBuilder("http://localhost:55177/");
            _uriBuilder.Query = "customerId=VINET";
            _converter = new Converters();
        }

        [Test]
        public void CheckExcelAcceptType()
        {
            _client.DefaultRequestHeaders.Remove("Accept");
            _client.DefaultRequestHeaders.Add("Accept", ExcelAcceptType);
            var response = _client.GetAsync(_uriBuilder.Uri).Result;
            var contentType = response.Content.Headers.ContentType.MediaType;

            Assert.AreEqual(true, response.IsSuccessStatusCode);
            Assert.AreEqual(ExcelAcceptType, contentType);
        }

        [Test]
        public void CheckTextXmlAcceptType()
        {
            _client.DefaultRequestHeaders.Remove("Accept");
            _client.DefaultRequestHeaders.Add("Accept", TextXmlAcceptType);
            var response = _client.GetAsync(_uriBuilder.Uri).Result;
            var contentType = response.Content.Headers.ContentType.MediaType;

            using (var stream = response.Content.ReadAsStreamAsync().Result)
            {
                var data = _converter.FromXmlFormat(stream);
                Assert.True(data.Any());
            }

            Assert.AreEqual(true, response.IsSuccessStatusCode);
            Assert.AreEqual(TextXmlAcceptType, contentType);
        }

        [Test]
        public void CheckApplicationXmlAcceptType()
        {
            _client.DefaultRequestHeaders.Remove("Accept");
            _client.DefaultRequestHeaders.Add("Accept", ApplicationXmlAcceptType);
            var response = _client.GetAsync(_uriBuilder.Uri).Result;
            var contentType = response.Content.Headers.ContentType.MediaType;

            using (var stream = response.Content.ReadAsStreamAsync().Result)
            {
                var data = _converter.FromXmlFormat(stream);
                Assert.True(data.Any());
            }

            Assert.AreEqual(true, response.IsSuccessStatusCode);
            Assert.AreEqual(ApplicationXmlAcceptType, contentType);
        }

        [Test]
        public void CheckDefaultAcceptType()
        {
            _client.DefaultRequestHeaders.Remove("Accept");
            _client.DefaultRequestHeaders.Add("Accept", DefaultAcceptType);
            var response = _client.GetAsync(_uriBuilder.Uri).Result;
            var contentType = response.Content.Headers.ContentType.MediaType;

            Assert.AreEqual(true, response.IsSuccessStatusCode);
            Assert.AreEqual(ExcelAcceptType, contentType);
        }
    }
}