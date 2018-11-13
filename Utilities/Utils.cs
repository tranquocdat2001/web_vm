using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Linq.Expressions;

namespace Utilities
{
    public static class Utils
    {
        public static string GetTimeAgoDisplay(DateTime date)
        {
            DateTime now = DateTime.Now;
            if (DateTime.Compare(now, date) >= 0)
            {
                TimeSpan s = DateTime.Now.Subtract(date);
                TimeSpan w = DateTime.Now.StartOfWeek(DayOfWeek.Monday).Subtract(date);

                int numberDays = (now - date).Days;

                int dayDiff = (int)s.TotalDays;
                int weekDiff = DateTime.Now.DayOfWeek == DayOfWeek.Monday ? (int)w.TotalDays + 7 : (int)w.TotalDays;

                int secDiff = (int)s.TotalSeconds;

                if (dayDiff == 0)
                {
                    if (secDiff < 60)
                    {
                        return "vừa xong";
                    }
                    if (secDiff < 120)
                    {
                        return "1 phút trước";
                    }
                    if (secDiff < 3600)
                    {
                        return string.Format("{0} phút trước", Math.Floor((double)secDiff / 60));
                    }
                    if (secDiff < 7200)
                    {
                        return "1 giờ trước";
                    }
                    if (secDiff < 86400)
                    {
                        return string.Format("{0} giờ trước", Math.Floor((double)secDiff / 3600));
                    }
                }
                if (numberDays == 1)
                {
                    return "Hôm qua lúc " + date.ToString("HH:mm");
                }
                if (dayDiff > 1 && weekDiff < 7)
                {
                    string dateOfWeek = string.Empty;
                    switch (date.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            dateOfWeek = "Thứ hai";
                            break;
                        case DayOfWeek.Tuesday:
                            dateOfWeek = "Thứ ba";
                            break;
                        case DayOfWeek.Wednesday:
                            dateOfWeek = "Thứ tư";
                            break;
                        case DayOfWeek.Thursday:
                            dateOfWeek = "Thứ năm";
                            break;
                        case DayOfWeek.Friday:
                            dateOfWeek = "Thứ sáu";
                            break;
                        case DayOfWeek.Saturday:
                            dateOfWeek = "Thứ bảy";
                            break;
                        case DayOfWeek.Sunday:
                            dateOfWeek = "Chủ nhật";
                            break;
                    }
                    return string.Format("{0} lúc {1}", dateOfWeek, date.ToString("HH:mm"));
                }
                if (dayDiff <= 7)
                {
                    return string.Format("{0} ngày trước", dayDiff);
                }
                //if (dayDiff < 31)
                //{
                //    //return string.Format("{0} tuần trước", Math.Ceiling((double)dayDiff / 7));
                //    return string.Format("{0} ngày trước", dayDiff);
                //}
                //if (dayDiff < 365)
                //{
                //    return string.Format("{0} tháng trước", Math.Ceiling((double)dayDiff / 30));
                //}
                //if (dayDiff >= 365)
                //{
                //    return string.Format("{0} {1}:{2}", date.ToString("dd/MM/yyyy"), date.Hour < 10 ? "0" + date.Hour : date.Hour + "", date.Minute < 10 ? "0" + date.Minute : date.Minute + "");
                //}
                //return date.ToString("dd/MM/yyyy <span>|</span> HH:mm");
                return string.Format("{0}<span>-</span>{1}", date.ToString("HH:mm"), date.ToString("dd/MM/yyyy"));
            }
            else
                return string.Empty;
        }

        public static string GetTimeAgoDisplay(long ticks)
        {
            if (ticks > 0)
            {
                var date = ConvertTicksToDateTime(ticks);
                if (date.HasValue)
                {
                    DateTime now = DateTime.Now;
                    if (DateTime.Compare(now, date.Value) >= 0)
                    {
                        TimeSpan s = DateTime.Now.Subtract(date.Value);
                        TimeSpan w = DateTime.Now.StartOfWeek(DayOfWeek.Monday).Subtract(date.Value);

                        int numberDays = (now - date).Value.Days;

                        int weekDiff = DateTime.Now.DayOfWeek == DayOfWeek.Monday ? (int)w.TotalDays + 7 : (int)w.TotalDays;

                        int dayDiff = (int)s.TotalDays;

                        int secDiff = (int)s.TotalSeconds;

                        if (dayDiff == 0)
                        {
                            if (secDiff < 60)
                            {
                                return "vừa xong";
                            }
                            if (secDiff < 120)
                            {
                                return "1 phút trước";
                            }
                            if (secDiff < 3600)
                            {
                                return string.Format("{0} phút trước", Math.Floor((double)secDiff / 60));
                            }
                            if (secDiff < 7200)
                            {
                                return "1 giờ trước";
                            }
                            if (secDiff < 86400)
                            {
                                return string.Format("{0} giờ trước", Math.Floor((double)secDiff / 3600));
                            }
                        }
                        if (numberDays == 1)
                        {
                            return "Hôm qua lúc " + date.Value.ToString("HH:mm");
                        }
                        if (dayDiff > 1 && weekDiff < 7)
                        {
                            string dateOfWeek = string.Empty;
                            switch (date.Value.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    dateOfWeek = "Thứ Hai";
                                    break;
                                case DayOfWeek.Tuesday:
                                    dateOfWeek = "Thứ Ba";
                                    break;
                                case DayOfWeek.Wednesday:
                                    dateOfWeek = "Thứ Tư";
                                    break;
                                case DayOfWeek.Thursday:
                                    dateOfWeek = "Thứ Năm";
                                    break;
                                case DayOfWeek.Friday:
                                    dateOfWeek = "Thứ Sáu";
                                    break;
                                case DayOfWeek.Saturday:
                                    dateOfWeek = "Thứ Bảy";
                                    break;
                                case DayOfWeek.Sunday:
                                    dateOfWeek = "Chủ Nhật";
                                    break;
                            }
                            return string.Format("{0} lúc {1}", dateOfWeek, date.Value.ToString("HH:mm"));
                        }
                        if (dayDiff <= 7)
                        {
                            return string.Format("{0} ngày trước", dayDiff);
                        }
                        //if (dayDiff < 31)
                        //{
                        //    //return string.Format("{0} tuần trước", Math.Ceiling((double)dayDiff / 7));
                        //    return string.Format("{0} ngày trước", dayDiff);
                        //}
                        //if (dayDiff < 365)
                        //{
                        //    return string.Format("{0} tháng trước", Math.Ceiling((double)dayDiff / 30));
                        //}
                        //if (dayDiff >= 365)
                        //{
                        //    return string.Format("{0} {1}:{2}", date.Value.ToString("dd/MM/yyyy"), date.Value.Hour < 10 ? "0" + date.Value.Hour : date.Value.Hour + "", date.Value.Minute < 10 ? "0" + date.Value.Minute : date.Value.Minute + "");
                        //}
                        return string.Format("{0} {1}:{2}", date.Value.ToString("dd/MM/yyyy"), date.Value.Hour < 10 ? "0" + date.Value.Hour : date.Value.Hour + "", date.Value.Minute < 10 ? "0" + date.Value.Minute : date.Value.Minute + "");
                    }
                    else
                        return string.Empty;
                }
            }
            return string.Empty;
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = startOfWeek - dt.DayOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(diff).Date;
        }

        public static string GetTimeDisplayByDay(long ticks)
        {
            if (ticks > 0)
            {
                var date = ConvertTicksToDateTime(ticks);
                if (date.HasValue)
                {
                    DateTime now = DateTime.Now;
                    if (DateTime.Compare(now, date.Value) >= 0)
                    {
                        TimeSpan s = DateTime.Now.Subtract(date.Value);

                        int dayDiff = (int)s.TotalDays;

                        int secDiff = (int)s.TotalSeconds;

                        if (dayDiff == 0)
                        {
                            if (secDiff < 60)
                            {
                                return "vừa xong";
                            }
                            if (secDiff < 120)
                            {
                                return "1 phút trước";
                            }
                            if (secDiff < 3600)
                            {
                                return string.Format("{0} phút trước", Math.Floor((double)secDiff / 60));
                            }
                            if (secDiff < 7200)
                            {
                                return "1 giờ trước";
                            }
                            if (secDiff < 86400)
                            {
                                return string.Format("{0} giờ trước", Math.Floor((double)secDiff / 3600));
                            }
                        }
                        else if (dayDiff == 1)
                        {
                            return date.Value.ToString("HH:mm") + " - Hôm qua";
                        }
                        else
                        {
                            return string.Format("{0}:{1} - {2}", date.Value.Hour < 10 ? "0" + date.Value.Hour : date.Value.Hour + "", date.Value.Minute < 10 ? "0" + date.Value.Minute : date.Value.Minute + "", date.Value.ToString("dd/MM/yyyy"));
                        }
                        return string.Empty;
                    }
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public static DateTime? ConvertTicksToDateTime(long lticks)
        {
            if (lticks == 0) return null;
            return new DateTime(lticks);
        }

        public static string ConvertTicksToStringFormat(long ticks)
        {
            if (ticks == 0) return string.Empty;
            var convertDate = ConvertTicksToDateTime(ticks);
            var strDateFormat = "{0} {1}";
            if (!convertDate.HasValue) return string.Empty;

            return string.Format(strDateFormat, convertDate.Value.ToString("dd/MM/yyyy"), convertDate.Value.ToString("HH:mm"));

        }

        public static string ConvertTicksToStringFormatWithDash(long ticks)
        {
            if (ticks == 0) return string.Empty;
            var convertDate = ConvertTicksToDateTime(ticks);
            var strDateFormat = "{0} - {1}";
            if (!convertDate.HasValue) return string.Empty;

            return string.Format(strDateFormat, convertDate.Value.ToString("dd/MM/yyyy"), convertDate.Value.ToString("HH:mm"));

        }

        public static string ConvertTicksToStringFormat(long ticks, string formatString)
        {
            if (ticks == 0) return string.Empty;
            var convertDate = ConvertTicksToDateTime(ticks);
            var strDateFormat = "{0}";
            if (!convertDate.HasValue) return string.Empty;
            return string.Format(strDateFormat, convertDate.Value.ToString(formatString));
        }

        public static string ConvertTicksToStringTimeFormat(long ticks)
        {
            if (ticks == 0) return string.Empty;
            var prefix = "";
            var convertDate = ConvertTicksToDateTime(ticks);
            if (!convertDate.HasValue) return string.Empty;

            int hour = convertDate.Value.Hour;
            if (hour >= 0 && hour <= 12) prefix = "sáng";
            if (hour >= 13 && hour <= 17) prefix = "chiều";
            if (hour >= 18 && hour <= 23) prefix = "tối";
            var strDateFormat = "lúc {0} " + prefix;
            return string.Format(strDateFormat, convertDate.Value.ToString("HH:mm"));
        }

        public static DateTime ConvertStringToDateTime(string value, string format)
        {
            return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
        }

        public static DateTime ConvertToDateTime(object value)
        {
            DateTime returnValue;

            if (null == value || !DateTime.TryParse(value.ToString(), out returnValue))
            {
                returnValue = DateTime.MinValue;
            }

            return returnValue;
        }

        /// <summary>
        /// Return double, > 0 then dateTo > dateFrom
        /// </summary>
        /// <pre>instant: d = Date, h = Hour, m = Minute, s = Second</pre>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="instant"></param>
        /// <returns></returns>
        public static double DateDiff(DateTime dateFrom, DateTime dateTo, string instant)
        {
            TimeSpan span = (TimeSpan)(dateTo - dateFrom);
            double num = 0.0;
            string str = instant.ToLower();
            if (str == null)
            {
                return num;
            }
            if (str != "d")
            {
                if (str != "h")
                {
                    if (str == "m")
                    {
                        return span.TotalMinutes;
                    }
                    if (str != "s")
                    {
                        return num;
                    }
                    return span.TotalSeconds;
                }
            }
            else
            {
                return span.TotalDays;
            }
            return span.TotalHours;
        }

        public static long DateTimeToUnixTime(DateTime dateTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = (TimeSpan)(dateTime - epoch.ToLocalTime());
            return (long)(span.TotalSeconds * 1000.0);
        }

        public static long DateTimeToUnixTimeDaily(DateTime dateTime)
        {
            dateTime = DateTime.Parse(dateTime.ToString("MM/dd/yyyy 00:00:00"));
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = (TimeSpan)(dateTime.Date - epoch.ToLocalTime());
            return (long)(span.TotalSeconds * 1000.0);
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp / 1000.0).ToLocalTime();
            return dtDateTime;
        }

        public static long DateTimeToSpanHourly(DateTime dateTime)
        {
            dateTime = DateTime.Parse(dateTime.ToString("MM/dd/yyyy HH:00:00"));
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, 0);
            TimeSpan span = (TimeSpan)(dateTime - time.ToLocalTime());
            return (long)(span.TotalSeconds * 1000.0);
        }

        public static bool IsLong(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            long longConverted = 0;
            if (long.TryParse(input, out longConverted)) return true;
            return false;
        }

        public static bool IsInteger(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            int integerConverted = 0;
            if (int.TryParse(input, out integerConverted)) return true;
            return false;
        }

        public static bool IsSizeFormat(string input)
        {

            if (input.IndexOf("x") != -1)
            {
                string[] sizes = input.Split('x');
                if (sizes.Length != 3) return false;
                else
                {
                    foreach (var size in sizes)
                    {
                        if (!IsInteger(size)) return false;
                    }
                    return true;
                }
            }
            return false;
        }

        public static string MySubString(string input, int length)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            StringBuilder __returnStr = new StringBuilder();
            string[] __separator = new string[] { " " };
            string[] __arrStr = input.Split(__separator, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < __arrStr.Length; i++)
            {
                if (i < length && i < __arrStr.Length)
                {
                    __returnStr.Append(__arrStr[i] + " ");

                }
                else
                {
                    break;
                }
            }
            return __returnStr.ToString();
        }

        public static string MySubStringNotHtml(string input, int length)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            String regex = "<a(.*?)</a>";
            Match math = Regex.Match(input, regex);
            String result = string.Empty;
            if (math.Success)
            {
                result = math.Groups[0].Value;
                input = Regex.Replace(input, "<a(.*?)</a>", "_temphtml_");
            }
            StringBuilder __returnStr = new StringBuilder();
            string[] __separator = new string[] { " " };
            string[] __arrStr = input.Split(__separator, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < __arrStr.Length; i++)
            {
                if (i < length && i < __arrStr.Length)
                {
                    __returnStr.Append(__arrStr[i] + " ");

                }
                else
                {
                    break;
                }
            }
            return __returnStr.Replace("_temphtml_", result).ToString();
        }

        public static string MySubStringWithDot(string input, int length)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            if (input.Length <= length)
            {
                return MySubStringNotHtml(input, length);
            }

            return string.Concat(MySubStringNotHtml(input, length).Trim(), "...");
        }

        public static string MySubCharater(string input, int length)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            if (input.Length <= length)
            {
                return input;
            }

            return input.Substring(0, length) + "...";
        }

        public static string PreProcessSearchString(string searchString)
        {
            string output = searchString.Replace("'", " ").Replace("\"\"", " ").Replace(">", " ").Replace("<", " ").Replace(",", " ").Replace("(", " ").Replace(")", " ").Replace("\"", " ");
            output = Regex.Replace(output, "[ ]+", "+");
            return output.Trim();
        }

        public static string GetMACAddress()
        {
            string str = string.Empty;
            try
            {
                NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface interface2 in allNetworkInterfaces)
                {
                    if (str == string.Empty)
                    {
                        interface2.GetIPProperties();
                        str = interface2.GetPhysicalAddress().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // no
            }
            return str;
        }

        public static string GetIPAddress()
        {
            try
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        string[] addresses = ipAddress.Split(',');
                        if (addresses.Length != 0)
                        {
                            return addresses[0];
                        }
                    }
                    return context.Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            catch (Exception ex)
            {
                //
            }
            return string.Empty;
        }

        public static string GetCssClassByTopicId(int topicId)
        {
            switch (topicId)
            {
                case 1:
                    return "boxtitle-furum";
                case 6:
                    return "boxtitle-tuvan";
                case 13:
                    return "boxtitle-baoduong";
                case 25:
                    return "boxtitle-traodoi";
                case 34:
                    return "boxtitle-giaoluu";
                default:
                    return "";
            }
        }

        public static string GetCurrentURL(string url, int? pageIndex)
        {
            if (url.IndexOf("/p" + pageIndex) != -1)
            {
                if (url.Substring(url.Length - (pageIndex.ToString().Length + 2)) == "/p" + pageIndex) return url.Substring(0, url.Length - (pageIndex.ToString().Length + 2));
            }
            return url;
        }

        public static int ConvertEnumToInt(Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        public static bool DelayBeforeContinue(string key, int delayInSeconds = 5)
        {
            var context = HttpContext.Current;
            if (context == null) return false;
            DateTime now = DateTime.Now;
            if (context.Session[key] != null)
            {
                DateTime sessionDateTime;
                if (DateTime.TryParse(context.Session[key].ToString(), out sessionDateTime))
                {
                    if (sessionDateTime.AddSeconds(delayInSeconds) < now)
                    {
                        context.Session.Remove(key);
                        return false;
                    }
                    else return true;
                }
            }
            else
            {
                context.Session.Add(key, now);
            }
            return false;
        }

        public static string GenPaggingAjax(string functionName, int pageIndex, int pageSize, int totalCount)
        {
            int pageNum = (int)Math.Ceiling((double)totalCount / pageSize);
            if (pageNum * pageSize < totalCount)
            {
                pageNum++;
            }
            string htmlPage = string.Empty;
            const string buildlink = "<li><a href=\"javascript:{0}('{1}')\" class=\"{2}\" title=\"{4}\">{3}</a></li>";
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            string currentPage = pageIndex.ToString(); // trang tiện tại
            int iCurrentPage = 0;
            if (pageIndex <= 0) iCurrentPage = 1;
            else iCurrentPage = pageIndex;
            string active;

            if (pageNum >= 2)
            {
                if (iCurrentPage == 1)
                {
                    htmlPage += String.Format(buildlink, "void", string.Empty, "hint", "<i class='fa fa-angle-double-left'></i>", "Trang đầu tiên");
                    htmlPage += String.Format(buildlink, "void", string.Empty, "hint", "<i class='fa fa-angle-left'></i>", "Trang trước");
                }
                else
                {
                    if ((iCurrentPage - 1) == 1)
                    {

                        htmlPage += String.Format(buildlink, functionName, string.Empty, string.Empty, "<i class='fa fa-angle-double-left'></i>", "Trang đầu tiên");
                        htmlPage += String.Format(buildlink, functionName, string.Empty, string.Empty, "<i class='fa fa-angle-left'></i>", "Trang trước");
                    }

                    else
                    {
                        htmlPage += String.Format(buildlink, functionName, 1, string.Empty, "<i class='fa fa-angle-double-left'></i>", "Trang đầu tiên");
                        htmlPage += String.Format(buildlink, functionName, (iCurrentPage - 1), string.Empty, "<i class='fa fa-angle-left'></i>", "Trang trước");
                    }
                }
            }

            if (pageNum <= 4)
            {
                if (pageNum != 1)
                {

                    for (int i = 1; i <= pageNum; i++)
                    {
                        active = currentPage == i.ToString() ? "active" : "";
                        if (i == 1) htmlPage += String.Format(buildlink, functionName, string.Empty, active, i, "Trang " + i);
                        else htmlPage += String.Format(buildlink, functionName, i, active, i, "Trang " + i);
                    }
                }
            }
            else
            {
                if (iCurrentPage < (pageNum - 3))
                {
                    if (iCurrentPage <= 3)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            active = currentPage == i.ToString() ? "active" : "";
                            if (i == 1) htmlPage += String.Format(buildlink, functionName, string.Empty, active, i, "Trang " + i);
                            else htmlPage += String.Format(buildlink, functionName, i, active, i, "Trang " + i);
                        }
                    }
                    else
                    {
                        for (int i = iCurrentPage - 2; i <= iCurrentPage + 2; i++)
                        {
                            active = currentPage == i.ToString() ? "active" : "";
                            htmlPage += String.Format(buildlink, functionName, i, active, i, "Trang " + i);
                        }
                    }
                }
                else
                {
                    for (int i = pageNum - 3; i <= pageNum; i++)
                    {
                        active = currentPage == i.ToString() ? "active" : "";
                        htmlPage += String.Format(buildlink, functionName, i, active, i, "Trang " + i);

                    }
                }

            }
            if (pageNum >= 2)
            {
                if (iCurrentPage == pageNum)
                {
                    htmlPage += String.Format(buildlink, "void", string.Empty, "hint", "<i class='fa fa-angle-right'></i>", "Trang sau");
                    htmlPage += String.Format(buildlink, "void", string.Empty, "hint", "<i class='fa fa-angle-double-right'></i>", "Trang cuối");
                }
                else
                {
                    htmlPage += String.Format(buildlink, functionName, (iCurrentPage + 1), string.Empty, "<i class='fa fa-angle-right'></i>", "Trang sau");
                    htmlPage += String.Format(buildlink, functionName, pageNum, string.Empty, "<i class='fa fa-angle-double-right'></i>", "Trang cuối");
                }
            }
            htmlPage = string.Format("<div class=\"paging\"><ul>{0}</ul></div>", htmlPage);
            return htmlPage;
        }

        public static string GenPagging(int pageIndex, int pageSize, string linkSite, int count, string paramNamePaging = "p")
        {
            int pageNum = (int)Math.Ceiling((double)count / pageSize);
            if (pageNum * pageSize < count)
            {
                pageNum++;
            }

            string prefix = linkSite.Contains("?") ? linkSite.Substring(linkSite.LastIndexOf("?", StringComparison.Ordinal), linkSite.Length - linkSite.LastIndexOf("?", StringComparison.Ordinal)).TrimEnd('/') : "";

            string strLinkSite = linkSite.Contains("?") ? Regex.Replace(linkSite.Substring(0, linkSite.LastIndexOf("?", StringComparison.Ordinal)), @"p\d+", "").TrimEnd('/') + "/" : linkSite.TrimEnd('/') + "/";

            linkSite = strLinkSite;

            string htmlPage = string.Empty;
            string buildlink = "<li class='{2}'><a href='{0}{1}' title='{4}'>{3}</a></li>";
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            string currentPage = pageIndex.ToString(); // trang tiện tại
            int iCurrentPage = 0;
            if (pageIndex <= 0) iCurrentPage = 1;
            else iCurrentPage = pageIndex;
            string active;
            if (pageNum >= 2)
            {
                if (iCurrentPage == 1)
                {
                    htmlPage += String.Format(buildlink, "javascript:;", string.Empty, "hint", "<i class='fa fa-angle-double-left'></i>", "Trang trước");
                }
                else
                {
                    if ((iCurrentPage - 1) == 1)
                        htmlPage += string.Format(buildlink, linkSite.TrimEnd('/'), string.Empty + prefix, string.Empty, "<i class='fa fa-angle-double-left'></i>", "Trang trước");
                    else
                        htmlPage += string.Format(buildlink, linkSite, prefix + paramNamePaging + (iCurrentPage - 1), string.Empty, "<i class='fa fa-angle-double-left'></i>", "Trang trước");
                }
            }
            if (pageNum <= 4)
            {
                if (pageNum != 1)
                {

                    for (int i = 1; i <= pageNum; i++)
                    {
                        active = currentPage == i.ToString() ? "active" : "";
                        if (i == 1) htmlPage += String.Format(buildlink, linkSite.TrimEnd('/'), string.Empty + prefix, active, i, "Trang " + i);
                        else htmlPage += String.Format(buildlink, linkSite, paramNamePaging + i + prefix, active, i, "Trang " + i);
                    }
                }
            }
            else
            {
                if (iCurrentPage < (pageNum - 3))
                {
                    if (iCurrentPage <= 3)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            active = currentPage == i.ToString() ? "active" : "";
                            if (i == 1) htmlPage += String.Format(buildlink, linkSite.TrimEnd('/'), string.Empty + prefix, active, i, "Trang " + i);
                            else htmlPage += String.Format(buildlink, linkSite, paramNamePaging + i + prefix, active, i, "Trang " + i);
                        }
                    }
                    else
                    {
                        for (int i = iCurrentPage - 2; i <= iCurrentPage + 2; i++)
                        {
                            active = currentPage == i.ToString() ? "active" : "";
                            htmlPage += String.Format(buildlink, linkSite, paramNamePaging + i + prefix, active, i, "Trang " + i);
                        }
                    }
                }
                else
                {
                    for (int i = pageNum - 3; i <= pageNum; i++)
                    {
                        active = currentPage == i.ToString() ? "active" : "";
                        htmlPage += String.Format(buildlink, linkSite, paramNamePaging + i + prefix, active, i, "Trang " + i);

                    }
                }

            }
            if (pageNum >= 2)
            {
                if (iCurrentPage == pageNum)
                {
                    htmlPage += String.Format(buildlink, "javascript:;", string.Empty, "hint", "<i class='fa fa-angle-double-right'></i>", "Trang sau");
                }
                else
                {
                    htmlPage += String.Format(buildlink, linkSite, paramNamePaging + (iCurrentPage + 1) + prefix, string.Empty, "<i class='fa fa-angle-double-right'></i>", "Trang sau");
                }
            }
            htmlPage = string.Format("<div class='paging'><ul>{0}</ul></div>", htmlPage);
            return htmlPage;
        }

        public static IEnumerable<SelectListItem> EnumToSelectList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = Convert.ToInt32(v).ToString()
            }).ToList();
        }

        public static IEnumerable<SelectListItem> EnumToSelectListDes<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(v => new SelectListItem
            {
                Text = StringUtils.GetEnumDescription((Enum)Enum.Parse(typeof(T), v.ToString(), true)),
                Value = Convert.ToInt32(v).ToString()
            }).ToList();
        }

        /*
        public static string GenarateCanonicalUrl(string standardUrl)
        {
            string strHostName = AppSettings.Instance.GetString("HostName");
            string strHostNameMobile = AppSettings.Instance.GetString("MobileHostName");
            string strBaseUrl = AppSettings.Instance.GetString("BaseUrl");
            string strBaseUrlMobile = AppSettings.Instance.GetString("MobileBaseUrl");

            bool isMobile = DetectDevice.Instance.BrowserIsMobile();
            string hostRequest = HttpContext.Current.Request.Url.Host.ToLower();

            hostRequest = hostRequest.Replace(string.Format(":{0}", HttpContext.Current.Request.Url.Port), string.Empty);

            string hostConfig = strHostName.ToLower();
            string mobileHostConfig = strHostNameMobile.ToLower();

            string baseUrl = strBaseUrl.ToLower();
            string mobileBaseUrl = strBaseUrlMobile.ToLower();

            if (hostConfig != mobileHostConfig)
            {
                if (hostRequest.Equals(hostConfig))
                {
                    isMobile = false;
                }
                else if (hostRequest.Equals(mobileHostConfig))
                {
                    isMobile = true;
                }
            }
            if (isMobile)
            {
                standardUrl = baseUrl + standardUrl.Replace(mobileHostConfig, string.Empty);
            }
            else
            {
                standardUrl = mobileBaseUrl + standardUrl.Replace(hostConfig, string.Empty);
            }
            return standardUrl;
        }
        */

        /// <summary>
        /// Decodes the query parameters.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">uri</exception>
        public static Dictionary<string, string> DecodeQueryParameters(this Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (uri.Query.Length == 0)
                return new Dictionary<string, string>();

            return uri.Query.TrimStart('?')
                            .Split(new[] { '&', ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(parameter => parameter.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                            .GroupBy(parts => parts[0],
                                        parts => parts.Length > 2 ? string.Join("=", parts, 1, parts.Length - 1) : (parts.Length > 1 ? parts[1] : ""))
                            .ToDictionary(grouping => grouping.Key,
                                            grouping => string.Join(",", grouping));
        }

        /// <summary>
        /// Dictionaries to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <returns></returns>
        public static T DictionaryToObject<T>(IDictionary<string, string> dict) where T : new()
        {
            var t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                KeyValuePair<string, string> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                // Find which property type (int, string, double? etc) the CURRENT property
                Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;

                // Fix nullables
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                // change the type
                object newA = Convert.ChangeType(item.Value, newT);
                t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
            }
            return t;
        }

        /// <summary>
        /// Replaces the case insensitive.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="search">The search.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>

        public static long CreateNewsId()
        {
            const string formatDate = "yyyyMMddHHmmssFFF";

            var dt = DateTime.Now;

            var dateToString = dt.ToString(formatDate);

            while (dateToString.Length < formatDate.Length)
            {
                dateToString += "0";
            }

            return Int64.Parse(dateToString);
        }

        public static bool IsNewsId(string str, out long newsId)
        {
            newsId = 0;
            if (str.Length == 17)
            {
                newsId = str.ToLong();
                if (newsId > 0) return true;
            }
            return false;
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 000);
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static string ExtractImageFromcontent(string content)
        {
            string firstImage = String.Empty;

            if (String.IsNullOrEmpty(content)) return firstImage;

            try
            {

                string strRegex = @"<img.+?src=[\""'](?<SRC>.+?)[\""'].*?>";
                Regex myRegex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                foreach (Match matchAvatar in myRegex.Matches(content))
                {
                    if (matchAvatar.Success)
                    {
                        firstImage = matchAvatar.Groups["SRC"].Value;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                // Todo
            }
            return firstImage;
        }

        public static string RenderRazorViewToString(this Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }
    }
}
