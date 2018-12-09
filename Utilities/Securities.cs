using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Utilities
{
    public static class KillCrawlerHelper
    {
        public static DateTime startDateRun = DateTime.MinValue;

        public static bool Running = false;

        public static List<string> QuickBlockIPDetected = new List<string>();

        public static Dictionary<string, CrawlerIPMonitor> IPCrawlerDetected = new Dictionary<string, CrawlerIPMonitor>();

        public static void Count(string Ip, string useragent)
        {
            try
            {
                if (Running)
                {

                    if (IPCrawlerDetected.ContainsKey(Ip))
                    {
                        var ipobj = IPCrawlerDetected[Ip];
                        ipobj.Count++;
                        IPCrawlerDetected[Ip] = ipobj;
                    }
                    else
                    {
                        IPCrawlerDetected.Add(Ip, new CrawlerIPMonitor() { Count = 1, StartRequest = DateTime.Now, StartUrl = useragent });

                        if (IPCrawlerDetected.Count > 2000)
                        {
                            IPCrawlerDetected.Clear();
                        }
                    }
                }
            }
            catch (Exception)
            {
                //
            }
        }

        public static string ToDes()
        {
            string ip = string.Empty;
            var lst = KillCrawlerHelper.IPCrawlerDetected.OrderByDescending(obj => obj.Value.Count).ToList();

            foreach (var pair in lst)
            {
                ip += string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", pair.Key, pair.Value.Count, pair.Value.StartRequest.ToString("dd/MM/yyyy HH:mm:ss"), pair.Value.StartUrl);
            }

            if (startDateRun != DateTime.MinValue)
            {
                ip += string.Format("<table border=\"1\"><tr><td>{0}</td></tr></table>", (int)(DateTime.Now - startDateRun).TotalSeconds);
            }

            return string.Format("<html><body><table border=\"1\">{0}</table></body></html>", ip);
        }

        public static string ToListDenied()
        {
            string retval = string.Empty;

            foreach (var ip in QuickBlockIPDetected)
            {
                retval += string.Format("{0}<br/>", ip);
            }

            return string.Format("<html><body>{0}</body></html>", retval);
        }

        public static string GetIPClient(this HttpRequest rq)
        {
            if (string.IsNullOrEmpty(rq.Headers["X-Forwarded-For"]) == false)
            {
                return rq.Headers["X-Forwarded-For"];
            }
            else
            {
                return rq.UserHostAddress;
            }
        }

        public static void CrawlerIPDetected()
        {
            HttpContext context = HttpContext.Current;
            var response = context.Response;
            var request = context.Request;
            string _url = request.Url.AbsoluteUri;
            try
            {
                if (string.IsNullOrEmpty(context.Request.UserAgent) || string.IsNullOrWhiteSpace(context.Request.UserAgent))
                {
                    response.Write("sorry! something is wrong.");
                    response.End();
                    return;
                }

                if (QuickBlockIPDetected.Count > 0)
                {
                    if (QuickBlockIPDetected.Contains(request.GetIPClient()))
                    {
                        response.Write("sorry! something is wrong.");
                        response.End();
                        return;
                    }
                }

                Count(request.GetIPClient(), request.UserAgent);
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
            }
            if (_url.Contains("/crawleripdetected"))
            {
                try
                {
                    if (_url.Contains("Run"))
                    {
                        KillCrawlerHelper.Running = true;
                        KillCrawlerHelper.startDateRun = DateTime.Now;
                    }
                    else if (_url.Contains("Stop"))
                    {
                        KillCrawlerHelper.Running = false;
                    }

                    else if (_url.Contains("Clear"))
                        KillCrawlerHelper.IPCrawlerDetected.Clear();
                    else if (_url.Contains("Add"))
                    {
                        string ip = context.Request.Path.Replace("/crawleripdetectedAdd", "");
                        if (KillCrawlerHelper.QuickBlockIPDetected.Contains(ip) == false)
                        {
                            KillCrawlerHelper.QuickBlockIPDetected.Add(ip);
                        }
                    }
                    else if (_url.Contains("RemoveAll"))
                    {
                        KillCrawlerHelper.QuickBlockIPDetected.Clear();
                        response.Write(KillCrawlerHelper.ToListDenied());
                    }
                    else if (_url.Contains("Remove"))
                    {
                        string ip = context.Request.Path.Replace("/crawleripdetectedRemove", "");
                        KillCrawlerHelper.QuickBlockIPDetected.Remove(ip);
                        response.Write(KillCrawlerHelper.ToListDenied());
                    }
                    else if (_url.Contains("List"))
                    {
                        response.Write(KillCrawlerHelper.ToListDenied());
                    }
                    else
                    {
                        response.Write(KillCrawlerHelper.ToDes());
                    }
                }
                catch (Exception ex)
                {
                    response.Write(ex.Message + "<br/>" + ex.StackTrace);
                }
                response.Charset = "UTF-8";
                response.ContentType = "text/html";
                response.End();
                return;
            }
        }
    }

    public struct CrawlerIPMonitor
    {
        public int Count { get; set; }
        public DateTime StartRequest { get; set; }
        public string StartUrl { get; set; }
    }
}
