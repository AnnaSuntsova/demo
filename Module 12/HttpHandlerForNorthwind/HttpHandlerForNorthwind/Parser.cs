using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace HttpHandlerForNorthwind
{
    public class Parser
    {
        public OrderParameters ParseBody(Stream inputStream)
        {
            using (var reader = new StreamReader(inputStream))
            {
                var data = reader.ReadToEnd().Split('&');

                var orderParams = new OrderParameters
                {
                    CustomerId = GetStringParameter("customerId", data),
                    DateFrom = GetDateTimeParameter("dateFrom", data),
                    DateTo = GetDateTimeParameter("dateTo", data),
                    Skip = GetIntParameter("skip", data),
                    Take = GetIntParameter("take", data)
                };

                return orderParams;
            }
        }

        public OrderParameters ParseQuery(NameValueCollection queryStrings)
        {
            if (queryStrings == null || queryStrings.Count == 0) return null;

            var orderParams = new OrderParameters();
            orderParams.CustomerId = queryStrings["CustomerId"];

            if (DateTime.TryParse(queryStrings["dateFrom"], out var fromDateTime))
            {
                orderParams.DateFrom = (DateTime?)fromDateTime;
            }
            else
            {
                orderParams.DateFrom = null;
            }

            if (DateTime.TryParse(queryStrings["dateTo"], out var toDateTime))
            {
                orderParams.DateFrom = (DateTime?)toDateTime;
            }
            else
            {
                orderParams.DateFrom = null;
            }

            if (int.TryParse(queryStrings["skip"], out var skipRes))
            {
                orderParams.Skip = skipRes;
            }
            else
            {
                orderParams.Skip = null;
            }

            if (int.TryParse(queryStrings["take"], out var takeRes))
            {
                orderParams.Skip = takeRes;
            }
            else
            {
                orderParams.Skip = null;
            }

            return orderParams;
        }

        private int? GetIntParameter(string name, string[] data)
        {
            var parameterValue = data.FirstOrDefault(param => param.StartsWith(name))?.Split('=');
            if (parameterValue != null)
            {
                bool success = int.TryParse(parameterValue[1], out var result);
                if (success)
                {
                    return result;
                }
            }
            return null;
        }

        private DateTime? GetDateTimeParameter(string name, string[] data)
        {
            var parameterValue = data.FirstOrDefault(param => param.StartsWith(name))?.Split('=');
            if (parameterValue != null)
            {
                bool success = DateTime.TryParse(parameterValue[1], out var result);
                if (success)
                {
                    return result;
                }
            }
            return null;
        }

        private string GetStringParameter(string name, string[] data)
        {
            var parameterValue = data.FirstOrDefault(param => param.StartsWith(name))?.Split('=');
            if (parameterValue != null) return parameterValue[1];
            return null;
        }
    }
}