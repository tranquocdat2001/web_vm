using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Utilities.XmlSiteMap
{
    public static class SiteMapHelper
    {
        public static string GetXmlContentFromPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;

            if (!File.Exists(path)) return string.Empty;

            using (StreamReader reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }

        public static T GetXmlContentFromPath<T>(string path)
        {
            if (string.IsNullOrEmpty(path)) return default(T);

            if (!File.Exists(path)) return default(T);

            using (StreamReader reader = new StreamReader(path))
            {
                string xmlContent = reader.ReadToEnd();

                return XmlDeserializeFromString<T>(xmlContent);
            }
        }

        public static bool SetXmlContentToPath(string path, object objectInstance)
        {
            if (string.IsNullOrEmpty(path)) return false;

            string xmlContent = XmlSerializeToString(objectInstance);

            if (!string.IsNullOrEmpty(xmlContent))
            {
                XmlDocument xdoc = new XmlDocument();

                xdoc.LoadXml(xmlContent);

                using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.Write(xmlContent);
                }

            }

            return true;
        }

        public static bool SetXmlContentToPath(string path, string xmlContent)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(xmlContent)) return false;

            if (!string.IsNullOrEmpty(xmlContent))
            {
                XmlDocument xdoc = new XmlDocument();

                xdoc.LoadXml(xmlContent);

                using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
                {
                    sw.Write(xmlContent);
                }

            }

            return true;
        }

        public static string XmlSerializeToString(this object objectInstance)
        {
            XmlSerializer serializer = new XmlSerializer(objectInstance.GetType());

            using (StringWriter writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, objectInstance);
                return writer.ToString();
            }
        }

        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
    }
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
}
