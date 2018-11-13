using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Utilities
{
    public sealed class CreateLogFiles
    {
        private static readonly Lazy<CreateLogFiles> lazy = new Lazy<CreateLogFiles>(() => new CreateLogFiles());
        public static CreateLogFiles Instance { get { return lazy.Value; } }
        public CreateLogFiles()
        {
        }
        private static byte isErrorLog = 0;
        private static byte isInfoLog = 0;
        private static readonly object padlock = new object();
        private static string applicationLocation = System.Reflection.Assembly.GetEntryAssembly().Location;
        private static string sPathName = Path.GetDirectoryName(applicationLocation) + "\\ErrorLog";
        public void ErrorLog(string sErrMsg)
        {
            lock (padlock)
            {
                if (isErrorLog == 0)
                {
                    if (ConfigurationManager.AppSettings["IsErrorLog"].ToString().ToLower() == "true")
                    {
                        isErrorLog = 1;
                    }
                    else isErrorLog = 2;
                }
                if (isErrorLog == 1)
                {
                    string sLogFormat = Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                    string sYear = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    string sMonth = DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    string sDay = DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    string sErrorTime = sYear + sMonth + sDay;
                    var sw = new StreamWriter(sPathName + "\\Error_" + sErrorTime + ".txt", true);
                    sw.WriteLine(Environment.NewLine + sLogFormat + sErrMsg);
                    sw.Flush();
                    sw.Close();
                }
            }
        }
        public void InfoLog(string sErrMsg)
        {
            lock (padlock)
            {
                if (isInfoLog == 0)
                {
                    if (ConfigurationManager.AppSettings["IsInfoLog"].ToString().ToLower() == "true")
                    {
                        isInfoLog = 1;
                    }
                    else isInfoLog = 2;
                }
                if (isInfoLog == 1)
                {
                    string sLogFormat = Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                    string sYear = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    string sMonth = DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    string sDay = DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    string sErrorTime = sYear + sMonth + sDay;
                    var sw = new StreamWriter(sPathName + "\\Info_" + sErrorTime + ".txt", true);
                    sw.WriteLine(Environment.NewLine + sLogFormat + sErrMsg);
                    sw.Flush();
                    sw.Close();
                }
            }
        }
    }
}
