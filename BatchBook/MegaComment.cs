using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace BatchBook
{
    public class MegaComment
    {
        internal static MegaComment[] BuildList(XmlReader rdr)
        {
            if (rdr.IsStartElement("mega_comments") && !rdr.IsEmptyElement)
            {
                rdr.ReadStartElement("mega_comments");
                List<MegaComment> megaComments = new List<MegaComment>();
                while (rdr.Name == "mega_comment")
                {
                    megaComments.Add(new MegaComment(rdr));
                }
                rdr.ReadEndElement();
                return megaComments.ToArray();
            }
            else
            {
                rdr.Skip();
                return new MegaComment[0];
            }
        }

        public static MegaComment Get(string apiKey, int commentId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/comments/{1}.xml", Util.Subdomain, commentId);
            return Util.Get(
                url,
                apiKey,
                rdr => new MegaComment(rdr));
        }

        public static MegaComment Get(int commentId)
        {
            return Get(Util.DefaultApiKey, commentId);
        }

        public MegaComment[] List(string apiKey, int page)
        {
            string url = String.Format("https://{0}.batchbook.com/service/comments.xml?page={1}", Util.Subdomain, page);
            return Util.Get(
                url,
                apiKey,
                BuildList);
        }

        public MegaComment[] List(int page)
        {
            return List(Util.DefaultApiKey, page);
        }

        public static void Update(
            string apiKey,
            int commentId,
            string comment)
        {
            string url = String.Format("https://{0}.batchbook.com/service/comments/{1}.xml", Util.Subdomain, commentId);
            Util.Put(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("comment");
                    wrtr.WriteElementString("comment", comment);
                    wrtr.WriteEndElement();
                });
        }

        public static void Update(
            int commentId,
            string comment)
        {
            Update(Util.DefaultApiKey, commentId, comment);
        }

        public static int CreateOnPerson(
            string apiKey,
            int personId,
            string comment)
        {
            string url = String.Format("https://{0}.batchbook.com/service/people/{1}/comments.xml", Util.Subdomain, personId);
            string location = Util.Post(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("comment");
                    wrtr.WriteElementString("comment", comment);
                    wrtr.WriteEndElement();
                });

            Match match = Regex.Match(location, @"\.batchbook\.com/service/comments/(\d+)\.xml");
            if (match.Success)
                return Convert.ToInt32(match.Groups[1].Captures[0].Value);
            else
                throw new Exception("Unable to parse location string.");
        }

        public static int CreateOnPerson(
            int personId,
            string comment)
        {
            return CreateOnPerson(Util.DefaultApiKey, personId, comment);
        }

        public static int CreateOnCompany(
            string apiKey,
            int companyId,
            string comment)
        {
            string url = String.Format("https://{0}.batchbook.com/service/companies/{1}/comments.xml", Util.Subdomain, companyId);
            string location = Util.Post(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("comment");
                    wrtr.WriteElementString("comment", comment);
                    wrtr.WriteEndElement();
                });

            Match match = Regex.Match(location, @"\.batchbook\.com/service/comments/(\d+)\.xml");
            if (match.Success)
                return Convert.ToInt32(match.Groups[1].Captures[0].Value);
            else
                throw new Exception("Unable to parse location string.");
        }

        public static int CreateOnCompany(
            int companyId,
            string comment)
        {
            return CreateOnCompany(Util.DefaultApiKey, companyId, comment);
        }

        public static int CreateOnCommunication(
            string apiKey,
            int communicationId,
            string comment)
        {
            string url = String.Format("https://{0}.batchbook.com/service/communications/{1}/comments.xml", Util.Subdomain, communicationId);
            string location = Util.Post(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("comment");
                    wrtr.WriteElementString("comment", comment);
                    wrtr.WriteEndElement();
                });

            Match match = Regex.Match(location, @"\.batchbook\.com/service/comments/(\d+)\.xml");
            if (match.Success)
                return Convert.ToInt32(match.Groups[1].Captures[0].Value);
            else
                throw new Exception("Unable to parse location string.");
        }

        public static int CreateOnCommunication(
            int communicationId,
            string comment)
        {
            return CreateOnCommunication(Util.DefaultApiKey, communicationId, comment);
        }

        public static int CreateOnDeal(
            string apiKey,
            int dealId,
            string comment)
        {
            string url = String.Format("https://{0}.batchbook.com/service/deals/{1}/comments.xml", Util.Subdomain, dealId);
            string location = Util.Post(
                url,
                apiKey,
                delegate(XmlWriter wrtr)
                {
                    wrtr.WriteStartElement("comment");
                    wrtr.WriteElementString("comment", comment);
                    wrtr.WriteEndElement();
                });

            Match match = Regex.Match(location, @"\.batchbook\.com/service/comments/(\d+)\.xml");
            if (match.Success)
                return Convert.ToInt32(match.Groups[1].Captures[0].Value);
            else
                throw new Exception("Unable to parse location string.");
        }

        public static int CreateOnDeal(
            int dealId,
            string comment)
        {
            return CreateOnDeal(Util.DefaultApiKey, dealId, comment);
        }
        
        public static void Destroy(string apiKey, int commentId)
        {
            string url = String.Format("https://{0}.batchbook.com/service/comments/{1}.xml", Util.Subdomain, commentId);
            Util.Delete(url, apiKey);
        }

        public static void Destroy(int commentId)
        {
            Destroy(Util.DefaultApiKey, commentId);
        }

        internal MegaComment(XmlReader rdr)
        {
            rdr.ReadStartElement("mega_comment");
            this.Id = rdr.ReadElementContentAsInt("id", "");
            this.Comment = rdr.ReadElementString("comment");
            this.User = rdr.ReadElementString("user");
            this.CreatedAt = rdr.ReadElementString("created_at").FromBatchBookFormat2();
            this.RecordId = int.Parse(rdr.ReadElementString("record_id"));
            rdr.ReadEndElement();
        }

        public int Id
        {
            get;
            protected set;
        }

        public string Comment
        {
            get;
            protected set;
        }

        public string User
        {
            get;
            protected set;
        }

        public DateTime CreatedAt
        {
            get;
            protected set;
        }

        public int RecordId 
        {
            get;
            protected set;
        }
    }

}
