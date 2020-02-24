using HttpHandlerForNorthwind;
using HttpHandlerForNorthwind.Db;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;

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

        public HandlerTests ()
        {
            //_context = new Northwind();
            //var parser = new Parser();
            //var converter = new Converters();
            //_handler = new ReportHandler(parser, converter);
            ////var listenerThread = new Thread(_service.Listen);
            ////listenerThread.Start();
            //_client = new HttpClient();
            //_uriBuilder = new UriBuilder("http://localhost:81");
            //_uriBuilder.Query = "customerId=VINET";
            //_converter = new Converters();
        }

        [Test]
        public void ExcelHandle()
        {
            //var parser = new Parser();
            //var stringBuilder = new StringBuilder();
            //using (var sw = new StringWriter())
            //{
            //    var response = new HttpResponse(sw);


            //    var request = WebRequest.CreateHttp(_uriBuilder.Uri);
            //    var context = new HttpContext(request., response);
            //    new ReportHandler(parser, _converter).ProcessRequest(context);
            //}

            // Create a request using a URL that can receive a post.   
            //Uri myUri = new Uri("http://localhost:43740/");
            //// Create a new request to the above mentioned URL.	
            //WebRequest request = WebRequest.Create(myUri);
            //WebRequest request = WebRequest.Create("http://localhost:43740/");
            // Set the Method property of the request to POST.  
            WebRequest request = WebRequest.Create("http://www.contoso.com/PostAccepter.aspx");
            request.Method = "POST";

            // Create POST data and convert it to a byte array.  
            string postData = "This is a test that posts this string to a Web server.";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.  
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.  
            request.ContentLength = byteArray.Length;

            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.  
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.  
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.  
            // The using block ensures the stream is automatically closed.
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Console.WriteLine(responseFromServer);
            }

            // Close the response.  
            response.Close();
        }
    }
}