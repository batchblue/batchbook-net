using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace BatchBook
{
    public class Tag
    {
        internal static Tag[] BuildList(XmlReader rdr)
        {
            if (rdr.IsStartElement("tags") && !rdr.IsEmptyElement)
            {
                rdr.ReadStartElement("tags");
                List<Tag> tags = new List<Tag>();
                while (rdr.Name == "tag")
                {
                    tags.Add(new Tag(rdr));
                }
                rdr.ReadEndElement();
                return tags.ToArray();
            }
            else
            {
                rdr.Skip();
                return new Tag[0];
            }
        }

        internal static void SerializeList(string[] tags, XmlWriter wrtr)
        {
            wrtr.WriteStartElement("tags");
            foreach (string tag in tags)
            {
                wrtr.WriteElementString("tag", tag);
            }
            wrtr.WriteEndElement();
        }

        public static Tag Get(string apiKey, string tagName)
        {
            string url = String.Format("https://{0}.batchbook.com/service/tags/{1}.xml", Util.Subdomain, HttpUtility.UrlEncode(tagName));
            return Util.Get(
                url,
                apiKey,
                rdr => new Tag(rdr));
        }

        public static Tag Get(string tagName)
        {
            return Get(Util.DefaultApiKey, tagName);
        }

        public static Tag[] List(string apiKey)
        {
            string url = String.Format("https://{0}.batchbook.com/service/tags.xml", Util.Subdomain);
            return Util.Get(url, apiKey, BuildList);
        }

        public static Tag[] List()
        {
            return List(Util.DefaultApiKey);
        }

        public static void AddToPerson(string apiKey, int personId, params string[] tags)
        {
            string url = string.Format("https://{0}.batchbook.com/service/people/{1}/add_tag.xml", Util.Subdomain, personId);
            Util.Put(
                url,
                apiKey,
                wrtr => SerializeList(tags, wrtr));
        }

        public static void AddToPerson(int personId, params string[] tags)
        {
            AddToPerson(Util.DefaultApiKey, personId, tags);
        }

        public static void AddToCompany(string apiKey, int companyId, params string[] tags)
        {
            string url = string.Format("https://{0}.batchbook.com/service/companies/{1}/add_tag.xml", Util.Subdomain, companyId);
            Util.Put(
                url,
                apiKey,
                wrtr => SerializeList(tags, wrtr));
        }

        public static void AddToCompany(int companyId, params string[] tags)
        {
            AddToCompany(Util.DefaultApiKey, companyId, tags);
        }

        public static void AddToDeal(string apiKey, int dealId, params string[] tags)
        {
            string url = string.Format("https://{0}.batchbook.com/service/deals/{1}/add_tag.xml", Util.Subdomain, dealId);
            Util.Put(
                url,
                apiKey,
                wrtr => SerializeList(tags, wrtr));
        }

        public static void AddToDeal(int dealId, params string[] tags)
        {
            AddToDeal(Util.DefaultApiKey, dealId, tags);
        }

        public static void AddToCommunication(string apiKey, int communicationId, params string[] tags)
        {
            string url = string.Format("https://{0}.batchbook.com/service/communications/{1}/add_tag.xml", Util.Subdomain, communicationId);
            Util.Put(
                url,
                apiKey,
                wrtr => SerializeList(tags, wrtr));
        }

        public static void AddToCommunication(int communicationId, params string[] tags)
        {
            AddToCommunication(Util.DefaultApiKey, communicationId, tags);
        }

        public static void RemoveFromPerson(string apiKey, int personId, string tag)
        {
            string url = string.Format("https://{0}.batchbook.com/service/people/{1}/remove_tag.xml", Util.Subdomain, personId);
            Util.Delete(
                url,
                apiKey,
                wrtr => wrtr.WriteElementString("tag", tag));
        }

        public static void RemoveFromPerson(int personId, string tag)
        {
            RemoveFromPerson(Util.DefaultApiKey, personId, tag);
        }

        public static void RemoveFromCompany(string apiKey, int companyId, string tag)
        {
            string url = string.Format("https://{0}.batchbook.com/service/companies/{1}/remove_tag.xml", Util.Subdomain, companyId);
            Util.Delete(
                url,
                apiKey,
                wrtr => wrtr.WriteElementString("tag", tag));
        }

        public static void RemoveFromCompany(int companyId, string tag)
        {
            RemoveFromCompany(Util.DefaultApiKey, companyId, tag);
        }

        public static void RemoveFromDeal(string apiKey, int dealId, string tag)
        {
            string url = string.Format("https://{0}.batchbook.com/service/deals/{1}/remove_tag.xml", Util.Subdomain, dealId);
            Util.Delete(
                url,
                apiKey,
                wrtr => wrtr.WriteElementString("tag", tag));
        }

        public static void RemoveFromDeal(int dealId, string tag)
        {
            RemoveFromDeal(Util.DefaultApiKey, dealId, tag);
        }

        public static void RemoveFromCommunication(string apiKey, int communicationId, string tag)
        {
            string url = string.Format("https://{0}.batchbook.com/service/communications/{1}/remove_tag.xml", Util.Subdomain, communicationId);
            Util.Delete(
                url,
                apiKey,
                wrtr => wrtr.WriteElementString("tag", tag));
        }

        public static void RemoveFromCommunication(int communicationId, string tag)
        {
            RemoveFromCommunication(Util.DefaultApiKey, communicationId, tag);
        }

        internal Tag(XmlReader rdr)
        {
            rdr.ReadStartElement("tag");
            this.Id = rdr.ReadElementContentAsInt("id", "");
            this.Name = rdr.ReadElementString("name");
            this.IsSupertag = Boolean.Parse(rdr.ReadElementString("supertag"));
            if (this.IsSupertag)
            {
                this.Fields = new Dictionary<string, string>();
                rdr.ReadStartElement("fields");
                while (rdr.NodeType != XmlNodeType.EndElement || rdr.Name != "fields")
                {
                    this.Fields.Add(rdr.Name, rdr.ReadElementString());
                }
                rdr.ReadEndElement();
            }
            rdr.ReadEndElement();
        }
        
        public int Id
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public bool IsSupertag
        {
            get;
            protected set;
        }

        public Dictionary<string, string> Fields
        {
            get;
            protected set;
        }
    }
}
