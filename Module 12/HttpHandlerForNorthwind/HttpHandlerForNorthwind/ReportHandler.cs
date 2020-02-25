using HttpHandlerForNorthwind.Db;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Data.Entity;

namespace HttpHandlerForNorthwind
{    
    public class ReportHandler : IHttpHandler
    {
        private Parser _parser;
        private Converters _converter;

        public ReportHandler (Parser parser, Converters converter)
        {
            _parser = parser;
            _converter = converter;
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            Listen();            
        }

        public void Listen()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://+:55177/");
            listener.IgnoreWriteExceptions = true;
            listener.Start();
            do
            {
                var context = listener.GetContext();

                var request = context.Request;
                var response = context.Response;
                string acceptType;
                var orderParams = new OrderParameters();

                if (request.HttpMethod == "POST" && request.InputStream != null)
                {
                    orderParams = _parser.ParseBody(request.InputStream);
                }
                else
                {
                    var dataFromQuery = HttpUtility.ParseQueryString(request.Url.Query);
                    orderParams = _parser.ParseQuery(dataFromQuery);
                }

                if ((request.AcceptTypes == null) || (!request.AcceptTypes.Any()))
                {
                    acceptType = "unknown";
                }
                else
                {
                    acceptType = request.AcceptTypes.FirstOrDefault();
                }

                var data = GetData(orderParams);
                SendResponse(acceptType, data, response);
            } while (true);            
        }

        private void SendResponse(string acceptType, List<OrderFields> data, HttpListenerResponse response)
        {
            var memoryStream = new MemoryStream();
            if (acceptType != null)
            {
                switch (acceptType)
                {
                    case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                        {
                            _converter.ToExcel(data, memoryStream);
                            response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                            break;
                        }
                    case "text/xml":
                        {
                            _converter.ToXmlFormat(data, memoryStream);
                            response.AppendHeader("Content-Type", "text/xml");
                            break;
                        }
                    case "application/xml":
                        {
                            _converter.ToXmlFormat(data, memoryStream);
                            response.AppendHeader("Content-Type", "application/xml");
                            break;
                        }
                    default:
                        {
                            _converter.ToExcel(data, memoryStream);
                            response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                            break;
                        }
                }
                
                response.StatusCode = (int)HttpStatusCode.OK;

                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.WriteTo(response.OutputStream);
                memoryStream.Close();
                response.OutputStream.Close();
            }
        }

        private List<OrderFields> GetData(OrderParameters orderParams)
        {
            DateTime dateFrom, dateTo;
            var db = new Northwind(); 

            dateFrom = orderParams.DateFrom == null ? DateTime.MinValue : orderParams.DateFrom.Value;
            dateTo = orderParams.DateTo == null? DateTime.MaxValue: orderParams.DateTo.Value;

            var orderInfo = new List<OrderFields>();
            if (orderParams.Take != null && orderParams.Skip != null)
            {
                var query = db.Orders.Include(o => o.Customer)
                .Skip(orderParams.Skip.Value)
                .Take(orderParams.Take.Value)
                .Where(o => o.OrderDate > dateFrom && o.OrderDate < dateTo)
                .OrderBy(o => o.OrderID)
                .Select(o => new OrderFields
                {
                    Customer = o.Customer.CompanyName,
                    OrderDate = o.OrderDate,
                    Freight = o.Freight,
                    ShipCountry = o.ShipCountry,
                    ShipName = o.ShipName
                });
                orderInfo = query.ToList();
            }
            else
            {
                if (orderParams.Take == null && orderParams.Skip.HasValue)
                {
                    var query = db.Orders.Include(o => o.Customer)
                    .Skip(orderParams.Skip.Value)
                    .Where(o => o.OrderDate > dateFrom && o.OrderDate < dateTo)
                    .OrderBy(o => o.OrderID)
                    .Select(o => new OrderFields
                    {
                        Customer = o.Customer.CompanyName,
                        OrderDate = o.OrderDate,
                        Freight = o.Freight,
                        ShipCountry = o.ShipCountry,
                        ShipName = o.ShipName
                    });
                    orderInfo = query.ToList();
                }
                else
                {
                    if (orderParams.Skip == null && orderParams.Take.HasValue)
                    {
                        var query = db.Orders.Include(o => o.Customer)
                        .Take(orderParams.Take.Value)
                        .Where(o => o.OrderDate > dateFrom && o.OrderDate < dateTo)
                        .OrderBy(o => o.OrderID)
                        .Select(o => new OrderFields
                        {
                            Customer = o.Customer.CompanyName,
                            OrderDate = o.OrderDate,
                            Freight = o.Freight,
                            ShipCountry = o.ShipCountry,
                            ShipName = o.ShipName
                        });
                        orderInfo = query.ToList();
                    }
                    else
                    {
                        var query = db.Orders.Include(o => o.Customer)
                        .Where(o => o.OrderDate > dateFrom && o.OrderDate < dateTo)
                        .OrderBy(o => o.OrderID)
                        .Select(o => new OrderFields
                        {
                            Customer = o.Customer.CompanyName,
                            OrderDate = o.OrderDate,
                            Freight = o.Freight,
                            ShipCountry = o.ShipCountry,
                            ShipName = o.ShipName
                        });
                        orderInfo = query.ToList();
                    }
                }
            }
            return orderInfo;
        }
    }
}