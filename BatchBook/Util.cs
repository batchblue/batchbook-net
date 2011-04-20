using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Configuration;

namespace BatchBook
{
    internal delegate void RequestDelegate(XmlWriter wrtr);
    internal delegate T ResponseDelegate<out T>(XmlReader rdr);
    internal static class Util
    {
        private static string s_defaultApiKey;
        private static string s_subdomain;
        private static readonly XmlWriterSettings s_xmlWriterSettings;
        private static readonly XmlReaderSettings s_xmlReaderSettings;

        static Util()
        {
            s_xmlWriterSettings = new XmlWriterSettings();
            s_xmlReaderSettings = new XmlReaderSettings();
            s_xmlReaderSettings.IgnoreWhitespace = true;
        }
        
        internal static string DefaultApiKey
        {
            get { return s_defaultApiKey ?? (s_defaultApiKey = ConfigurationManager.AppSettings["BatchBookApiKey"]); }
            set 
            { 
                s_defaultApiKey = value; 
            }
        }

        internal static string Subdomain
        {
            get { return s_subdomain ?? (s_subdomain = ConfigurationManager.AppSettings["BatchBookSubdomain"]); }
            set
            {
                s_subdomain = value;
            }
        }
       
        public static T Get<T>(string url, string apiKey, ResponseDelegate<T> responseDelegate)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.Credentials = new NetworkCredential(apiKey, "x");
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            using (Stream responseStream = webResponse.GetResponseStream())
            {
                if (responseStream != null)
                {
                    return responseDelegate(XmlReader.Create(responseStream, s_xmlReaderSettings));
                }
            }
            return default(T);
        }

        public static string Post(string url, string apiKey, RequestDelegate postDelegate)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.Credentials = new NetworkCredential(apiKey, "x");

            if (postDelegate != null)
            {
                byte[] content;
                using (MemoryStream itemStream = new MemoryStream())
                using (XmlWriter wrtrItem = XmlWriter.Create(itemStream, s_xmlWriterSettings))
                {
                    postDelegate(wrtrItem);
                    wrtrItem.Flush();
                    content = new byte[itemStream.Length];
                    itemStream.Seek(0, SeekOrigin.Begin);
                    itemStream.Read(content, 0, (int) itemStream.Length);
                }

                webRequest.ContentLength = content.Length;
                webRequest.ContentType = "application/xml";

                using (Stream requestStream = webRequest.GetRequestStream())
                {
#if DEBUG
                    string decodedContent = Encoding.UTF8.GetString(content);
                    System.Diagnostics.Debug.WriteLine(decodedContent);
#endif
                    requestStream.Write(content, 0, content.Length);
                }
            }
            else
                webRequest.ContentLength = 0;

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
#if DEBUG
            using (StreamReader rdr = new StreamReader(response.GetResponseStream()))
            {
                string returnXml = rdr.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(returnXml);
            }
#endif
            response.Close();


            return response.Headers["Location"];
        }

        public static void Put(string url, string apiKey, RequestDelegate putDelegate)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "PUT";
            webRequest.Credentials = new NetworkCredential(apiKey, "x");
            webRequest.ContentType = "application/xml";

            if (putDelegate != null)
            {
                byte[] content;
                using (MemoryStream itemStream = new MemoryStream())
                using (XmlWriter wrtrItem = XmlWriter.Create(itemStream, s_xmlWriterSettings))
                {
                    putDelegate(wrtrItem);
                    wrtrItem.Flush();
                    content = new byte[itemStream.Length];
                    itemStream.Seek(0, SeekOrigin.Begin);
                    itemStream.Read(content, 0, (int)itemStream.Length);
                }
                webRequest.ContentLength = content.Length;
                using (Stream requestStream = webRequest.GetRequestStream())
                {
#if DEBUG
                    string decodedContent = Encoding.UTF8.GetString(content);
                    System.Diagnostics.Debug.WriteLine(decodedContent);
#endif
                    requestStream.Write(content, 0, content.Length);
                }
            }
            else
                webRequest.ContentLength = 0;

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
#if DEBUG
            using (StreamReader rdr = new StreamReader(response.GetResponseStream()))
            {
                string returnXml = rdr.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(returnXml);
            }
#endif
            response.Close();
        }

        public static void Delete(string url, string apiKey, RequestDelegate deleteDelegate)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = "application/xml";
            webRequest.Method = "DELETE";
            webRequest.Credentials = new NetworkCredential(apiKey, "x");

            if (deleteDelegate != null)
            {
                byte[] content;
                using (MemoryStream itemStream = new MemoryStream())
                using (XmlWriter wrtrItem = XmlWriter.Create(itemStream, s_xmlWriterSettings))
                {
                    deleteDelegate(wrtrItem);
                    wrtrItem.Flush();
                    content = new byte[itemStream.Length];
                    itemStream.Seek(0, SeekOrigin.Begin);
                    itemStream.Read(content, 0, (int) itemStream.Length);
                }
                webRequest.ContentLength = content.Length;
                using (Stream requestStream = webRequest.GetRequestStream())
                {
#if DEBUG
                    string decodedContent = Encoding.UTF8.GetString(content);
                    System.Diagnostics.Debug.WriteLine(decodedContent);
#endif
                    requestStream.Write(content, 0, content.Length);
                }
            }
            else
                webRequest.ContentLength = 0;
            
            WebResponse response = webRequest.GetResponse();
            response.Close();
        }

        public static void Delete(string url, string apiKey)
        {
            Delete(url, apiKey, null);
        }

        public static string ToBatchBookFormat(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("MMM dd HH:mm:ss UTC yyyy");
        }

        public static DateTime FromBatchBookFormat(this string date)
        {
            DateTime localDate = DateTime.ParseExact(date, "ddd MMM dd HH:mm:ss UTC yyyy", CultureInfo.CurrentCulture);
            return new DateTime(localDate.Ticks, DateTimeKind.Utc);
        }

        
        public static DateTime FromBatchBookFormat2(this string date)
        {
            DateTime localDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss zzz", CultureInfo.CurrentCulture);
            return new DateTime(localDate.Ticks, DateTimeKind.Utc);
        }
    }
}
