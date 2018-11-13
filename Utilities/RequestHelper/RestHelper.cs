using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using RestSharp;

namespace Utilities.RequestHelper
{

    public class RestHelper : IRestHelper
    {
        private string _baseUrl = "";

        public RestHelper()
        {

        }

        public RestHelper(string baseurl)
        {
            this._baseUrl = baseurl;
        }

        public T PostRequest<T>(string action, object data)
        {
            try
            {
                RestClient client = new RestClient
                {
                    BaseUrl = new Uri(_baseUrl)

                };
                IRestRequest request = new RestRequest
                {
                    Resource = action,
                    RequestFormat = DataFormat.Json,
                    Method = Method.POST
                };

                request.AddHeader("Accept", "application/json");

                request.AddBody(data);

                IRestResponse response = client.Execute(request);

                return NewtonJson.Deserialize<T>(response.Content);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }

            return default(T);
        }

        public T GetRequest<T>(string action)
        {
            try
            {
                RestClient client = new RestClient
                {
                    BaseUrl = new Uri(_baseUrl)

                };
                IRestRequest request = new RestRequest
                {
                    Resource = action,
                    RequestFormat = DataFormat.Json,
                    Method = Method.GET
                };

                request.AddHeader("Accept", "application/json");

                IRestResponse response = client.Execute(request);

                return NewtonJson.Deserialize<T>(response.Content);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }

            return default(T);
        }

        public string PostRequest(string action, object data)
        {
            try
            {
                RestClient client = new RestClient
                {
                    BaseUrl = new Uri(_baseUrl)

                };
                IRestRequest request = new RestRequest
                {
                    Resource = action,
                    RequestFormat = DataFormat.Json,
                    Method = Method.POST
                };

                request.AddHeader("Accept", "application/json");

                request.AddBody(data);

                IRestResponse response = client.Execute(request);

                return response.Content;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }

            return string.Empty;
        }

        public string GetRequest(string action)
        {
            try
            {
                RestClient client = new RestClient
                {
                    BaseUrl = new Uri(_baseUrl)

                };
                IRestRequest request = new RestRequest
                {
                    Resource = action,
                    RequestFormat = DataFormat.Json,
                    Method = Method.GET
                };

                request.AddHeader("Accept", "application/json");

                IRestResponse response = client.Execute(request);

                return response.Content;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }

            return string.Empty;
        }

        public Utilities.ResponseData MakeRequest(string url, IEnumerable<HeaderCustom> headerCustoms)
        {
            Utilities.ResponseData responseData = new Utilities.ResponseData();
            try
            {
                var client = new RestClient(url);

                var request = new RestRequest(Method.GET);

                if (headerCustoms != null && headerCustoms.Any())
                {
                    foreach(HeaderCustom item in headerCustoms)
                    {
                        request.AddHeader(item.Name, item.Value);
                    }
                }

                IRestResponse response = client.Execute(request);

                responseData.Data = response.StatusCode;
                responseData.Content = response.Content;
                responseData.Success = true;
                responseData.Message = "Success";
            }
            catch (Exception ex)
            {
                responseData.Success = false;
                responseData.Message = ex.Message;
            }
            return responseData;
        }

        public struct HeaderCustom
        {
            public string Name;
            public string Value;
        }
    }
}
